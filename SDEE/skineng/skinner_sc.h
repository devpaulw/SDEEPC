#pragma once
#include "framework.h"

#define SKNSCAPPWND_UIDSUBCLASS 1452758807
#define SKNSCAPPWND_HOOKED TEXT("SKNSCAPPWND_HOOKED")

struct SkinnerStruct {
	UINT skngExtMsg;
	UINT sknWndExtMsg;
	HWND appHWnd;
	HWND deHWnd;
	HWND sknWndHwnd;
	LONG lBaseStyle;
	LONG lBaseExStyle;
};

HWND getDEHWndFromShMem();
BOOL removeOverlap(_In_ struct SkinnerStruct* sknStruct);
void restoreOverlap(_In_ struct SkinnerStruct* sknStruct);
BOOL isCaptionWindow(HWND hWnd);
void dispose_skinner(struct SkinnerStruct* skinnerStruct);
LRESULT APIENTRY AppSubclassProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam, UINT_PTR uIdSubclass, DWORD_PTR dwRefData);
void HookWindow(HWND hWnd);