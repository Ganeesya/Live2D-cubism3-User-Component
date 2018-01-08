using UnityEngine;
using System.Collections;
using Live2D.Cubism.Core;

namespace Ganeesyan.Cubism3Compornets
{
	public class wristPositionParameter : MonoBehaviour
	{
		public enum PositionType
		{
			X,
			Y,
			Z
		}

		public PositionType type;
		public bool isLeft;
		public bool isReverse;
		public float min;
		public float max;

		private CubismParameter parameter;

		// Use this for initialization
		void Start()
		{
			parameter = gameObject.GetComponent<CubismParameter>();
		}
		
		public void setParam(float input)
		{
			parameter.SetToPersent(( isReverse ? (1f - input) : input ) * (max - min) + min);
		}
	}
}