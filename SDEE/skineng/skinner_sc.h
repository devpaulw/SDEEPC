#pragma once
#include "framework.h"

#define SKNSCAPPWND_UIDSUBCLASS 1452758807
#define SKNSCAPPWND_HOOKED TEXT("SKNSCAPPWND_HOOKED")

struct SkinnerStruct {
	UINT skngExtMsg;
	UINT sknWndExtMsg;
	HWND appHWnd;
	HWND deHWnd;
	HWND sknWndHWnd;
};

HWND getDEHWndFromShMem();
BOOL removeOverlap(HWND hWnd);
BOOL isCaptionWindow(HWND hWnd);
VOID CALLBACK NewWndSendAsyncProc(HWND hwnd, UINT uMsg, ULONG_PTR dwData, LRESULT lResult);
VOID CALLBACK SkinRequestSendAsyncProc(HWND hWnd, UINT uMsg, ULONG_PTR dwData, LRESULT lResult);
void dispose_skinner(struct SkinnerStruct* skinnerStruct);
LRESULT APIENTRY AppSubclassProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam, UINT_PTR uIdSubclass, DWORD_PTR dwRefData);
void HookWindow(HWND hWnd);