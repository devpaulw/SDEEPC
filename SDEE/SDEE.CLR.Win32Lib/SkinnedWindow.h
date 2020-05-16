#pragma once
#include <Windows.h>

namespace SDEE {
	namespace CLI {
		namespace Win32Lib { // TODO Rename these namespaces SDEE.Framework.CLINativeImpl
			public ref class SkinnedWindow
			{
			public:
				SkinnedWindow();

				property System::Text::StringBuilder^ Title;
				property System::Windows::Controls::Control^ AssociatedControl;
				property System::Windows::Thickness Borders;

				void Close();
				void Maximize();
				void Minimize();

#ifdef _DEBUG
				void ShowInfos();
#endif

			internal:

				HWND Run(HWND baseHwnd);
				void Stop();

			private:
				bool isRunning;

				HWND m_hWnd;
				HWND m_baseHwnd;
				UINT skngExtMsg = NULL;

				void OnMouseLeftButtonDown(System::Object^ sender, System::Windows::Input::MouseButtonEventArgs^ e); // Especially to move the window

				System::IntPtr WndProc(System::IntPtr hWnd, System::Int32 msg, System::IntPtr wParam, System::IntPtr lParam, System::Boolean% handled); // TODO When destroy, automatically send unload to my friend! Please get WM_DESTROY!

				LRESULT InternalWndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam, bool% handled);
			};
		}
	}
}
