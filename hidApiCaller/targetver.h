#pragma once

// SDKDDKVer.h をインクルードすると、利用できる最も上位の Windows プラットフォームが定義されます。

// 以前の Windows プラットフォーム用にアプリケーションをビルドする場合は、WinSDKVer.h をインクルードし、
// SDKDDKVer.h をインクルードする前に、サポート対象とするプラットフォームを示すように _WIN32_WINNT マクロを設定します。

#include <SDKDDKVer.h>

#ifdef HIDAPICALLER_EXPORTS
#define HIDAPICALLER_API __declspec(dllexport)
#else
#define HIDAPICALLER_API __declspec(dllimport)
#endif


extern "C" HIDAPICALLER_API void InitHidPicker();

extern "C" HIDAPICALLER_API int getReadSize();

extern "C" HIDAPICALLER_API bool findDevice(int vid, int pid, int usa, int usape);

extern "C" HIDAPICALLER_API void CloseDHandle();

extern "C" HIDAPICALLER_API void ReadData(unsigned char **dataPtr, int * size);

