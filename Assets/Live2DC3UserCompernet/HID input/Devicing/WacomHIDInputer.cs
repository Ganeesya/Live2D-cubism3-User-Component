using Live2D.Cubism.Core;
using UnityEngine;

namespace Ganeesyan.Cubism3Compornets
{
	public class WacomHIDInputer : HIDReader
	{
		public bool isOnTablet;

		public bool SideButtenHead;
		public bool SideButtenTail;

		public float horizontal;
		public float vertical;
		public float pressure;
		public float TiltToHorizon;
		public float TiltToVertical;
		public float PenPointHeigth;

		// Use this for initialization
		new void Start()
		{
			venderID = 1386;
			productID = 772;
			usage = 10;
			usagePage = 65280;
			base.Start();
		}

		// Update is called once per frame
		new void Update()
		{
			base.Update();

			if (buffer.Length < 11) return;

			//293.8 x 165.2mm
			isOnTablet = ((buffer[2] & 32) > 0);
			horizontal = ((buffer[3] << 8) + buffer[4]) / 29380f;
			vertical = ((buffer[5] << 8) + buffer[6]) / 16520f;
			pressure = (((buffer[7]) << 3) + ((buffer[2] & 1) << 2) + ((buffer[8] & 0xc0) >> 6)) / 2048f;
			TiltToHorizon = (((buffer[8] & 0x3f) << 1) + ((buffer[9] & 0x80) >> 7) - 64) / 40f;
			TiltToVertical = ((buffer[9] & 0x7f) - 64) / 40f;
			PenPointHeigth = (buffer[10] - 95) / 100f;

			SideButtenTail = ((buffer[2] & 4) > 0);
			SideButtenHead = ((buffer[2] & 2) > 0);
		}
	}
}
