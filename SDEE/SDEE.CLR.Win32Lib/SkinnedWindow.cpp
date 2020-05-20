#include "SkinnedWindow.h"

using namespace System; // HTBD Remove unessessary usings
using namespace System::Runtime::InteropServices;
using namespace System::ComponentModel;
using namespace System::Collections::Generic;
using namespace System::Windows::Markup;
using namespace System::Windows::Input;
using namespace System::Threading;

namespace SDEE {
	namespace CLI {
		namespace Win32Lib { // TODO Rename these namespaces SDEE.Framework.CLINativeImpl

			SkinnedWindow::SkinnedWindow()
			{
				skngExtMsg = RegisterWindowMessage(SKNGEXT_STRMSG);
				if (!skngExtMsg)
					throw gcnew Win32Exception(GetLastError(), L"Could not register the Extern Skin Engine Window Message.");
			}

			SkinnedWindow::~SkinnedWindow()
			{
				Stop(); 
				m_hWnd = NULL;
				window = nullptr;
				m_baseHwnd = NULL;
				logicalParent = NULL;
				m_title = nullptr;
				m_canClose = false;
				m_canMinimize = false;
				m_canMaximize = false;
			}

			bool SkinnedWindow::IsMaximized::get()
			{
				WINDOWPLACEMENT wpl;
				GetWindowPlacement(m_hWnd, &wpl);
				return wpl.showCmd == SW_MAXIMIZE;
			}

			void SkinnedWindow::Close()
			{
				if (!CanClose)
					throw gcnew Exception("The window can't be closed.");

				SendMessage(m_hWnd, WM_CLOSE, SKNGEXT_SKNWND_TYPEMSG, 0);
			}

			void SkinnedWindow::Maximize()
			{
				if (!CanMaximize)
					throw gcnew Exception("The window can't be maximized.");

				ShowWindow(m_hWnd, SW_MAXIMIZE);
			}

			void SkinnedWindow::Minimize()
			{
				if (!CanMinimize)
					throw gcnew Exception("The window can't be minimized.");

				ShowWindow(m_hWnd, SW_MINIMIZE);
			}

			void SkinnedWindow::Restore()
			{
				if (!CanMaximize)
					throw gcnew Exception("The window can't be maximized.");

				ShowWindow(m_hWnd, SW_RESTORE);
			}

#ifdef _DEBUG	
			void SkinnedWindow::ShowInfos()
			{
				Stop();
				//LONG style = GetWindowLong(m_baseHwnd, GWL_STYLE);
				//Windows::MessageBox::Show(style.ToString("X8"));

				//const int wndTextLength = GetWindowTextLength(m_baseHwnd);
				//TCHAR wtBuf[1024];
				//GetWindowText(m_baseHwnd, wtBuf, wndTextLength + 1);
				//Windows::MessageBox::Show(gcnew String(wtBuf));
			}
#endif //_ DEBUG

			HWND SkinnedWindow::Run(HWND baseHwnd)
			{
#pragma region Config of windows property

				//Getting caption title
				const int wndTextLength = GetWindowTextLength(baseHwnd);
				TCHAR wtBuf[1024];
				GetWindowText(baseHwnd, wtBuf, wndTextLength + 1);
				String^ title = gcnew String(wtBuf);

				Title = title;

				LONG lStyle = GetWindowLong(baseHwnd, GWL_STYLE);
				CanMinimize = (lStyle & WS_MINIMIZEBOX) == WS_MINIMIZEBOX;//newSknWndStruct->hasMinimizeBox;
				CanMaximize = (lStyle & WS_MAXIMIZEBOX) == WS_MAXIMIZEBOX;//newSknWndStruct->hasMaximizeBox;
				CanClose = true;//newSknWndStruct->hasCloseBox;;

				Initialized(this, EventArgs::Empty);
#pragma endregion

#pragma region Windows Config
				window = gcnew Windows::Window();

				// Position and size
				RECT baseWndRect;
				if (!GetWindowRect(baseHwnd, &baseWndRect)) {
					throw gcnew Win32Exception(GetLastError(), L"Could not get window placement.");
				}

				LONG left = baseWndRect.left - (long)Borders.Left, // TODO Function for this
					top = baseWndRect.top - (long)Borders.Top,
					width = baseWndRect.right - baseWndRect.left + (long)Borders.Left + (long)Borders.Right,
					height = baseWndRect.bottom - baseWndRect.top + (long)Borders.Bottom + (long)Borders.Top;
				window->Left = left;
				window->Top = top;
				window->Width = width;
				window->Height = height;

				// TODO Fix just below
				// Min Max Pos [Still bugged yet because GETMINMAXINFO not work every time!]
				// And other bug, the min and max are not correctly set
				MINMAXINFO mmi;
				ZeroMemory(&mmi, sizeof mmi);
				SendMessage(baseHwnd, WM_GETMINMAXINFO, 0, (LONG_PTR)&mmi);
				if (mmi.ptMinTrackSize.x != 0) window->MinWidth = (double)mmi.ptMinTrackSize.x;
				if (mmi.ptMinTrackSize.y != 0) window->MinHeight = (double)mmi.ptMinTrackSize.y;
				if (mmi.ptMaxTrackSize.x != 0) window->MaxWidth = (double)mmi.ptMaxTrackSize.x;
				if (mmi.ptMaxTrackSize.y != 0) window->MaxHeight = (double)mmi.ptMaxTrackSize.y;

				// Getting new skin UIElement
				window->Content = AssociatedControl;

				// TODO Refine min height of the window.
				window->WindowStyle = Windows::WindowStyle::None;
				window->ResizeMode = Windows::ResizeMode::NoResize;
				window->AllowsTransparency = true;

				// Opacity regulation
				window->Opacity = AssociatedControl->Opacity;
				AssociatedControl->Opacity = 1.0;
				//window->MouseLeftButtonDown += gcnew MouseButtonEventHandler(this, &SkinnedWindow::OnMouseLeftButtonDown);
				// TODO If no resize base wnd, same here.
				// HTBD When can't select, don't force [EXCELLENCE]
				//window->ShowInTaskbar = false;
				//window->ShowActivated = false;
				//DEWin32Impl::HideWindowFromAltTab(hWndIP);
#pragma endregion

#pragma region Win32 Config
				// Getting the window handle
				auto wih = gcnew Windows::Interop::WindowInteropHelper(window);
				IntPtr hWndIP = wih->EnsureHandle();
				HWND hwnd = (HWND)hWndIP.ToPointer();
				m_baseHwnd = baseHwnd;
				m_hWnd = hwnd;

				// Win32 config
				logicalParent = (HWND)SetWindowLongPtr(m_baseHwnd, GWLP_HWNDPARENT, (LONG_PTR)m_hWnd);

				// Hook
				Windows::Interop::HwndSource^ hwndSrc = Windows::Interop::HwndSource::FromHwnd(hWndIP);
				hwndSrc->AddHook(gcnew Windows::Interop::HwndSourceHook(this, &SkinnedWindow::WndProc));
#pragma endregion

				window->Show();
				isRunning = true;

				return hwnd;
			}

			void SkinnedWindow::Stop() // TODO Rename it Unload
			{
				if (!isRunning) // Already stopped or not running yet
					return;

				isRunning = false;

				PostMessage(m_hWnd, WM_CLOSE, 0, 0);
				SendMessage(m_baseHwnd, skngExtMsg,
					MAKEWPARAM(SKNGEXT_SKNWND_TYPEMSG, SKNGEXT_SKNWND_MSG_UNLOAD),
					0);
			}

			System::IntPtr SkinnedWindow::WndProc(System::IntPtr hWnd, System::Int32 msg, System::IntPtr wParam, System::IntPtr lParam, System::Boolean% handled)
			{
				return (IntPtr)InternalWndProc((HWND)hWnd.ToPointer(), (UINT)msg, (WPARAM)wParam.ToPointer(), (LPARAM)lParam.ToPointer(), handled);
			}

			LRESULT SkinnedWindow::InternalWndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam, bool% handled)
			{
				switch (msg) {
					//TODO When Show Window, active it. Please make a better activation system!
				case WM_ACTIVATE:
					switch (LOWORD(wParam)) {
					case WA_ACTIVE: {
						SetActiveWindow(m_baseHwnd); // HTBD: No blinking at all, more reliable. SetForegroundWindow is probably dangerous, look at MSDN
						//SetForegroundWindow(m_baseHwnd);
						//LockWindowUpdate(NULL);
						//ShowWindow(m_baseHwnd, SW_SHOWNORMAL);
						//SetActiveWindow(m_baseHwnd);
						//PostMessage(m_baseHwnd, WM_ACTIVATEAPP, 1, 0);
						break;
					}
					}
					break;
				case WM_MOUSEACTIVATE: { // TODO BETTER HANDLE NEVER ENABLE SKNWND
					handled = true;
					//SetForegroundWindow(m_hWnd); // HTBD Test whether we never need AllowSetForegroundWindow
					SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
					return MA_NOACTIVATE;
				}
				case WM_WINDOWPOSCHANGED: {
					// TODO Try it for perfs and fix resize bug if cool //handled = true; // More efficient
					// TODO BUG With CMD Winows 7
					WINDOWPOS* wp = (WINDOWPOS*)lParam;

					WINDOWPOS wpCopy = *wp;

					wpCopy.x += (int)Borders.Left; // TODO Function for this
					wpCopy.y += (int)Borders.Top;
					wpCopy.cx -= (int)Borders.Left + (int)Borders.Right;
					wpCopy.cy -= (int)Borders.Bottom + (int)Borders.Top;

					SetWindowPos(m_baseHwnd, m_hWnd, wpCopy.x, wpCopy.y, wpCopy.cx, wpCopy.cy, SWP_NOZORDER); // TODO Make it faster

					break;
				}
				case WM_CLOSE: {
					//CloseWindow(m_baseHwnd);
					// TODO Don't close when closed from the app wpf.

					if (wParam == SKNGEXT_SKNWND_TYPEMSG) {
						// HTBD Here SendMessageTimeout to check if the skin window is stayed without the base
						PostMessage(m_baseHwnd, WM_CLOSE, 0, 0);// HTBD RATHER RESTORE than Close when good DE shutdown! // TODO pass argument like it's from the SKNWND so that there is no loop
						handled = true;
						return 0;
					}

					//if (wParam != SKNGEXT_APPWND_TYPEMSG) {
					//	//PostMessage(m_baseHwnd, WM_CLOSE, 0, 0); // UNDONE
					//}


					break;
				}
				case WM_NCDESTROY: // Last message
				{
					isRunning = false;

					if (logicalParent != NULL)
						EnableWindow(logicalParent, TRUE);
					else {
						SetWindowLongPtr(m_baseHwnd, GWLP_HWNDPARENT, NULL);
					}

					Closed(this, EventArgs::Empty);
				}
				break;
				default:
					break;
				}
				return 0;
			}
		}
	}
}