using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Live2D.Cubism.Core;

namespace Ganeesyan.Cubism3Compornets
{
	public class LMHandControllBase : MonoBehaviour
	{
		protected LeapmotionInputer leapmotionInputer;

		[System.NonSerialized]
		public bool isLeft;

		protected CubismModel model;
		//{
		//	get
		//	{
		//		return GetComponent<CubismModel>();
		//	}
		//}

		// Use this for initialization
		protected void Start()
		{
			model = GetComponent<CubismModel>();
			if (model == null)
			{
				model = GetComponentInParent<CubismModel>();
			}
			leapmotionInputer = GameObject.FindObjectOfType<LeapmotionInputer>();
			Debug.Log("LMHandControllBase : Start()");
		}
	}
}