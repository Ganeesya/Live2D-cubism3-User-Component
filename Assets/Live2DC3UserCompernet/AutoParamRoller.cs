using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework.UserData;
using Live2D.Cubism.Core;
using Live2D.Cubism.Rendering;

namespace Ganeesyan.Cubism3Compornets
{
	public class AutoParamRoller : MonoBehaviour
	{
		public CubismUserDataTag From;
		public CubismUserDataTag To;

		public float Offset;

		public bool Reverse;

		private Vector2 ft;
		private Vector2 fromvect;
		private Vector2 tovect;

		private CubismRenderer fromRender;
		private CubismRenderer toRender;

		private CubismParameter parameter;

		private Transform modelTransform;

		// Use this for initialization
		void Start()
		{
			Refresh();
		}

		public void Refresh()
		{
			parameter = this.GetComponent<CubismParameter>();

			if (From != null)
			{
				fromRender = From.GetComponent<CubismRenderer>();
			}

			if (To != null)
			{
				toRender = To.GetComponent<CubismRenderer>();
			}

			var model = this.GetComponentInParent<CubismModel>();
			if (model != null)
			{
				modelTransform = model.transform;
			}
		}

		// Update is called once per frame
		void Update()
		{

		}

		void LateUpdate()
		{
			fromvect = modelTransform.TransformPoint( fromRender.Mesh.bounds.center );
			tovect = modelTransform.TransformPoint( toRender.Mesh.bounds.center );

			ft = tovect - fromvect;
			var angle = Mathf.Atan2(ft.y, ft.x) * Mathf.Rad2Deg;

			parameter.SetToValue(( angle * (Reverse?-1:1)) + Offset);
		}
	}
}