using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading;
using Live2D.Cubism.Core;
using System;

public class CubismMtnMotionRecoder : MonoBehaviour
{
	public int RecodeFPS = 30;
	public string RecodeFolderPath;
	public string RecodeFileName;

	private float lastRecTime;
	private BinaryWriter buffFile;
	private CubismParameter[] cubismParams;
	private Thread recodeThead;

	public void Start()
	{
		cubismParams = this.FindCubismModel().Parameters;
	}

	// Update is called once per frame
	void Update()
	{
		if (buffFile != null)
		{
			recoding();
		}
	}
	
	public void recodeStart()
	{
		lastRecTime = Time.time * 1000;

		FileStream fs = new FileStream(RecodeFolderPath + RecodeFileName + ".mtl", FileMode.Create, FileAccess.Write);
		buffFile = new BinaryWriter(fs);
		foreach (var e in cubismParams)
		{
			buffFile.Write(e.Value);
		}
	}
	
	public void recoding()
	{
		if (Time.time * 1000 - lastRecTime < RecodeFPS)
		{
			return;
		}
		lastRecTime += RecodeFPS;
		foreach (var e in cubismParams)
		{
			buffFile.Write(e.Value);
		}
	}

	public void recodeEndStart()
	{
		buffFile.Close();
		buffFile = null;

		recodeThead = new Thread(new ThreadStart(recodeCombertThread));

		recodeThead.Start();
	}

	public void recodeCombertThread()
	{
		FileStream mtnf = new FileStream(RecodeFolderPath + RecodeFileName + DateTime.Now.Year + "_"
			+ DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second
			+ ".mtn", FileMode.Create, FileAccess.Write);
		StreamWriter mtnsw = new StreamWriter(mtnf);
		mtnsw.WriteLine("# Live2D Animator Motion Data");
		mtnsw.WriteLine("$fps=30");
		mtnsw.WriteLine("");
		mtnsw.WriteLine("$fadein=0");
		mtnsw.WriteLine("");
		mtnsw.WriteLine("$fadeout=0");
		mtnsw.WriteLine("");

		int i = 0;
		foreach (var e in cubismParams)
		{ 
			FileStream fs = new FileStream(RecodeFolderPath + RecodeFileName + ".mtl", FileMode.Open, FileAccess.Read);

			BinaryReader br = new BinaryReader(fs);
			string val = "";
			try
			{
				mtnsw.Write(e.Id + "=");
				while (true)
				{
					int x = 0;
					for (; x < i; x++)
					{
						br.ReadSingle();
					}
					val = br.ReadSingle().ToString("0.###");
					x++;
					mtnsw.Write(val + ",");
					for (; x < cubismParams.Length; x++)
					{
						br.ReadSingle();
					}
				}
			}
			catch (EndOfStreamException x)
			{ }
			finally
			{
				mtnsw.Write(val + "\n");
				br.Close();
			}
			mtnsw.WriteLine("");
			mtnsw.WriteLine("");
			i++;
		}
		File.Delete(RecodeFolderPath + RecodeFileName + ".mtl");
		mtnsw.Close();
	}
}
