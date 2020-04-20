#include "pch.h"

#include "SDEE.CLR.Win32Lib.h"

//extern "C" __declspec(dllexport) int __cdecl GetMof() {
//	return 42;
//}
//
//extern "C" __declspec(dllexport) int __cdecl WpfHshDtWndProc(HWND hwnd, int msg, WPARAM wParam, LPARAM lParam, bool* handled)
//{
//	switch (msg) {
//	case WM_LBUTTONUP:
//		MessageBox(0, 0, 0, 0);
//	}
//
//	return 0;
//}
IntPtr SDEE::CLR::Win32Lib::DesktopEnvironment::HwndSourceHookWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, bool& handled)
{
	return (IntPtr)HwndSourceHookWndProc((HWND)hwnd.ToPointer(), msg, (WPARAM)wParam.ToPointer(), (LPARAM)lParam.ToPointer());
}

int SDEE::CLR::Win32Lib::DesktopEnvironment::HwndSourceHookWndProc(HWND hwnd, INT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg) {
	case WM_LBUTTONUP:
		MessageBox(0, 0, 0, 0);
	}

	return 0;
}
