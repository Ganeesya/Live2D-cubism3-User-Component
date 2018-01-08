using UnityEngine;
using System.Collections;
using Leap;
using Live2D.Cubism.Core;

namespace Ganeesyan.Cubism3Compornets
{
	public class FingerExtendParameter : MonoBehaviour
	{
		public Finger.FingerType type;
		public bool isLeft;
		public bool isReverse;

		private CubismParameter parameter;

		// Use this for initialization
		void Start()
		{
			parameter = gameObject.GetComponent<CubismParameter>();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void setGrab(float grab)
		{
			parameter.SetToPersent(isReverse ? (1f - grab) : grab);
		}
	}
}