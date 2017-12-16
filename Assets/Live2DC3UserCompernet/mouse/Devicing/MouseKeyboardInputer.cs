using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


namespace Ganeesyan.Cubism3Compornets
{
	public class MouseKeyboardInputer : MonoBehaviour {
		#region Native Calls
		public delegate int HookHandler(int code, IntPtr message, IntPtr state);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int hookType, HookHandler hookDelegate, IntPtr module, uint threadId);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hook);

		[DllImport("user32.dll")]
		public static extern int CallNextHookEx(IntPtr hook, int code, IntPtr message, IntPtr state);

		[StructLayout(LayoutKind.Sequential)]
		public class KBDLLHOOKSTRUCT
		{
			public uint vkCode;
			public uint scanCode;
			public KBDLLHOOKSTRUCTFlags flags;
			public uint time;
			public UIntPtr dwExtraInfo;
		}

		[Flags]
		public enum KBDLLHOOKSTRUCTFlags : uint
		{
			LLKHF_EXTENDED = 0x01,
			LLKHF_INJECTED = 0x10,
			LLKHF_ALTDOWN = 0x20,
			LLKHF_UP = 0x80,
		}


		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class MSLLHOOKSTRUCT
		{
			public POINT pt;
			public int mouseData;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}

		enum MessageList : uint
		{
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205,
			WM_KEYDOWN = 0x0100,
			WM_KEYUP = 0x0101,
			WM_MOUSEMOVE = 0x0200
		}
		/*
		/// <summary>
		/// Selects duplex or double-sided printing for printers capable of duplex printing. 
		/// </summary>
		internal enum DMDUP : short
		{
			/// <summary>
			/// Unknown setting.
			/// </summary>
			DMDUP_UNKNOWN = 0,

			/// <summary>
			/// Normal (nonduplex) printing.
			/// </summary>
			DMDUP_SIMPLEX = 1,

			/// <summary>
			/// Long-edge binding, that is, the long edge of the page is vertical.
			/// </summary>
			DMDUP_VERTICAL = 2,

			/// <summary>
			/// Short-edge binding, that is, the long edge of the page is horizontal.
			/// </summary>
			DMDUP_HORIZONTAL = 3,
		}

		struct POINTL
		{
			public Int32 x;
			public Int32 y;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct DEVMODE
		{
			public const int CCHDEVICENAME = 32;
			public const int CCHFORMNAME = 32;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
			public string dmDeviceName;
			public Int16 dmSpecVersion;
			public Int16 dmDriverVersion;
			public Int16 dmSize;
			public Int16 dmDriverExtra;
			public DMDUP dmFields;
		
			Int16 dmOrientation;
			Int16 dmPaperSize;
			Int16 dmPaperLength;
			Int16 dmPaperWidth;
			Int16 dmScale;
			Int16 dmCopies;
			Int16 dmDefaultSource;
			Int16 dmPrintQuality;
		
			public POINTL dmPosition;
			public Int32 dmDisplayOrientation;
			public Int32 dmDisplayFixedOutput;
		
			public short dmColor; // See note below!
			public short dmDuplex; // See note below!
			public short dmYResolution;
			public short dmTTOption;
			public short dmCollate; // See note below!

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
			public string dmFormName;
			public Int16 dmLogPixels;
			public Int32 dmBitsPerPel;
			public Int32 dmPelsWidth;
			public Int32 dmPelsHeight;
			public Int32 dmDisplayFlags;
			public Int32 dmNup;
			public Int32 dmDisplayFrequency;
		}

		[DllImport("user32.dll")]
		static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
		const int ENUM_CURRENT_SETTINGS = -1;
		const int ENUM_REGISTRY_SETTINGS = -2;

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAY_DEVICE
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cb;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DeviceName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceString;
			[MarshalAs(UnmanagedType.U4)]
			public DisplayDeviceStateFlags StateFlags;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceID;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceKey;
		}

		[Flags]
		public enum DisplayDeviceStateFlags
		{
			None = 0,
			/// <summary>The device is part of the desktop.</summary>
			AttachedToDesktop = 0x1,
			MultiDriver = 0x2,
			/// <summary>The device is part of the desktop.</summary>
			PrimaryDevice = 0x4,
			/// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
			MirroringDriver = 0x8,
			/// <summary>The device is VGA compatible.</summary>
			VGACompatible = 0x16,
			/// <summary>The device is removable; it cannot be the primary display.</summary>
			Removable = 0x20,
			/// <summary>The device has more display modes than its output devices support.</summary>
			ModesPruned = 0x8000000,
			Remote = 0x4000000,
			Disconnect = 0x2000000,
		}

		[DllImport("user32.dll")]
		static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);*/

		delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle2 lprcMonitor, IntPtr dwData);

		[DllImport("user32.dll")]
		static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

		[DllImport("user32.dll")]
		static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

		[StructLayout(LayoutKind.Sequential)]
		public class MonitorInfo
		{
			public uint cbSize;
			public Rectangle2 rcMonitor;
			public Rectangle2 rcWork;
			public uint dwFlags;

			public MonitorInfo()
			{
				rcMonitor = new Rectangle2();
				rcWork = new Rectangle2();

				cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(MonitorInfo));
				dwFlags = 0;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Rectangle2
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern uint GetLastError();

		#endregion

		private HookHandler m_keyhookH;
		private HookHandler m_mousehookH;
		private IntPtr keyhook;
		private IntPtr mousehook;

		private List<Rect> displays = new List<Rect>();
		private Rect AllField;

		public Vector2 rawMouse;
		public Vector2 displayOnMouse;
		public Vector2 calcedPoint;

		public int targetDisplay;

		public PointCalcMode pointCalcMode;

		public bool lbMouse = false;
		public bool rbMouse = false;

		public bool[] keys = new bool[255];

		public delegate void KeyEvent(int keynum);

		public event KeyEvent OnKeyUp;
		public event KeyEvent OnKeyDown;

		public delegate void MouseEvent();

		public event MouseEvent OnLeftButtonUp;
		public event MouseEvent OnLeftButtonDown;
		public event MouseEvent OnRightButtonUp;
		public event MouseEvent OnRightButtonDown;

		int keyHook(int nCode, IntPtr wParam, IntPtr lParam)
		{
			KBDLLHOOKSTRUCT kb = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

			if (0 <= nCode)
			{
				switch ((uint)wParam)
				{
					case (uint)MessageList.WM_KEYDOWN:
						keys[(int)kb.vkCode] = true;
						if (OnKeyDown != null) OnKeyDown((int)kb.vkCode);
						break;

					case (uint)MessageList.WM_KEYUP:
						keys[(int)kb.vkCode] = false;
						if (OnKeyUp != null) OnKeyUp((int)kb.vkCode);
						break;
				}
			}

			return CallNextHookEx(keyhook, nCode, wParam, lParam);
		}

		int mouseHook(int nCode, IntPtr wParam, IntPtr lParam)
		{
			MSLLHOOKSTRUCT ms = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

			if (0 <= nCode)
			{
				switch ((uint)wParam)
				{
					case (uint)MessageList.WM_LBUTTONDOWN:
						lbMouse = true;
						rawMouse.x = ms.pt.x;
						rawMouse.y = ms.pt.y;
						if(OnLeftButtonDown != null) OnLeftButtonDown();
						break;

					case (uint)MessageList.WM_LBUTTONUP:
						lbMouse = false;
						rawMouse.x = ms.pt.x;
						rawMouse.y = ms.pt.y;
						if (OnLeftButtonUp != null) OnLeftButtonUp();
						break;

					case (uint)MessageList.WM_RBUTTONDOWN:
						rbMouse = true;
						rawMouse.x = ms.pt.x;
						rawMouse.y = ms.pt.y;
						if (OnRightButtonDown != null) OnRightButtonDown();
						break;

					case (uint)MessageList.WM_RBUTTONUP:
						rbMouse = false;
						rawMouse.x = ms.pt.x;
						rawMouse.y = ms.pt.y;
						if (OnRightButtonUp != null) OnRightButtonUp();
						break;

					case (uint)MessageList.WM_MOUSEMOVE:
						rawMouse.x = ms.pt.x;
						rawMouse.y = ms.pt.y;
						break;
				}
			}

			return CallNextHookEx(mousehook, nCode, wParam, lParam);
		}

		// Use this for initialization
		void Start()
		{
			const int KEYBOARD_LL = 13;
			const int WH_MOUSE_LL = 14;

			m_keyhookH = new HookHandler(keyHook);
			m_mousehookH = new HookHandler(mouseHook);

			//IntPtr hMod = Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]);

			keyhook = SetWindowsHookEx(KEYBOARD_LL, m_keyhookH, IntPtr.Zero, 0);
			if (keyhook == IntPtr.Zero)
			{
				int errorCode = Marshal.GetLastWin32Error();
				throw new Exception("windows error code:" + errorCode.ToString());
			}

			mousehook = SetWindowsHookEx(WH_MOUSE_LL, m_mousehookH, IntPtr.Zero, 0);
			if (mousehook == IntPtr.Zero)
			{
				int errorCode = Marshal.GetLastWin32Error();
				throw new Exception("windows error code:" + errorCode.ToString());
			}

			EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
			delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle2 lprcMonitor, IntPtr dwData)
			{
				displays.Add(new Rect(lprcMonitor.left, lprcMonitor.top, lprcMonitor.right - lprcMonitor.left, lprcMonitor.bottom - lprcMonitor.top));
				return true;
			}, IntPtr.Zero);

			if (displays.Count > 0)
			{
				AllField = displays[0];
			}
			foreach (var e in displays)
			{
				AllField.xMin = Math.Min(e.xMin, AllField.xMin);
				AllField.yMin = Math.Min(e.yMin, AllField.yMin);
				AllField.xMax = Math.Max(e.xMax, AllField.xMax);
				AllField.yMax = Math.Max(e.yMax, AllField.yMax);
			}
		}

		// Update is called once per frame
		void Update()
		{
			Rect targetDispRect;
			switch (pointCalcMode)
			{
				case PointCalcMode.AllField:
					displayOnMouse.x = (rawMouse.x - AllField.xMin) / AllField.width;
					displayOnMouse.y = (rawMouse.y - AllField.yMin) / AllField.height;
					calcedPoint = displayOnMouse;
					break;

				case PointCalcMode.Switching:
					targetDisplay = displays.FindIndex(x => x.Contains(rawMouse));
					if (targetDisplay == -1) break;

					targetDispRect = displays[targetDisplay];
					displayOnMouse.x = (rawMouse.x - targetDispRect.xMin) / targetDispRect.width;
					displayOnMouse.y = (rawMouse.y - targetDispRect.yMin) / targetDispRect.height;
					calcedPoint = displayOnMouse;
					break;

				case PointCalcMode.Targeted:
					targetDispRect = displays[targetDisplay];
					displayOnMouse.x = (rawMouse.x - targetDispRect.xMin) / targetDispRect.width;
					displayOnMouse.y = (rawMouse.y - targetDispRect.yMin) / targetDispRect.height;
					calcedPoint = displayOnMouse;
					break;
			}
		}

		private void OnDisable()
		{
			if (keyhook != IntPtr.Zero)
			{
				UnhookWindowsHookEx(keyhook);
			}
			if (mousehook != IntPtr.Zero)
			{
				UnhookWindowsHookEx(mousehook);
			}
		}

		public enum PointCalcMode
		{
			AllField,
			Targeted,
			Switching
		}
	}
}
