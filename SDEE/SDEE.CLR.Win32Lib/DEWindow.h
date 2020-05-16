#pragma once

#include <Windows.h>
#include "Exceptions.hpp"

namespace SDEE {
	namespace CLI {
		namespace Win32Lib
		{
			delegate LRESULT WndProcDelegate(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
			
			public ref class DEWindow
			{
			public:
				DEWindow();
				~DEWindow();
				!DEWindow();

				property bool IsOpen {
					bool get() {
						return isOpen;
					}
				}
				void DispatchMessages();
				void Close();

				event System::EventHandler<System::Drawing::Graphics^>^ Paint;

			private:
				const LPCWSTR szClassName = TEXT("sdeecliwin32libwindow");
				HINSTANCE hInstance;
				HWND hWnd;
				bool isOpen;
				WndProcDelegate^ wndProcDelegate;
				LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);

				void register_class();
				void init_instance();
				void peek_msg();

				void Destroy();
			};
		}
	}
}