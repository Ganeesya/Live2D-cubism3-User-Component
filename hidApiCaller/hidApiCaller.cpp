// hidApiCaller.cpp : DLL アプリケーション用にエクスポートされる関数を定義します。
//

#include "stdafx.h"
extern "C" {
#include "hidsdi.h"
#include "setupapi.h"
}
#pragma comment(lib, "hid.lib")
#pragma comment(lib, "setupapi.lib")

#include <thread>


class HidController
{
private:
	HANDLE fHandle;
	HIDD_ATTRIBUTES deviceAttr;
	HIDP_CAPS deviceCaps;

public:
	BYTE*	buffers = NULL;

	HidController()
	{
	}

	~HidController()
	{
		Close();
	}

	bool findDevice(int vid, int pid, int usa, int usape)
	{
		GUID hidGuid;
		HidD_GetHidGuid(&hidGuid);
		HDEVINFO devinf;
		devinf = SetupDiGetClassDevs(&hidGuid, NULL, 0, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
		SP_DEVICE_INTERFACE_DATA spid;
		spid.cbSize = sizeof(spid);
		for (int index = 0; ; index++)
		{
			if (!SetupDiEnumDeviceInterfaces(devinf, NULL, &hidGuid, index, &spid))
			{
				//終了であればループ抜け
				break;
			}
			//
			//      デバイスインターフェイスの詳細情報の取得
			//
			unsigned long size;
			SetupDiGetDeviceInterfaceDetail(devinf, &spid, NULL, 0, &size, 0);
			PSP_INTERFACE_DEVICE_DETAIL_DATA dev_det = PSP_INTERFACE_DEVICE_DETAIL_DATA(new char[size]);
			dev_det->cbSize = sizeof(SP_INTERFACE_DEVICE_DETAIL_DATA);
			//
			SetupDiGetDeviceInterfaceDetail(devinf, &spid, dev_det, size, &size, 0);

			//
			//      ファイルハンドルの取得
			//
			HANDLE handle = CreateFile(dev_det->DevicePath, GENERIC_READ | GENERIC_WRITE,
				FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
			if (handle == INVALID_HANDLE_VALUE) { continue; }
			//
			//      VenderIDとProductIDの取得
			//
			HIDD_ATTRIBUTES attr;
			HidD_GetAttributes(handle, &attr);
			/*cout << "VenderID: " << attr.VendorID << endl;
			cout << "ProductID: " << attr.ProductID << endl;
			cout << endl;*/

			HIDP_CAPS caps;
			PHIDP_PREPARSED_DATA preData;
			HidD_GetPreparsedData(handle, &preData);
			HidP_GetCaps(preData, &caps);
			if ((vid == attr.VendorID) &&
				(pid == attr.ProductID) &&
				(usa == caps.Usage) &&
				(usape == caps.UsagePage))
			{
				fHandle = handle;
				deviceAttr = attr;
				deviceCaps = caps;
				StartReadingThread();
				return true;
			}

			//
			CloseHandle(handle);
		}
		return false;
	}

	void ReadingThread()
	{
		while (fHandle != NULL)
		{
			Reading();
		}
	}

	void StartReadingThread()
	{
		if (fHandle != NULL)
		{
			buffers = new BYTE[getReadSize()];
			std::thread rt(&HidController::ReadingThread,this);
			//rt.join();
			rt.detach();
		}
	}

	void Close()
	{
		if(fHandle)	
			CloseHandle(fHandle);
		fHandle = NULL;
	}

	int getReadSize()
	{
		return deviceCaps.InputReportByteLength;
	}
	
	bool Reading()
	{
		if ((fHandle == NULL) || (buffers == NULL))	return false;		
		return ReadFile(fHandle, buffers, deviceCaps.InputReportByteLength, NULL,NULL);
	}
};

HidController *hc = NULL;

HIDAPICALLER_API void InitHidPicker()
{
	hc = new HidController();
}

HIDAPICALLER_API bool findDevice(int vid, int pid, int usa, int usape)
{
	hc->Close();
	return hc->findDevice(vid, pid, usa, usape);
}

HIDAPICALLER_API int getReadSize()
{
	return hc->getReadSize();
}

HIDAPICALLER_API void CloseDHandle()
{
	hc->Close();
	delete hc;
}

HIDAPICALLER_API void ReadData(unsigned char **dataPtr,int * size)
{
	*size = hc->getReadSize();
	*dataPtr = (unsigned char *)malloc(sizeof(char)*(*size));
	memcpy(*dataPtr, hc->buffers, sizeof(char)*(*size));
}

//void a()
//{
//
//	GUID hidGuid;
//	HidD_GetHidGuid(&hidGuid);
//	HDEVINFO devinf;
//	devinf = SetupDiGetClassDevs(&hidGuid, NULL, 0, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
//	SP_DEVICE_INTERFACE_DATA spid;
//	spid.cbSize = sizeof(spid);
//	for (int index = 0; ; index++)
//	{
//		if (!SetupDiEnumDeviceInterfaces(devinf, NULL, &hidGuid, index, &spid))
//		{
//			break;
//		}
//		//
//		//      デバイスインターフェイスの詳細情報の取得
//		//
//		unsigned long size;
//		SetupDiGetDeviceInterfaceDetail(devinf, &spid, NULL, 0, &size, 0);
//		PSP_INTERFACE_DEVICE_DETAIL_DATA dev_det = PSP_INTERFACE_DEVICE_DETAIL_DATA(new char[size]);
//		dev_det->cbSize = sizeof(SP_INTERFACE_DEVICE_DETAIL_DATA);
//		//
//		SetupDiGetDeviceInterfaceDetail(devinf, &spid, dev_det, size, &size, 0);
//		cout << dev_det->DevicePath << endl;
//		//
//		//      ファイルハンドルの取得
//		//
//		HANDLE handle = CreateFile(dev_det->DevicePath, GENERIC_READ | GENERIC_WRITE,
//			FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
//		if (handle == INVALID_HANDLE_VALUE) { cout << "INVALID_HADLE" << endl; continue; }
//		//
//		//      VenderIDとProductIDの取得
//		//
//		HIDD_ATTRIBUTES attr;
//		HidD_GetAttributes(handle, &attr);
//		cout << "VenderID: " << attr.VendorID << endl;
//		cout << "ProductID: " << attr.ProductID << endl;
//		cout << endl;
//		//
//		CloseHandle(handle);
//	}
//}