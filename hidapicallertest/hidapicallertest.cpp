// hidapicallertest.cpp : アプリケーションのエントリ ポイントを定義します。
//

#include "stdafx.h"

#include <targetver.h>
#pragma comment(lib, "hidApiPicker.lib")

int main()
{
	InitHidPicker();
	if (findDevice(true, 1386, false, 772, true, 10, true, 0xff00))
	{
		int s = getReadSize();
		unsigned char *datas = new unsigned char[s];
		while (true)
		{
			Read(datas);
			for (int i = 0; i < s; i++)
			{
				printf("%o ", datas[i]);
			}
			printf("\n");
		}
	}
    return 0;
}

