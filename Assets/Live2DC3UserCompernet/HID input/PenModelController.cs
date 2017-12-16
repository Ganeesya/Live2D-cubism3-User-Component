using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Core;

namespace Ganeesyan.Cubism3Compornets
{
	public class PenModelController : MonoBehaviour
	{
		public WacomHIDInputer inputer;
		public Animator animator;
		public float weight;

		private List<CubismParameter> penHeights;
		private List<CubismParameter> penPosHorizon;
		private List<CubismParameter> penPosVirtical;
		private List<CubismParameter> penPress;
		private List<CubismParameter> penTiltHorizon;
		private List<CubismParameter> penTiltVirtical;

		// Use this for initialization
		void Start()
		{
			penHeights = new List<CubismParameter>();
			foreach (var e in gameObject.GetComponentsInChildren<PenHeight>())
			{
				penHeights.Add(e.gameObject.GetComponent<CubismParameter>());
			}
			penPosHorizon = new List<CubismParameter>();
			foreach (var e in gameObject.GetComponentsInChildren<PenPointHorizontal>())
			{
				penPosHorizon.Add(e.gameObject.GetComponent<CubismParameter>());
			}
			penPosVirtical = new List<CubismParameter>();
			foreach (var e in gameObject.GetComponentsInChildren<PenPointVirtical>())
			{
				penPosVirtical.Add(e.gameObject.GetComponent<CubismParameter>());
			}
			penPress = new List<CubismParameter>();
			foreach (var e in gameObject.GetComponentsInChildren<PenPressure>())
			{
				penPress.Add(e.gameObject.GetComponent<CubismParameter>());
			}
			penTiltHorizon = new List<CubismParameter>();
			foreach (var e in gameObject.GetComponentsInChildren<PenTiltHorizontal>())
			{
				penTiltHorizon.Add(e.gameObject.GetComponent<CubismParameter>());
			}
			penTiltVirtical = new List<CubismParameter>();
			foreach (var e in gameObject.GetComponentsInChildren<PenTiltVirtical>())
			{
				penTiltVirtical.Add(e.gameObject.GetComponent<CubismParameter>());
			}
		}

		// Update is called once per frame
		void LateUpdate()
		{
			//penHeights.ToArray().SetToPersent(pen)
		}
	}
}