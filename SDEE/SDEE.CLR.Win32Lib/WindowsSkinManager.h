#pragma once
#include <Windows.h>
#include "SkinnedWindow.h"

namespace SDEE {
	namespace CLI {
		namespace Win32Lib { // TODO Rename these namespaces SDEE.Framework.CLINativeImpl
			//delegate LRESULT WndProcDelegate(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

			public ref class WindowsSkinManager
			{
			public:
				WindowsSkinManager(System::Func<SkinnedWindow^>^ getNewSkinWndCtrl);

				void StartEngine(System::Windows::Interop::HwndSource^ desktopEnvironmentSource);
				void StopEngine();

			private:
#ifdef _DEBUG
#ifdef _WIN64 // x64
				static const LPCWCHAR SkinEngineRun32Path = TEXT("..\\..\\..\\..\\Debug\\Win32\\skineng_run32.exe");
				static const LPCWCHAR SkinEngineDllPath = TEXT("..\\..\\..\\..\\Debug\\x64\\skineng.dll");
#else // Win32
				static const LPCWCHAR SkinEngineDllPath = TEXT("..\\..\\..\\..\\Debug\\Win32\\skineng.dll");
#endif
#else
#ifdef _WIN64 // x64
				static const LPCWCHAR SkinEngineRun32Path = TEXT("skineng_run32.exe");
#else // Win32
#endif
				static const LPCWCHAR SkinEngineDllPath = TEXT("skineng.dll");
#endif

				static const LPCSTR SkinEngineEntryFunc = "SetHooks"; 

				initonly System::Func<SkinnedWindow^>^ m_getNewSkndWnd;
				initonly System::Collections::Generic::List<SkinnedWindow^>^ openedSkinnedWindows;
				System::Windows::Interop::HwndSource^ m_desktopEnvironmentSource;
				UINT sknWndExtMsg = NULL;
				HANDLE shmemhMapObject;
				PVOID shmempBuf;
				UINT skngExtMsg = NULL;
				BOOL(*skinEngSetHooks)(BOOL);
				bool skinEngStarted = false;
				HWND deHWnd;
				HANDLE hAck32, hProcess32;

#ifdef _WIN64
				BOOL skinEngRun32(BOOL set);
#endif

				void DisposeOpenedSkinnedWindows();

				System::IntPtr DESkinEngRouterWndProc(System::IntPtr hWnd, System::Int32 msg, System::IntPtr wParam, System::IntPtr lParam, System::Boolean% handled);

				LRESULT InternalDESkinEngRouterWndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam, bool% handled);

				void DestroySharedMem();
				void CreateSharedMem();
			};
		}
	}
}