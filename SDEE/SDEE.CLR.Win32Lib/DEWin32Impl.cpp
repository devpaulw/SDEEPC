#include "DEWin32Impl.h"
#include "Exceptions.hpp"
#include <versionhelpers.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::ComponentModel;
using namespace System::Collections::Generic;
using namespace System::Windows::Markup;
using namespace System::Threading;

SDEE::CLI::Win32Lib::DEWin32Impl::DEWin32Impl()
{
}

void SDEE::CLI::Win32Lib::DEWin32Impl::HookDesktop(System::Windows::Interop::HwndSource^ source)
{
	if (hooked)
		return;

	m_source = source;
	m_source->AddHook(gcnew Windows::Interop::HwndSourceHook(this, &DEWin32Impl::DesktopWndProc));
	HideWindowFromAltTab(m_source->Handle);

	hooked = true;
}

void SDEE::CLI::Win32Lib::DEWin32Impl::UnhookDesktop()
{
	if (!hooked)
		return;

	hooked = false;

	m_source->RemoveHook(gcnew Windows::Interop::HwndSourceHook(this, &DEWin32Impl::DesktopWndProc));
}

void SDEE::CLI::Win32Lib::DEWin32Impl::HideWindowFromAltTab(System::IntPtr hWnd)
{
	SetWindowLong((HWND)hWnd.ToPointer(), GWL_EXSTYLE, WS_EX_TOOLWINDOW);
}

IntPtr SDEE::CLI::Win32Lib::DEWin32Impl::DesktopWndProc(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean% handled)
{
	auto result = InternalDesktopWndProc((HWND)hWnd.ToPointer(), (UINT)msg, (WPARAM)wParam.ToPointer(), (LPARAM)lParam.ToPointer(), handled);
	return (IntPtr)result;
}

LRESULT SDEE::CLI::Win32Lib::DEWin32Impl::InternalDesktopWndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam, bool% handled)
{
	if (msg == WM_MBUTTONUP) {
		MessageBox(0, L"MLKHJGF", 0, 0);
	}
	else if (msg == WM_WINDOWPOSCHANGING) {
		// Keep window at background
		((WINDOWPOS*)lParam)->flags |= SWP_NOZORDER;
	}

	return 0;
}

WindowsVersion SDEE::CLI::Win32Lib::DEWin32Impl::GetWindowsVersion()
{
	if (IsWindows10OrGreater()) return WindowsVersion::Win10;
	else if (IsWindows8Point1OrGreater()) return WindowsVersion::Win8Point1;
	else if (IsWindows8OrGreater()) return WindowsVersion::Win8;
	else if (IsWindows7SP1OrGreater()) return WindowsVersion::Win7SP1;
	else if (IsWindows7OrGreater()) return WindowsVersion::Win7;
	else if (IsWindowsVistaSP2OrGreater()) return WindowsVersion::WinVistaSP2;
	else if (IsWindowsVistaSP1OrGreater()) return WindowsVersion::WinVistaSP1;
	else if (IsWindowsVistaOrGreater()) return WindowsVersion::WinVista;
	else if (IsWindowsXPSP3OrGreater()) return WindowsVersion::WinXPSP3;
	else if (IsWindowsXPSP2OrGreater()) return WindowsVersion::WinXPSP2;
	else if (IsWindowsXPSP1OrGreater()) return WindowsVersion::WinXPSP1;
	else if (IsWindowsXPOrGreater()) return WindowsVersion::WinXP;
	else if (IsWindowsServer()) return WindowsVersion::AnyWinServer;
	else return WindowsVersion::OlderThanWinXPOrUnknown;
}
