// ---
// SDEE Skin engine
// Made by Paul Wacquet, inspired from BBLeanSkin
// ---

#include "skineng.h"

#include <stdio.h> 
#include <stdlib.h> 
#include <string.h> 

HINSTANCE hInstance;

HHOOK hGetMsgHook;
HHOOK hCallWndHook;
//
UINT sknegMsg;

LRESULT CALLBACK CallWndProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	CWPSTRUCT* cwps = (CWPSTRUCT*)lParam;
	UINT msg = cwps->message;

	if (nCode >= 0)
	{
		if (msg == WM_NCCREATE) // TODO Look for faster messages
		{
			PostMessage(cwps->hwnd, sknegMsg, SKNG_LOAD_PARAM, 0);
		}
	}
	return CallNextHookEx(hCallWndHook, nCode, wParam, lParam);
}

LRESULT CALLBACK GetMsgProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	MSG* lMsg = ((MSG*)lParam);
	UINT message = lMsg->message;

	if (nCode >= 0
		&& message == sknegMsg
		&& lMsg->wParam == SKNG_LOAD_PARAM)
	{
		HookWindow(lMsg->hwnd);
		/*if (message == WM_MBUTTONUP)
			MessageBox(NULL, L"HACKED", L"dll", MB_ICONERROR);*/
	}
	return CallNextHookEx(hGetMsgHook, nCode, wParam, lParam);
}

extern __declspec(dllexport)
BOOL SetHooks(BOOL set) {
	if (set) {
		hGetMsgHook = SetWindowsHookEx(WH_GETMESSAGE, GetMsgProc, hInstance, 0 /*Every thread*/);
		hCallWndHook = SetWindowsHookEx(WH_CALLWNDPROC, CallWndProc, hInstance, 0);

		return hGetMsgHook != NULL;
	}
	else if (!set) {

		BOOL r = UnhookWindowsHookEx(hGetMsgHook);
		BOOL r2 = UnhookWindowsHookEx(hCallWndHook);

		return r && r2;
	}

	return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
	)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		hInstance = hModule;
		sknegMsg = RegisterWindowMessage(SKNG_MSG);
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}