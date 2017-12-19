﻿using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Json;


namespace Ganeesyan.Cubism3Compornets
{
	public class CubismPose
	{
		public Dictionary<string, float> setting = new Dictionary<string, float>();

		public static CubismPose LoadCubismPoseFromAssetPath(string assetpath)
		{
			CubismPose cubismPose = null;

			string loadedJson = null;

#if UNITY_EDITOR
			loadedJson = File.ReadAllText(assetpath);
#else
            loadedJson = Resources.Load(assetpath, typeof(TextAsset)) as TextAsset;			
#endif
			if(loadedJson == null)
			{
				Debug.Log("CubismPose: input path is no good.");
				return null;
			}

			CubismMotion3Json json = CubismMotion3Json.LoadFrom(loadedJson);

			if( json == null)
			{
				Debug.Log("CubismPose: i feel input json is not json.");
				return null;
			}

			cubismPose = new CubismPose();

			foreach (var curbe in json.Curves)
			{
				if((curbe.Target == "Parameter") &&(curbe.Segments.Length > 2))
				{
					cubismPose.setting.Add(curbe.Id, curbe.Segments[1]);
				}
			}

			return cubismPose;
		}	
		
		public static CubismPose CombinePoses(CubismPose[] poses,float[] levels)
		{
			if( (poses.Length != levels.Length) || (poses.Length == 0))
			{
				return null;
			}

			CubismPose newpose = new CubismPose();
			float sumlevel = 0;
			for(int i = 0; i<poses.Length;i++)
			{
				foreach (var set in poses[i].setting.Keys)
				{
					newpose.setting[set] = 0;
				}

				sumlevel += levels[i];
			}

			for (int i = 0; i < poses.Length; i++)
			{
				foreach (var set in poses[i].setting.Keys)
				{
					newpose.setting[set] += poses[i].setting[set] / sumlevel * levels[i];
				}
			}

			return newpose;
		}

		public void SetPosing(CubismModel target, float weight = 1f)
		{
			foreach(KeyValuePair<string,float> e in setting)
			{
				var param = target.Parameters.FindById(e.Key);
				if(param != null)
				{
					param.SetToValue(e.Value, weight);
				}
			}
		}
	}
}