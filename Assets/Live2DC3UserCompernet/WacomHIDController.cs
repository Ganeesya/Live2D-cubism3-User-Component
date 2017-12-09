using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;

public class WacomHIDController : MonoBehaviour
{
	public int venderID;
	public int productID;
	public int usage;
	public int usagePage;

	public byte[] buffer;

	public string debugdata;

	[DllImport("hidApiPicker")] private static extern void InitHidPicker();

	[DllImport("hidApiPicker")] private static extern int getReadSize();

	[DllImport("hidApiPicker")] private static extern bool findDevice(int vid, int pid, int usa, int usape);

	[DllImport("hidApiPicker")] private static extern void CloseDHandle();

	[DllImport("hidApiPicker")] private static extern bool ReadData(out byte[] buscket);

	private Thread readingThread;

	// Use this for initialization
	void Start () {
		InitHidPicker();
		if(findDevice(venderID,productID,usage,usagePage) )
		{
			buffer = new byte[getReadSize()];
			readingThread = new Thread(new ThreadStart(Reading));
			readingThread.Start();
		}
	}

	void Reading()
	{
		while (true)
		{
			ReadData(out buffer);
		}
	}
	
	// Update is called once per frame
	void Update () {
		debugdata = "";
		foreach(var e in buffer)
		{
			debugdata = debugdata + e.ToString();
		}
	}

	private void OnDestroy()
	{
		CloseDHandle();
	}
}
