#pragma once

#include <Windows.h>
#include "WindowsVersion.h"

namespace SDEE {
	namespace CLI {
		namespace Win32Lib
		{

			public ref class DEWin32Impl
			{
			public:
				DEWin32Impl();

				void HookDesktop(System::Windows::Interop::HwndSource^ source);
				void UnhookDesktop();
				
				static void HideWindowFromAltTab(System::IntPtr hWnd);

			private:
				bool hooked;
				System::Windows::Interop::HwndSource^ m_source;

				System::IntPtr DesktopWndProc(System::IntPtr hWnd, System::Int32 msg, System::IntPtr wParam, System::IntPtr lParam, System::Boolean% handled);

				LRESULT CALLBACK InternalDesktopWndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam, bool% handled);

				static WindowsVersion GetWindowsVersion();
			};
		}
	}
}