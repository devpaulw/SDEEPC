#pragma once

#include <Windows.h>

// WPF HwndSourceHook Desktop Window Procedure
//extern "C" __declspec(dllexport) int __cdecl WpfHshDtWndProc(HWND hwnd, int msg, WPARAM wParam, LPARAM lParam, bool* handled);

using namespace System;

namespace SDEE {
	namespace CLR {
		namespace Win32Lib {
			public ref class DesktopEnvironment
			{
			public:
				
				static IntPtr HwndSourceHookWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, bool &handled);
			private:
				static int HwndSourceHookWndProc(HWND hwnd, INT msg, WPARAM wParam, LPARAM lParam);
			};
		}
	}
}