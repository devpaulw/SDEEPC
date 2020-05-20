#pragma once
#include <Windows.h>
#include "skineng_ext.h"


namespace SDEE {
	namespace CLI {
		namespace Win32Lib { // TODO Rename these namespaces SDEE.Framework.CLINativeImpl
			public ref class SkinnedWindow
			{
			public:
				SkinnedWindow();
				~SkinnedWindow();

				property System::Windows::Controls::Control^ AssociatedControl;
				property System::Windows::Thickness Borders;
				property bool IsMaximized {
					bool get();
				}

				event System::EventHandler^ Initialized;
				event System::EventHandler^ Closed;

				void Close();
				void Maximize();
				void Minimize();
				void Restore();

				property System::String^ Title
				{
					System::String^ get() { return m_title; }
				private:
					void set(System::String^ value) { m_title = value; }
				}
				property bool CanClose
				{
					bool get() { return m_canClose; }
				private:
					void set(bool value) { m_canClose = value; }
				}
				property bool CanMinimize
				{
					bool get() { return m_canMinimize; }
				private:
					void set(bool value) { m_canMinimize = value; }
				}
				property bool CanMaximize
				{
					bool get() { return m_canMaximize; }
				private:
					void set(bool value) { m_canMaximize = value; }
				}

#ifdef _DEBUG
				void ShowInfos();
#endif

			internal:
				HWND Run(HWND baseHwnd);
				void Stop();

				property System::IntPtr HWnd {
					System::IntPtr get() { return System::IntPtr(m_hWnd); }
				}

			private:
				bool isRunning;

				initonly UINT skngExtMsg;
				System::Windows::Window^ window;
				HWND m_hWnd;
				HWND m_baseHwnd;
				HWND logicalParent;
				
				System::IntPtr WndProc(System::IntPtr hWnd, System::Int32 msg, System::IntPtr wParam, System::IntPtr lParam, System::Boolean% handled); // TODO When destroy, automatically send unload to my friend! Please get WM_DESTROY!

				LRESULT InternalWndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam, bool% handled);

				System::String^ m_title;
				System::Boolean m_canClose;
				System::Boolean m_canMinimize;
				System::Boolean m_canMaximize;
			};
		}
	}
}
