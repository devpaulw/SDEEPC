#include "SkinnedWindow.h"
#include "skineng_ext.h"

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

			}

			void SkinnedWindow::Close()
			{
				SendMessage(m_hWnd, WM_CLOSE, 0, 0);
			}

			void SkinnedWindow::Maximize()
			{
				ShowWindow(m_hWnd, SW_MAXIMIZE);
			}

			void SkinnedWindow::Minimize()
			{
				ShowWindow(m_hWnd, SW_MINIMIZE);
			}

#ifdef _DEBUG	
			void SkinnedWindow::ShowInfos()
			{
				LONG style = GetWindowLong(m_baseHwnd, GWL_STYLE);
				Windows::MessageBox::Show(style.ToString("X8"));

				//const int wndTextLength = GetWindowTextLength(m_baseHwnd);
				//TCHAR wtBuf[1024];
				//GetWindowText(m_baseHwnd, wtBuf, wndTextLength + 1);
				//Windows::MessageBox::Show(gcnew String(wtBuf));
			}
#endif //_ DEBUG

			HWND SkinnedWindow::Run(HWND baseHwnd)
			{
#pragma region Skin Control config

				//Getting caption title
				if (Title != nullptr) {
					const int wndTextLength = GetWindowTextLength(baseHwnd);
					TCHAR wtBuf[1024];
					GetWindowText(baseHwnd, wtBuf, wndTextLength + 1);
					Title->Clear();
					Title->Append(gcnew String(wtBuf));
				}
#pragma endregion

#pragma region Windows Config
				Windows::Window^ window = gcnew Windows::Window();

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
				skngExtMsg = RegisterWindowMessage(SKNGEXT_STRMSG);
				if (!skngExtMsg)
					throw gcnew Win32Exception(GetLastError(), L"Could not register the Extern Skin Engine Window Message.");

				// Getting the window handle
				auto wih = gcnew Windows::Interop::WindowInteropHelper(window);
				IntPtr hWndIP = wih->EnsureHandle();
				HWND hwnd = (HWND)hWndIP.ToPointer();
				m_baseHwnd = baseHwnd;
				m_hWnd = hwnd;

				// Win32 config
				SetWindowLongPtr(m_baseHwnd, GWLP_HWNDPARENT, (LONG_PTR)m_hWnd);

				// Hook
				Windows::Interop::HwndSource^ hwndSrc = Windows::Interop::HwndSource::FromHwnd(hWndIP);
				hwndSrc->AddHook(gcnew Windows::Interop::HwndSourceHook(this, &SkinnedWindow::WndProc));
#pragma endregion

				window->Show();
				isRunning = true;

				return hwnd;
			}

			void SkinnedWindow::Stop()
			{
				if (!isRunning) // Already stopped or not running yet
					return;

				isRunning = false;

				SendMessage(m_baseHwnd, skngExtMsg,
					MAKEWPARAM(SKNGEXT_SKNWND_TYPEMSG, SKNGEXT_SKNWND_MSG_UNLOAD),
					0);
			}

			void SkinnedWindow::OnMouseLeftButtonDown(System::Object^ sender, System::Windows::Input::MouseButtonEventArgs^ e)
			{
				Windows::Window^ window = (Windows::Window^)sender;
				window->DragMove();
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
						//SetActiveWindow(m_baseHwnd);
						break;
					}
					}
					break;
				case WM_MOUSEACTIVATE: {
					handled = true;
					//SetForegroundWindow(m_hWnd); // HTBD Test whether we never need AllowSetForegroundWindow
					SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
					return MA_NOACTIVATE;
				}


				case WM_WINDOWPOSCHANGED: {
					WINDOWPOS* wp = (WINDOWPOS*)lParam;

					WINDOWPOS wpCopy = *wp;

					wpCopy.x += (int)Borders.Left; // TODO Function for this
					wpCopy.y += (int)Borders.Top;
					wpCopy.cx -= (int)Borders.Left + (int)Borders.Right;
					wpCopy.cy -= (int)Borders.Bottom + (int)Borders.Top;
					
					SetWindowPos(m_baseHwnd, m_hWnd, wpCopy.x, wpCopy.y, wpCopy.cx, wpCopy.cy, SWP_NOZORDER); // TODO Make it faster
					
					break;
				}
				case WM_DESTROY: {
					SendMessage(m_baseHwnd, WM_CLOSE, 0, 0);// HTBD RATHER RESTORE than Close when good DE shutdown! // TODO pass argument like it's from the SKNWND so that there is no loop
					break;
				}
				default:
					break;
				}
				return 0;
			}
		}
	}
}