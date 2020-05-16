// ---
// Skinner subclass
// ---

#include "skinner_sc.h"
#include <Uxtheme.h>

WNDPROC origWndProc;

HWND shwnd;

const char g_szClassName[] = "myWindowClass";
// Step 4: the Window Procedure
LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg)
	{
	case WM_WINDOWPOSCHANGED:
	{

		break;
		MessageBox(NULL, L"CUSTOM", 0, 0);
		WINDOWPOS* wp = (WINDOWPOS*)lParam;

		//result = DefSubclassProc(hwnd, msg, wParam, lParam);

		//WINDOWPOS wpCopy = *wp;
		//wpCopy.x -= 20;
		//wpCopy.y -= 20;
		//wpCopy.cx += 40;
		//wpCopy.cy += 40;

		//SetWindowPos(hwnd, HWND_TOP, wp->x, wp->y, 800, 700, 0);
		break;
	}
	case WM_CLOSE:
		DestroyWindow(hwnd);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hwnd, msg, wParam, lParam);
	}
	return 0;
}

DWORD WINAPI ThreadFunc(void* data) {
	//WCHAR buf[16];
	//wsprintfW(buf, L"%d", (int)data);

	//MessageBox(0, L"Hello the world.", buf, 0);

	HINSTANCE hInstance = GetModuleHandle(NULL);

	WNDCLASSEX wc;
	HWND hwnd;
	MSG Msg;//Step 1: Registering the Window Class
	wc.cbSize = sizeof(WNDCLASSEX);
	wc.style = 0;
	wc.lpfnWndProc = WndProc;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = hInstance;
	wc.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	wc.lpszMenuName = NULL;
	wc.lpszClassName = g_szClassName;
	wc.hIconSm = LoadIcon(NULL, IDI_APPLICATION);

	if (!RegisterClassExW(&wc)) {
		return 0;
	}

	//if (!RegisterClassExW(&wc))
	//{
	//	MessageBoxW(NULL, L"Window Registration Failed!", L"Error!",
	//		MB_ICONEXCLAMATION | MB_OK);
	//	return 0;
	//}

	// Step 2: Creating the Window
	hwnd = CreateWindowExW(
		WS_EX_CLIENTEDGE,
		g_szClassName,
		L"The title of my window",
		WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, CW_USEDEFAULT, 500, 500,
		NULL, NULL, hInstance, NULL);

	if (hwnd == NULL)
	{
		MessageBoxW(NULL, L"Window Creation Failed!", L"Error!",
			MB_ICONEXCLAMATION | MB_OK);
		return 0;
	}

	ShowWindow(hwnd, SW_NORMAL);
	UpdateWindow(hwnd);

	shwnd = hwnd;

	// Step 3: The Message Loop
	while (GetMessageW(&Msg, NULL, 0, 0) > 0)
	{
		TranslateMessage(&Msg);
		DispatchMessage(&Msg);
	}

	return Msg.wParam;
}

BOOL SkinWindow(HWND hWnd) {
	if (GetProp(hWnd, L"hooked"))
		return TRUE;
	SetProp(hWnd, L"hooked", 1);

	LONG style = GetWindowLong(hWnd, GWL_STYLE);
	LONG exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

	// Ignore if the window has no caption
	if ((style & WS_CAPTION) != WS_CAPTION)
		return TRUE;
	// Remove borders
	{
		//// TODO understand better
		//LONG lStyle = GetWindowLong(hWnd, GWL_STYLE);
		//lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
		//SetWindowLong(hWnd, GWL_STYLE, lStyle);

		//LONG lExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
		//lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
		//SetWindowLong(hWnd, GWL_EXSTYLE, lExStyle);

		//SetWindowPos(hWnd, NULL, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);
	}

	//WCHAR cn[256];
	//GetClassName(hWnd, cn, 256);

	static HANDLE thread;
	thread = CreateThread(NULL, 0, ThreadFunc, NULL, 0, NULL);

	/*if (thread != NULL) {
		WaitForSingleObject(thread, 2500);
	}*/

	//SetWindowThemeNonClientAttributes(hWnd, WTNCA_NODRAWCAPTION, WTNCA_NODRAWCAPTION);
	//SetWindowTheme(hWnd, L"", L"");

	//SetWindowThemeNonClientAttributes(hWnd, WTNCA_NODRAWCAPTION, WTNCA_NODRAWCAPTION);

	//setWndRegion(hWnd);
	SetWindowSubclass(hWnd, WndSkinProc, SKNG_UIDSUBCLASS, 0);
	//origWndProc = (WNDPROC)SetWindowLongPtr(hWnd, GWLP_WNDPROC, (LONG_PTR)WndSkinProc);

	return TRUE; // TEMP 
}

BOOL setWndRegion(HWND hWnd) {
	RECT rc;
	GetWindowRect(hWnd, &rc);
	int width = rc.right - rc.left,
		height = rc.bottom - rc.top;

	// TODO Cut

	int hiddenTop = 20,
		hiddenSide = 5,
		hiddenBottom = 5;

	//HRGN hrgn = CreateRectRgn(hiddenSide, hiddenTop, width - hiddenSide, height - hiddenBottom);
	HRGN hrgn = CreateRectRgn(0, 0, width * 2, height);
	SetWindowRgn(hWnd, hrgn, TRUE);

	return TRUE; // TODO: tmp, is that BOOL really useful?
}

HRGN getWndCutHrgn(HWND hWnd) {
	RECT rc;
	GetWindowRect(hWnd, &rc);
	HRGN hrgn = CreateRectRgn(
		rc.left,
		rc.top + 30,
		rc.right,
		rc.bottom
		);
}

void paintWnd(HWND hWnd) {
	RECT rc;
	GetWindowRect(hWnd, &rc);

	HDC hdc = GetWindowDC(hWnd);
	HGDIOBJ otherPen = (HGDIOBJ)0;

	for (size_t i = 0; i < 30; i++)
	{
		otherPen = SelectObject(hdc, CreatePen(PS_SOLID, 1, RGB(255 - (unsigned long)((255 / 30) * i), 20, 10)));
		MoveToEx(hdc, 0, i, NULL);
		LineTo(hdc, rc.right - rc.left, i);
	}

	DeleteObject(SelectObject(hdc, otherPen));
	ReleaseDC(hWnd, hdc);
}

WCHAR** wtostr(int value) {
	WCHAR sv[64];
	wsprintfW(sv, L"%d", value);

	return sv;
}

LRESULT APIENTRY WndSkinProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	LRESULT result = (LONG_PTR)0;

	switch (msg) { // TODO MetafilesCreatePen(PS_SOLID, 1, (unsigned long)((maxV / 30) * i))
	case WM_MBUTTONUP: {
		//MessageBox(NULL, L"Mbu", 0, 0);

		//WCHAR buf[16];
		//wsprintfW(buf, L"%d", (int)shwnd);
		//MessageBox(NULL, buf, 0, 0);

		//SendMessage(shwnd, 0x03004984, NULL, NULL);
		break;
	}
	case WM_WINDOWPOSCHANGING: {
	    WINDOWPOS* wp = (WINDOWPOS*)lParam;

		//result = DefSubclassProc(hwnd, msg, wParam, lParam);

		//WINDOWPOS wpCopy = *wp;
		//wpCopy.x -= 20;
		//wpCopy.y -= 20;
		//wpCopy.cx += 40;
		//wpCopy.cy += 40;

		//SetWindowPos(shwnd, HWND_TOP, 10, 10, 500, 500, 0);

		SendMessage(shwnd, WM_WINDOWPOSCHANGED, NULL, (LPARAM)wp);

		break;
	}
	//case WM_MBUTTONUP: {
	//	RECT rc;
	//	GetWindowRect(hwnd, &rc);
	//	HRGN hrgn = CreateRectRgn(
	//		rc.left + 5,
	//		rc.top + 10,
	//		rc.right + 5,
	//		rc.bottom + 5
	//		);
	//	SetWindowRgn(hwnd, hrgn, TRUE);

	//}

	//				 break;
	//case WM_NCPAINT: {

	//	HRGN wndCutHrgn = getWndCutHrgn(hwnd);
	//	result = DefSubclassProc(hwnd, msg, (WPARAM)wndCutHrgn, lParam);
	//	DeleteObject(wndCutHrgn);

	//	paintWnd(hwnd);
	//	break;
	//}
	//case WM_WINDOWPOSCHANGING:
	//{
	//	/*WINDOWPOS* wp = (WINDOWPOS*)lParam;

	//	if (wp->cx > 0 && wp->cy > 0) {
	//		HRGN hrgn = CreateRectRgn(wp->x, wp->y, wp->cx, wp->cy);
	//		SetWindowRgn(hwnd, hrgn, TRUE);
	//	}*/

	//	break;
	//}
	case WM_DESTROY:
		RemoveWindowSubclass(hwnd, WndSkinProc, SKNG_UIDSUBCLASS);
		break;
	default:
		result = DefSubclassProc(hwnd, msg, wParam, lParam);
		//result = CallWindowProc(origWndProc, hwnd, msg, wParam, lParam);
	}

	return DefSubclassProc(hwnd, msg, wParam, lParam);//result;
}