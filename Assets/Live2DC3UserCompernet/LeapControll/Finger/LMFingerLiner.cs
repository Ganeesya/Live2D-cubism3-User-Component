using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

namespace Ganeesyan.Cubism3Compornets
{
	public class LMFingerLiner : LMHandControllBase
	{
		[SerializeField]
		private List<float> fingerLevels;
		[SerializeField]
		private List<List<FingerExtendParameter>> fingerParams;

		// Use this for initialization
		protected new void Start()
		{
			base.Start();

			fingerLevels = new List<float>();

			fingerParams = new List<List<FingerExtendParameter>>();
			for (int i = 0; i <= (int)Finger.FingerType.TYPE_PINKY; i++)
			{
				fingerLevels.Add(0f);
				fingerParams.Add(new List<FingerExtendParameter>());
				foreach (var e in model.GetComponentsInChildren<FingerExtendParameter>())
				{
					if ((e.type == (Finger.FingerType)i) && e.isLeft == isLeft)
					{
						fingerParams[i].Add(e);
					}
				}
			}
		}

		// Update is called once per frame
		void LateUpdate()
		{
			var hand = leapmotionInputer.getHanddata(isLeft);
			if (hand != null)
			{
				for (int i = 0; i < (int)Finger.FingerType.TYPE_PINKY; i++)
				{
					Finger finger = hand.Fingers.Find(x => x.Type == (Finger.FingerType)i);
					if (finger != null)
					{
						fingerLevels[i] = getFingerGrab(finger);
						fingerParams[i].ForEach(x => x.setGrab(fingerLevels[i]));
					}
				}
			}
		}

		private static float getFingerGrab(Finger finger)
		{
			float dot = 1f;
			Vector3 beforeDirection = finger.bones[1].Direction.ToVector3();
			for (int i = 2; i < finger.bones.Length; i++)
			{
				dot *= Mathf.Abs(Vector3.Dot(beforeDirection, finger.bones[i].Direction.ToVector3()));
				beforeDirection = finger.bones[i].Direction.ToVector3();
			}
			return dot;
		}
	}
}