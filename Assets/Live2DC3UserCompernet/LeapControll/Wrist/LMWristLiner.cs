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

		// Use this for initialization
		new void Start()
		{
			base.Start();
			wristPositonX = new List<wristPositionParameter>();
			wristPositonY = new List<wristPositionParameter>();
			wristPositonZ = new List<wristPositionParameter>();
			foreach (var param in model.transform.GetComponentsInChildren<wristPositionParameter>())
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

		// Update is called once per frame
		void LateUpdate()
		{
			var hand = leapmotionInputer.getHanddata(isLeft);
			if(hand != null)
			{
				wristPoint = hand.WristPosition.ToVector3();

				foreach (var x in wristPositonX)
				{
					x.setParam(wristPoint.x);
				}
				foreach (var y in wristPositonY)
				{
					y.setParam(wristPoint.y);
				}
				foreach (var z in wristPositonZ)
				{
					z.setParam(wristPoint.z);
				}
			}
		}
	}
}