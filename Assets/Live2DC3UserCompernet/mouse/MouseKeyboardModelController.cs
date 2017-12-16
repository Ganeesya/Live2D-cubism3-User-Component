using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Core;

namespace Ganeesyan.Cubism3Compornets
{
	public class MouseKeyboardModelController : MonoBehaviour
	{
		public MouseKeyboardInputer inputer;
		public float weight = 1f;
		public Animator animator;

		private List<CubismParameter> mouseXs;
		private List<CubismParameter> mouseYs;

		// Use this for initialization
		void Start()
		{
			mouseXs = new List<CubismParameter>();
			foreach(var e in gameObject.GetComponentsInChildren<MouseX>())
			{
				mouseXs.Add(e.GetComponent<CubismParameter>());
			}

			mouseYs = new List<CubismParameter>();
			foreach (var e in gameObject.GetComponentsInChildren<MouseY>())
			{
				mouseYs.Add(e.GetComponent<CubismParameter>());
			}
		}

		// Update is called once per frame
		void LateUpdate()
		{
			if((animator != null )& (inputer != null))
			{
				for(int i = 0; i < inputer.keys.Length;i++)
				{
					animator.SetBool("Key" + i.ToString(), inputer.keys[i]);
				}
				animator.SetBool("LeftMouse", inputer.lbMouse);
				animator.SetBool("RightMouse", inputer.rbMouse);
			}

			if(inputer != null)
			{
				mouseXs.ToArray().SetToPersent(inputer.calcedPoint.x, weight);
				mouseYs.ToArray().SetToPersent(1f -inputer.calcedPoint.y, weight);
			}
		}
	}
}
