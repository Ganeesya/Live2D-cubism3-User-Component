// hidapicallertest.cpp : アプリケーションのエントリ ポイントを定義します。
//

#include "stdafx.h"

#include <targetver.h>
#pragma comment(lib, "hidApiPicker.lib")
#include <stdlib.h>

int main()
{
	InitHidPicker();
	if (findDevice( 1386,  772,  10,  0xff00))
	{
		int s;
		unsigned char *datas;
		while (true)
		{
			ReadData( &datas , &s );
			for (int i = 0; i < s; i++)
			{
				printf("%o ", datas[i]);
			}
			printf("\n");
			free(datas);
		}
	}
    return 0;
}

