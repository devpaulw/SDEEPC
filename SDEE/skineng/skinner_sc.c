// ---
// Skinner subclass
// ---

#include "skinner_sc.h"
#include "skineng_ext.h"
#include <malloc.h>

void HookWindow(HWND hWnd)
{
	// TODO all of this in thread
	if (!isCaptionWindow(hWnd))
		return;

	if (GetProp(hWnd, SKNSCAPPWND_HOOKED) == TRUE) // TODO Check otherwise effects
		return;

	struct SkinnerStruct* sknStruct = (struct SkinnerStruct*)malloc(sizeof(struct SkinnerStruct));
	if (!sknStruct)
		return;

	sknStruct->appHWnd = hWnd;

	sknStruct->deHWnd = getDEHWndFromShMem();
	if (!sknStruct->deHWnd)
		return;

	DWORD wndPId, dePid;
	GetWindowThreadProcessId(sknStruct->appHWnd, &wndPId);
	GetWindowThreadProcessId(sknStruct->deHWnd, &dePid);

	if (wndPId != 0
		&& dePid != 0
		&& wndPId == dePid) // Is already a skin window (coming from the same process of the DE), don't fall into loop
		return;

	sknStruct->skngExtMsg = RegisterWindowMessage(SKNGEXT_STRMSG);
	if (!sknStruct->skngExtMsg)
		return; // ISSUE: If an operation fails, what's next? There should be a RESTORER somewhere

	if (!SendMessageCallback(sknStruct->deHWnd, sknStruct->skngExtMsg, 
		MAKEWPARAM(SKNGEXT_APPWND_TYPEMSG, SKNGEXT_APPWND_MSG_SKNREQUEST), 
		sknStruct->appHWnd, SkinRequestSendAsyncProc, sknStruct)) // UNDONE TODO No multiple skin windows, and callback problem
		return;
}

VOID CALLBACK SkinRequestSendAsyncProc(HWND hWnd, UINT uMsg, ULONG_PTR dwData, LRESULT lResult) // This skin request is useless yet, but provides the exclusion list, for example ; warning it makes it slower to remove borders
{
	// Hook window part. 2 after response received
	struct SkinnerStruct* sknStruct = dwData;

	if (uMsg != sknStruct->skngExtMsg)
		return;

	if (!removeOverlap(sknStruct->appHWnd)) // TODO restorer somewhere
		return;

	BOOL response = lResult;
	if (response == FALSE) // Request denied, don't skin the window.
		return;

	if (!SendMessageCallback(sknStruct->deHWnd, sknStruct->skngExtMsg, 
		MAKEWPARAM(SKNGEXT_APPWND_TYPEMSG, SKNGEXT_APPWND_MSG_NEWSKNWND),
		sknStruct->appHWnd, NewWndSendAsyncProc, sknStruct))
		return;
}

VOID CALLBACK NewWndSendAsyncProc(HWND hWnd, UINT uMsg, ULONG_PTR dwData, LRESULT lResult)
{
	// Hook window part. 3 after new wnd obtained
	struct SkinnerStruct* sknStruct = dwData;

	if (uMsg != sknStruct->skngExtMsg)
		return;

	if (!lResult)
		return;
	sknStruct->sknWndHWnd = lResult;
	
	if (!SetWindowSubclass(sknStruct->appHWnd, AppSubclassProc, SKNSCAPPWND_UIDSUBCLASS, sknStruct)) // TODO Use rather dwRefData!
		return;

	SetProp(sknStruct->appHWnd, SKNSCAPPWND_HOOKED, TRUE); // Everything Succeeded, we set it hooked
}

HWND getDEHWndFromShMem()
{
	int size = sizeof(PVOID);

	HANDLE hMapFile;

	hMapFile = OpenFileMapping(
		FILE_MAP_READ,   // read access
		FALSE,                 // do not inherit the name
		SKNGEXT_DESKENV_DESHMEMID);               // name of mapping object

	if (hMapFile == NULL)
	{
		//MessageBox(0, TEXT("Could not open file mapping object (%d).\n"), 0, 0);
		return 0;
	}

	PVOID* pBuf = (PVOID*)MapViewOfFile(hMapFile, // handle to map object
		FILE_MAP_READ,  // read permission
		0,
		0,
		size);

	if (pBuf == NULL)
	{
		//MessageBox(NULL, TEXT("Could not map view of file (%d).\n"), 0, 0);

		CloseHandle(hMapFile);
		return 0;
	}

	HWND deHWnd = (HWND)(*pBuf);

	UnmapViewOfFile(&pBuf);
	CloseHandle(hMapFile);

	return deHWnd;
}

BOOL removeOverlap(HWND hWnd)
{
	LONG lStyle = GetWindowLong(hWnd, GWL_STYLE);
	lStyle &= ~ WS_OVERLAPPEDWINDOW; // TODO Try without sysmenu and with
	
	SetLastError(0);
	if (!SetWindowLong(hWnd, GWL_STYLE, lStyle))
		if (GetLastError() != ERROR_SUCCESS)
			return FALSE;

	LONG lExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
	lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
	if (!SetWindowLong(hWnd, GWL_EXSTYLE, lExStyle))
		if (GetLastError() != ERROR_SUCCESS)
			return FALSE;

	if (!SetWindowPos(hWnd, NULL, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER)) // TODO Try with SWP_REDRAW because of problems that can happens
		return FALSE;

	return TRUE;
}

BOOL isCaptionWindow(HWND hWnd)
{
	LONG style = GetWindowLong(hWnd, GWL_STYLE);
	LONG exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
	
	// Ignore if the window has no caption
	if ((style & WS_CAPTION) != WS_CAPTION)
		return FALSE;

	// No sysmenu, it's not an overlapped window!
	if (!(style & WS_SYSMENU) && !(style & WS_VISIBLE))
		return FALSE;

	// When it's a child window without sysmenu
	if ((style & WS_CHILD)
		&& !(style & WS_SYSMENU)) // Child windows are excluded
		return FALSE;

	// DOLATER Possibility missing: Is Child SysMenu/ MDI client, windows without sysmenu attention

	return TRUE;
}

LRESULT APIENTRY AppSubclassProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam, UINT_PTR uIdSubclass, DWORD_PTR dwRefData)
{
	struct SkinnerStruct* sknStruct = (struct SkinnerStruct*)dwRefData;
	HWND sknHwnd = sknStruct->sknWndHWnd;

	//if (msg == sknStruct->sknWndExtMsg) {
	//	switch (wParam) {
	//	case SKNWNDEXT_WINDOWPOSCHANGED_PARAM: {
	//		SetActiveWindow(hwnd);
	//		break;
	//		WINDOWPOS* wp = (WINDOWPOS*)lParam;
	//		SetWindowPos(hwnd, HWND_TOP, wp->x, wp->y, wp->cx, wp->cy, 0); // TODO Try with SWP_ASYNCWINDOWPOS, no custom message

	//		break;
	//	}
	//	}
	//	return 0;
	//}

	// TODO When detach, send to deskenv so that it can remove appWnd from its list
	switch (msg)
	{
	case WM_DESTROY:
	{
		SendMessage(sknHwnd, WM_CLOSE, 0, 0);
		break;
	}
	//case WM_ACTIVATE: {;
	//	if (LOWORD(wParam) == WA_INACTIVE) // Is going to be deactivated
	//		break;

	//	SetWindowPos(sknHwnd, HWND_TOP, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOSIZE | SWP_NOMOVE);

	//	break;
	//}
	//case WM_SYSCOMMAND: {
	//	switch (wParam) {
	//	case SC_MINIMIZE: {
	//		ShowWindow(sknHwnd, SW_FORCEMINIMIZE);
	//		break;
	//	}
	//	case SC_RESTORE: {
	//		ShowWindow(sknHwnd, SW_RESTORE);
	//		break;
	//	}
	//	}
	//	break;
	//}
	case WM_NCDESTROY:
		dispose_skinner(sknStruct);
		break;
	default:
		if (msg == sknStruct->skngExtMsg) {
			WORD skngExtMsgType = LOWORD(wParam),
				skngExtMsgValue = HIWORD(wParam);

			switch (skngExtMsgType) {
			case SKNGEXT_SKNWND_TYPEMSG: {
				switch (skngExtMsgValue) {
				case SKNGEXT_SKNWND_MSG_UNLOAD: {
					dispose_skinner(sknStruct);

					break;
				}
				}

				break;
			}
			}
			break;
		}

		return DefSubclassProc(hwnd, msg, wParam, lParam);
	}

	return DefSubclassProc(hwnd, msg, wParam, lParam); // TODO return 0 or this?
}

void dispose_skinner(struct SkinnerStruct* skinnerStruct)
{
	RemoveWindowSubclass(skinnerStruct->appHWnd, AppSubclassProc, SKNSCAPPWND_UIDSUBCLASS); // TODO When unhook too
	free(skinnerStruct);
}