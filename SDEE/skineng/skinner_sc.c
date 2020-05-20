// ---
// Skinner subclass
// ---

#include "skinner_sc.h"
#include "skineng_ext.h"
#include <malloc.h>

#pragma region Secondary functions

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

BOOL removeOverlap(_In_ struct SkinnerStruct* sknStruct)
{
	LONG lStyle = GetWindowLong(sknStruct->appHWnd, GWL_STYLE);
	sknStruct->lBaseStyle = lStyle;
	lStyle &= ~WS_OVERLAPPEDWINDOW; // TODO Try without sysmenu and with

	SetLastError(0);
	if (!SetWindowLong(sknStruct->appHWnd, GWL_STYLE, lStyle))
		if (GetLastError() != ERROR_SUCCESS)
			return FALSE;

	LONG lExStyle = GetWindowLong(sknStruct->appHWnd, GWL_EXSTYLE);
	sknStruct->lBaseExStyle = lExStyle;
	lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
	if (!SetWindowLong(sknStruct->appHWnd, GWL_EXSTYLE, lExStyle))
		if (GetLastError() != ERROR_SUCCESS)
			return FALSE;

	if (!SetWindowPos(sknStruct->appHWnd, NULL, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER)) // TODO Try with SWP_REDRAW because of problems that can happens
		return FALSE;

	return TRUE;
}

void restoreOverlap(_In_ struct SkinnerStruct* sknStruct)
{
    SetWindowLong(sknStruct->appHWnd, GWL_STYLE, sknStruct->lBaseStyle);
	SetWindowLong(sknStruct->appHWnd, GWL_EXSTYLE, sknStruct->lBaseExStyle);
	SetWindowPos(sknStruct->appHWnd, NULL, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);
}

BOOL isCaptionWindow(HWND hWnd)
{
	LONG style = GetWindowLong(hWnd, GWL_STYLE);
	LONG exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

	// Ignore if the window has no caption
	if ((style & WS_CAPTION) != WS_CAPTION)
		return FALSE;

	// No sysmenu, it's not an overlapped window! 
	// TODO BAD
	if (!(style & WS_SYSMENU) && !(style & WS_VISIBLE))
		return FALSE;

	// When it's a child window without sysmenu
	if ((style & WS_CHILD)
		&& !(style & WS_SYSMENU)) // Child windows are excluded
		return FALSE;

	// DOLATER Possibility missing: Is Child SysMenu/ MDI client, windows without sysmenu attention

	return TRUE;
}
#pragma endregion

void HookWindow(HWND hWnd) // TODO IMPORTANT Deprectate SendMessageCallback and do instead a SendMessage with a pointers for performances reasons so that we can't see the original Windows caption
{
	if (!isCaptionWindow(hWnd))
		return;

	if (GetProp(hWnd, SKNSCAPPWND_HOOKED) == TRUE) // TODO Check otherwise effects
		return;

	SetProp(hWnd, SKNSCAPPWND_HOOKED, TRUE); // TODO Compare this here in regard to most-below strategy

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

	sknStruct->sknWndHwnd = SendMessage(sknStruct->deHWnd, sknStruct->skngExtMsg, // UNDONE COPYDATA STRUCT
		MAKEWPARAM(SKNGEXT_APPWND_TYPEMSG, SKNGEXT_APPWND_MSG_NEWSKNWND),
		sknStruct->appHWnd);

	if (sknStruct->sknWndHwnd == NULL)
		return; // TODO restorer somewhere (or something like this)

	if (!removeOverlap(sknStruct)) {
		return;
	}

	if (!SetWindowSubclass(sknStruct->appHWnd, AppSubclassProc, SKNSCAPPWND_UIDSUBCLASS, sknStruct)) // TODO Use rather dwRefData!
		return;

	// BIN BELOW
	//if (FALSE == SendMessage(sknStruct->deHWnd, sknStruct->skngExtMsg,
	//	MAKEWPARAM(SKNGEXT_APPWND_TYPEMSG, SKNGEXT_APPWND_MSG_SKNREQUEST),
	//	sknStruct->appHWnd)) // UNDONE TODO No multiple skin windows, and callback problem
	//	return;

	// Initialization of the NewSknWnd Struct
	// Get infos about the boxes minimize, maximize and close.
	//LONG lStyle = GetWindowLong(sknStruct->appHWnd, GWL_STYLE);
	//sknStruct->newSknWndStruct.hasMaximizeBox = (lStyle & WS_MAXIMIZEBOX) == WS_MAXIMIZEBOX;
	//sknStruct->newSknWndStruct.hasMinimizeBox = (lStyle & WS_MINIMIZEBOX) == WS_MINIMIZEBOX;
	//sknStruct->newSknWndStruct.hasCloseBox = TRUE; // TODO Handle close ,or no sysmenu at all, etc... Good luck
	//sknStruct->newSknWndStruct.baseHwnd = sknStruct->appHWnd;
	//ZeroMemory(&sknStruct->newSknWndStruct.mmi, sizeof(MINMAXINFO));
	
	//COPYDATASTRUCT cdt;
	//cdt.dwData = MAKEWPARAM(SKNGEXT_APPWND_TYPEMSG, SKNGEXT_APPWND_MSG_NEWSKNWND);
	//cdt.cbData = sizeof(struct NewSknWndStruct);
	//cdt.lpData = &sknStruct->newSknWndStruct; // TODO COmpare with PostMessage here
}

LRESULT APIENTRY AppSubclassProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam, UINT_PTR uIdSubclass, DWORD_PTR dwRefData)
{
	struct SkinnerStruct* sknStruct = (struct SkinnerStruct*)dwRefData;

	//if (msg == sknStruct->skngExtMsg) {
	//	WORD msgType = LOWORD(wParam);
	//	WORD msgValue = HIWORD(wParam);

	//	if (msgType == SKNGEXT_DESKENV_TYPEMSG) {

	//		switch (msgValue) {
	//		case SKNGEXT_DESKENV_MSG_SKNWNDHWND: 
	//		{
	//			sknStruct->sknWndHWnd = (HWND)lParam;
	//		}
	//		break;
	//		default:
	//			break;
	//		}
	//	}

	//}

	// TODO When detach, send to deskenv so that it can remove appWnd from its list
	switch (msg)
	{
	case WM_DESTROY:
	{
		/*PostMessage(sknStruct->deHWnd, sknStruct->skngExtMsg, 
			MAKEWPARAM(SKNGEXT_APPWND_TYPEMSG, SKNGEXT_APPWND_MSG_CLOSE), sknStruct->appHWnd);*/

		PostMessage(sknStruct->sknWndHwnd, WM_CLOSE, SKNGEXT_APPWND_TYPEMSG, 0);
		dispose_skinner(sknStruct);

		break;
	}
	//case WM_ACTIVATE: {;
	//	if (LOWORD(wParam) == WA_INACTIVE) // Is going to be deactivated//
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
	// Restoration
	restoreOverlap(skinnerStruct);

	RemoveWindowSubclass(skinnerStruct->appHWnd, AppSubclassProc, SKNSCAPPWND_UIDSUBCLASS); // TODO When unhook too
	free(skinnerStruct);
}