using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Leap.Unity;

namespace Ganeesyan.Cubism3Compornets
{
	public class LMWristLiner : LMHandControllBase
	{
		private List<wristPositionParameter> wristPositonX;
		private List<wristPositionParameter> wristPositonY;
		private List<wristPositionParameter> wristPositonZ;

		public Vector3 wristPoint;
		private Vector3 outputData;

		public Vector3 ZeroPos;
		public float scale = 1;

		// Use this for initialization
		public override void Refresh()
		{
			base.Refresh();
			wristPositonX = new List<wristPositionParameter>();
			wristPositonY = new List<wristPositionParameter>();
			wristPositonZ = new List<wristPositionParameter>();
			foreach (var param in model.GetComponentsInChildren<wristPositionParameter>())
			{
				if(param.isLeft == isLeft)
				{
					switch(param.type)
					{
						case wristPositionParameter.PositionType.X:
							wristPositonX.Add(param);
							break;
						case wristPositionParameter.PositionType.Y:
							wristPositonY.Add(param);
							break;
						case wristPositionParameter.PositionType.Z:
							wristPositonZ.Add(param);
							break;
					}
				}
			}
		}

		public void castZeroSet()
		{
			Invoke("SetDefault", 3f);
		}

		public void SetDefault()
		{
			var hand = leapmotionInputer.getHanddata(isLeft);
			var handRev = leapmotionInputer.getHanddata(!isLeft);
			if (hand != null)
			{
				ZeroPos = hand.WristPosition.ToVector3();
				if (handRev != null)
				{
					scale = (handRev.WristPosition.ToVector3() - ZeroPos).magnitude;
				}
			}
		}

		// Update is called once per frame
		void LateUpdate()
		{
			var hand = leapmotionInputer.getHanddata(isLeft);
			if(hand != null)
			{
				wristPoint = hand.WristPosition.ToVector3();
				outputData = (wristPoint - ZeroPos) / scale;

				foreach (var x in wristPositonX)
				{
					x.setParam((wristPoint.x - ZeroPos.x)/scale);
				}
				foreach (var y in wristPositonY)
				{
					y.setParam((wristPoint.y - ZeroPos.y) / scale);
				}
				foreach (var z in wristPositonZ)
				{
					z.setParam((wristPoint.z - ZeroPos.z) / scale);
				}
			}
		}
	}
}