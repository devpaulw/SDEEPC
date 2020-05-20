#include "WindowsSkinManager.h"
#include "Exceptions.hpp"
#include "DEWin32Impl.h"
#include "skineng_ext.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::ComponentModel;
using namespace System::Collections::Generic;
using namespace System::Windows::Markup;
using namespace System::Windows::Input;
using namespace System::Threading;

SDEE::CLI::Win32Lib::WindowsSkinManager::WindowsSkinManager(System::Func<SkinnedWindow^>^ getNewSkinWnd)
{
	m_getNewSkndWnd = getNewSkinWnd;
	openedSkinnedWindows = gcnew List<SkinnedWindow^>();

#pragma region Load Skin Engine entry function
	HMODULE engDll(LoadLibrary(SkinEngineDllPath));

	if (engDll == NULL)
		throw gcnew SkinEngineException(L"The Skin Engine DLL could not be loaded.");

	skinEngSetHooks = (BOOL(*)(BOOL))GetProcAddress(engDll, SkinEngineEntryFunc);

	if (skinEngSetHooks == NULL) {
		throw gcnew SkinEngineException(L"The Skin Engine DLL's entry function could not be found.");
	}

#pragma endregion
}

void SDEE::CLI::Win32Lib::WindowsSkinManager::StartEngine(System::Windows::Interop::HwndSource^ desktopEnvironmentSource)
{
	if (skinEngStarted) // ALready started
		return;

	deHWnd = (HWND)desktopEnvironmentSource->Handle.ToPointer();
	if (deHWnd == NULL) {
		throw gcnew NullReferenceException(TEXT("Could not start the skin engine: Desktop Environment, HwndSource is empty"));
	}

	m_desktopEnvironmentSource = desktopEnvironmentSource;
	m_desktopEnvironmentSource->AddHook(gcnew Windows::Interop::HwndSourceHook(this, &WindowsSkinManager::DESkinEngRouterWndProc));

	CreateSharedMem();

	skngExtMsg = RegisterWindowMessage(SKNGEXT_STRMSG);
	if (!skngExtMsg)
		throw gcnew Win32Exception(GetLastError(), L"Could not register the Extern Skin Engine Window Message.");

	if (!skinEngSetHooks(TRUE)) // TODO Do true variables
		throw gcnew SkinEngineException(L"An issue has occurred while trying to start the Skin Engine");
#ifdef _WIN64
	if (!skinEngRun32(TRUE)) // TODO Do true variables instead of true false
		throw gcnew SkinEngineException(L"An issue has occurred while trying to start the Skin Engine");
#endif // _WIN64

	skinEngStarted = true;
}

void SDEE::CLI::Win32Lib::WindowsSkinManager::StopEngine()
{
	if (!skinEngStarted) // Already stopped or not started yet
		return;

	skinEngStarted = false;
	m_desktopEnvironmentSource->RemoveHook(gcnew Windows::Interop::HwndSourceHook(this, &WindowsSkinManager::DESkinEngRouterWndProc));

	DisposeOpenedSkinnedWindows();//

	BOOL engStoppedSuccessfully = TRUE;

	engStoppedSuccessfully &= skinEngSetHooks(FALSE);
#ifdef _WIN64
	engStoppedSuccessfully &= skinEngRun32(FALSE);
#endif // _WIN64

	DestroySharedMem();

	if (!engStoppedSuccessfully)
		throw gcnew SkinEngineException(L"The skin engine has not been shutdown successfully");
}

#ifdef _WIN64
BOOL SDEE::CLI::Win32Lib::WindowsSkinManager::skinEngRun32(BOOL set)
{
	if (set) {
		STARTUPINFO si;
		PROCESS_INFORMATION pi;
		BOOL ret;

		hAck32 = CreateEvent(NULL, FALSE, FALSE, SKNGEXT_DESKENV_RUN32EVENT);
		if (hAck32 == NULL)
			return FALSE;
		if (GetLastError() == ERROR_ALREADY_EXISTS)
			return TRUE; // TODO return TRUE anyway or other

		TCHAR cmdline[256];

		wsprintf(cmdline, L"\"%s\" %s",
			SkinEngineRun32Path,
			SKNGEXT_DESKENV_RUN32CMDLINE);

		ZeroMemory(&si, sizeof(si));
		si.cb = sizeof(si);
		ZeroMemory(&pi, sizeof(pi));
		ret = CreateProcess(NULL, cmdline, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi);
		if (ret == FALSE)
			return FALSE;

		CloseHandle(pi.hThread);
		hProcess32 = pi.hProcess;
		WaitForSingleObject(hAck32, 5000);
	}
	else {
		if (hAck32) {
			PulseEvent(hAck32);
			CloseHandle(hAck32);
		}
		if (hProcess32) {
			WaitForSingleObject(hProcess32, 5000);
			CloseHandle(hProcess32);
		}
		hAck32 = hProcess32 = NULL;
	}

	return TRUE;
}
#endif

void SDEE::CLI::Win32Lib::WindowsSkinManager::OnASkinnedWindowClosed(System::Object^ sender, System::EventArgs^ e)
{
	SkinnedWindow^ sw = (SkinnedWindow^)sender;
	openedSkinnedWindows->Remove(sw);
	delete sw; // Dispose when closed assuming closed is from the last message WM_NCDESTROY
}

void SDEE::CLI::Win32Lib::WindowsSkinManager::DisposeOpenedSkinnedWindows() // TODO Rename it UnloadOpened...
{
	for each (SkinnedWindow ^ sw in openedSkinnedWindows) {
		sw->Stop();
	}
}


System::IntPtr SDEE::CLI::Win32Lib::WindowsSkinManager::DESkinEngRouterWndProc(System::IntPtr hWnd, System::Int32 msg, System::IntPtr wParam, System::IntPtr lParam, System::Boolean% handled)
{
	auto convertedWndProc = (IntPtr)InternalDESkinEngRouterWndProc((HWND)hWnd.ToPointer(), (UINT)msg, (WPARAM)wParam.ToPointer(), (LPARAM)lParam.ToPointer(), handled);
	return convertedWndProc;
}

LRESULT SDEE::CLI::Win32Lib::WindowsSkinManager::InternalDESkinEngRouterWndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam, bool% handled)
{
	if (skinEngStarted && msg == skngExtMsg)
	{
		handled = true; // Because it's our own message, and it's a callback, we have to return the result ourself

		WORD skngExtMsgType = LOWORD(wParam),
			skngExtMsgValue = HIWORD(wParam);

		if (skngExtMsgType == SKNGEXT_APPWND_TYPEMSG) {
			switch (skngExtMsgValue) {
			case SKNGEXT_APPWND_MSG_NEWSKNWND:
			{
				HWND hWnd = (HWND)lParam;

				SkinnedWindow^ newSkndWnd = m_getNewSkndWnd();
				openedSkinnedWindows->Add(newSkndWnd); // TODO When unloaded by himself, remove from the list!
				newSkndWnd->Closed += gcnew EventHandler(this, &WindowsSkinManager::OnASkinnedWindowClosed);
				HWND sknWndHwnd = newSkndWnd->Run(hWnd);

				return (LONG_PTR)sknWndHwnd;
			}
			break;
			}
		}
	}

	return 0;
}
// TODO Thread exception like AccessViolationException catch.
void SDEE::CLI::Win32Lib::WindowsSkinManager::DestroySharedMem()
{
	UnmapViewOfFile(shmempBuf);
	CloseHandle(shmemhMapObject);
}

void SDEE::CLI::Win32Lib::WindowsSkinManager::CreateSharedMem()
{// Creating shared memory in order to share the Desktop Environment Handle
	const int size = sizeof HWND;

	shmemhMapObject = CreateFileMapping(
		INVALID_HANDLE_VALUE,             // use paging file
		NULL,                   // no security attributes
		PAGE_READWRITE,         // read/write access
		0,                      // size: high 32-bits
		size,                   // size: low 32-bits
		SKNGEXT_DESKENV_DESHMEMID      // name of map object
		);

	if (shmemhMapObject) {
		shmempBuf = MapViewOfFile(
			shmemhMapObject, // object to map view of
			FILE_MAP_WRITE, // write access
			0,		// high offset:  map from
			0,		// low offset:   beginning
			size);  // default: map entire file

		PVOID deHWndPtr =
			deHWnd == NULL
			? throw gcnew NullReferenceException(L"Could not create shared memory: Desktop Environment HWnd is null.")
			: deHWnd;

		if (shmempBuf) {
			memcpy(shmempBuf, &deHWndPtr, size);

		}
		else {
			CloseHandle(shmemhMapObject);

			throw gcnew Win32Exception(GetLastError(), L"Could not map view of file.");
		}
	}
	else {
		throw gcnew Win32Exception(GetLastError(), L"Couldn't create file mapping object.");
	}
}

// MORE IMPORTANT
// Windows 10 opti : make it work on win10: try with cmd close...
// Not allow not allowed things
// Exclusion system, don't take already overriden caption, perfectly overlap even when more difficult
// Cleaner design with ZONES
// indesirable windows work for most famous softwares
// good restorer "somewhere"
// Optimization so that window is not outside the screen
// do TODOs

// LESS IMPORTANT - EXCELLENCE
// Don't let the normal window show up at all
// Sync and perfs p2
// Details like blinking, perfect sync
// Skin wnd, I'm the app wnd, [moving/sizing briefly]
// Smoother resize
// Coordinated animations
// Cool ergonomic windows things like either sides automatic, etc...
// Compiler advertissements
// >>> Extrem reliability