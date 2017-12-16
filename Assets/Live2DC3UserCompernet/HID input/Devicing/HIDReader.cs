using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;
using System;

namespace Ganeesyan.Cubism3Compornets
{
	public class HIDReader : MonoBehaviour
	{
		protected int venderID;
		protected int productID;
		protected int usage;
		protected int usagePage;

		protected byte[] buffer;

		[Multiline(20)]
		public string DegitalRawDatas;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		[DllImport("hidApiPicker")] private static extern void InitHidPicker();

		[DllImport("hidApiPicker")] private static extern int getReadSize();

		[DllImport("hidApiPicker")] private static extern bool findDevice(int vid, int pid, int usa, int usape);

		[DllImport("hidApiPicker")] private static extern void CloseDHandle();

		[DllImport("hidApiPicker")] private static extern void ReadData(out IntPtr dataPtr, out int size);
#endif

		// Use this for initialization
		protected void Start()
		{

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			InitHidPicker();
			if (findDevice(venderID, productID, usage, usagePage))
			{
				int length = getReadSize();
				buffer = new byte[length];
			}
#endif
		}

		// Update is called once per frame
		protected void Update()
		{

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			IntPtr dataPtr = IntPtr.Zero;
			int size = 0;

			ReadData(out dataPtr, out size);

			byte[] bu2 = new byte[size];

			Marshal.Copy(dataPtr, buffer, 0, size);

			DegitalRawDatas = "";
			int line = 0;
			foreach (var e in buffer)
			{
				DegitalRawDatas += "[" + line.ToString().PadLeft(3, '0') + "] " + Convert.ToString(e, 2).PadLeft(8, '0') + "\n";
				line++;
			}
#endif
		}

		private void OnDestroy()
		{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			CloseDHandle();
#endif
		}
	}
}
