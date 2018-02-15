using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

namespace Ganeesyan.Cubism3Compornets
{
	public class LMPalmLiner : LMHandControllBase
	{
		public Quaternion handQ;
		public Vector3 handEl;
		public float handRoll;
		// Use this for initialization
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
				handQ = hand.Rotation.ToQuaternion();
				handEl = handQ.eulerAngles;
				handQ.ToAngleAxis(out handRoll,out handEl);
			}
		}
	}
}