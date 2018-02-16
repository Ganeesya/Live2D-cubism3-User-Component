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

        public List<palmRollingParameter> list;

		// Use this for initialization
		public override void Refresh()
		{
			base.Refresh();

            foreach (var e in model.GetComponentsInChildren<palmRollingParameter>())
            {
                if ( e.isLeft == isLeft)
                {
                    list.Add(e);
                }
            }
        }

		// Update is called once per frame
		void LateUpdate()
		{
			var hand = leapmotionInputer.getHanddata(isLeft);
			if (hand != null)
			{
				handQ = hand.Rotation.ToQuaternion();
                list.ForEach(x => x.setQuaternion(handQ));

                handEl = hand.Direction.ToVector3();//handQ.eulerAngles;
                var target = Vector3.forward;
				handQ.ToAngleAxis(out handRoll,out target);
			}
		}
	}
}