using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

namespace Ganeesyan.Cubism3Compornets
{
	public class LMPalmPoseflag : LMHandControllBase
	{
		[PathAttribute]
		public string lancePath;
		private CubismPose lance;

		[PathAttribute]
		public string wristPath;
		private CubismPose wrist;

		[PathAttribute]
		public string chopPath;
		private CubismPose chop;

		[PathAttribute]
		public string thumbPath;
		private CubismPose thumb;

		[PathAttribute]
		public string palmPath;
		private CubismPose palm;

		[PathAttribute]
		public string backpalmPath;
		private CubismPose backpalm;

		public float hitLevel = 0.8f;
		public float weight = 1f;

		[SerializeField]
		private Vector3 handVect;

		// Use this for initialization
		protected new void Start()
		{
			base.Start();
			lance = CubismPose.LoadCubismPoseFromAssetPath(lancePath);
			wrist = CubismPose.LoadCubismPoseFromAssetPath(wristPath);
			chop = CubismPose.LoadCubismPoseFromAssetPath(chopPath);
			thumb = CubismPose.LoadCubismPoseFromAssetPath(thumbPath);
			palm = CubismPose.LoadCubismPoseFromAssetPath(palmPath);
			backpalm = CubismPose.LoadCubismPoseFromAssetPath(backpalmPath);
			Debug.Log("LMPalmPoseflag : Start()");
		}

		// Update is called once per frame
		void LateUpdate()
		{
			var handdata = leapmotionInputer.getHanddata(isLeft);
			if((handdata != null) && (model != null))
			{
				Vector3 chopVecter = Vector3.Cross(handdata.Direction.ToVector3(), handdata.PalmNormal.ToVector3());

				var lanceDot = Vector3.Dot(handdata.Direction.ToVector3(), Vector3.back);
				var chopDot = - Vector3.Dot(chopVecter, Vector3.back);
				var palmDot = Vector3.Dot(handdata.PalmNormal.ToVector3(), Vector3.back);

				handVect.x = lanceDot;
				handVect.y = chopDot;
				handVect.z = palmDot;

				if (lanceDot > hitLevel)
				{
					CastPose(lance);
				}
				else if (lanceDot < -hitLevel)
				{
					CastPose(wrist);
				}
				else if (chopDot > hitLevel)
				{
					CastPose(chop);
				}
				else if (chopDot < -hitLevel)
				{
					CastPose(thumb);
				}
				else if (palmDot > hitLevel)
				{
					CastPose(palm);
				}
				else if (palmDot < -hitLevel)
				{
					CastPose(backpalm);
				}
			}
		}

		private void CastPose(CubismPose pose)
		{
			if((pose != null) && (model != null))
			{
				pose.SetPosing(model, weight);
			}
		}
	}
}