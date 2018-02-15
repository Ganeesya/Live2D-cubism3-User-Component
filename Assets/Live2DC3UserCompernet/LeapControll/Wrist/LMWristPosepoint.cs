using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

namespace Ganeesyan.Cubism3Compornets
{
	public class LMWristPosepoint : LMHandControllBase
	{
		[SerializeField]
		public List<PosePoint> palmPositionPosePoints;

		public Vector3 wristPoint;

		public bool settingMode = false;

		public int settingTarget;
		
		public override void Refresh()
		{
			base.Refresh();
		}

		// Update is called once per frame
		void LateUpdate()
		{
			var hand = leapmotionInputer.getHanddata(isLeft);
			if (hand != null)
			{
				wristPoint = hand.WristPosition.ToVector3();
				if (settingMode)
				{
					palmPositionPosePoints[settingTarget].Pose.SetPosing(model);
				}
				else
				{
					Nicoiti(hand.WristPosition.ToVector3());
				}
			}
		}

		public void RollStartMode()
		{
			if (settingMode && (palmPositionPosePoints.Count > 0))
			{
				palmPositionPosePoints[settingTarget].position = wristPoint;
				settingTarget++;
				if (settingTarget >= palmPositionPosePoints.Count)
				{
					settingTarget = 0;
				}
			}
			settingMode = !settingMode;
		}

		private void Nicoiti(Vector3 point)
		{
			if (palmPositionPosePoints.Count == 0) return;
			PosePoint ans = PosePoint.RecallNearst(palmPositionPosePoints, 0, palmPositionPosePoints.Count - 1, point);
			ans.Pose.SetPosing(model);
		}
	}
}