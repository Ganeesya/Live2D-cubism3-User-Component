using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganeesyan.Cubism3Compornets
{
	[System.Serializable]
	public class PosePoint
	{
		[PathAttribute]
		public string motion3Json;
		private CubismPose _pose = null;
		private bool jsonLoaded = false;
		public CubismPose Pose
		{
			get
			{
				if (jsonLoaded)
				{
					return _pose;
				}
				else
				{
					loadPoseFile();
					return _pose;
				}
			}
			set
			{
				_pose = value;
				if (_pose != null)
				{
					jsonLoaded = true;
				}
			}
		}
		public Vector3 position = new Vector3();

		// Use this for initialization
		PosePoint(string jsonpath)
		{
			motion3Json = jsonpath;
			loadPoseFile();
		}

		PosePoint()
		{
			loadPoseFile();
		}

		private void loadPoseFile()
		{
			_pose = CubismPose.LoadCubismPoseFromAssetPath(motion3Json);
			if (_pose != null)
			{
				jsonLoaded = true;
			}
		}

		public static PosePoint MixPoint(PosePoint A, PosePoint B, float weightA)
		{
			PosePoint result = new PosePoint();
			result.position = B.position + (A.position - B.position) * weightA;
			result.Pose = B.Pose + (A.Pose - B.Pose) * weightA;
			return result;
		}

		public static PosePoint MixPointSameLong(PosePoint Key, Vector3 longSample, PosePoint slideSample)
		{
			if ((Key.position - slideSample.position).sqrMagnitude == 0)
			{
				return Key;
			}
			float longWeight = (Key.position - longSample).sqrMagnitude / (Key.position - slideSample.position).sqrMagnitude;
			if (float.IsInfinity(longWeight))
			{
				longWeight = 1f;
			}
			return MixPoint(slideSample, Key, longWeight);
		}

		public static PosePoint MixPointNearst(PosePoint Key, Vector3 target, PosePoint slideSample, bool overFloor)
		{
			if ((Key.position - slideSample.position).magnitude == 0)
			{
				return Key;
			}

			Vector3 keyToTarget = target - Key.position;
			Vector3 keyToSlide = slideSample.position - Key.position;

			float dot = Vector3.Dot(keyToSlide.normalized, keyToTarget.normalized) * keyToTarget.magnitude / keyToSlide.magnitude;
			if (!overFloor)
			{
				dot = Mathf.Min(1f, Mathf.Max(0f, dot));
			}
			return MixPoint(Key, slideSample, (1f - dot));
		}

		public static PosePoint RecallNearst(List<PosePoint> list, int A, int B, Vector3 point)
		{
			if (A == B)
			{
				return list[A];
			}
			else
			{
				PosePoint PoseA = RecallNearst(list, A, Mathf.CeilToInt((A + B) / 2), point);
				PosePoint PoseB = RecallNearst(list, Mathf.CeilToInt((A + B) / 2) + 1, B, point);

				return PosePoint.MixPointNearst(PoseA, point, PoseB, true);
			}
		}
	}
}