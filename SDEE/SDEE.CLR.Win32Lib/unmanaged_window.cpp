//#include "unmanaged_window.h"
//
//BOOL unmanaged_window::register_class(HINSTANCE hInstance)
//{
//	WNDCLASSEX wc;
//
//	wc.cbSize = sizeof(WNDCLASSEX);
//	wc.style = 0;
//	wc.lpfnWndProc = MessageRouter;
//	wc.cbClsExtra = 0;
//	wc.cbWndExtra = 0;
//	wc.hInstance = hInstance;
//	wc.hIcon = LoadIcon(NULL, IDI_APPLICATION);
//	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
//	wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
//	wc.lpszMenuName = NULL;
//	wc.lpszClassName = szClassName;
//	wc.hIconSm = LoadIcon(NULL, IDI_APPLICATION);
//
//	if (!RegisterClassEx(&wc)) {
//		if (!(GetLastError() == ERROR_CLASS_ALREADY_EXISTS) ||true) // Be tolerant when class already registered
//			return FALSE;
//	}
//
//	return TRUE;
//}
//
//BOOL unmanaged_window::init_instance(HINSTANCE hInstance)
//{
//	HWND hWnd = CreateWindowEx(WS_EX_APPWINDOW, szClassName, NULL, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
//		NULL, NULL, hInstance, this); /*TODO adapt*/
//
//	if (!hWnd)
//		return FALSE;
//
//	PostMessage(hWnd, 7899645, 0, 0);
//
//	ShowWindow(hWnd, SW_NORMAL);
//	UpdateWindow(hWnd);
//
//	return TRUE;
//}
//
//void unmanaged_window::peek_msg()
//{
//	MSG msg;
//
//	if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE)) {
//		TranslateMessage(&msg);
//		DispatchMessage(&msg);
//	}
//}
//
//LRESULT unmanaged_window::MessageRouter(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
//{
//	unmanaged_window* uw;
//	if (msg == 7899645) {
//		uw = (unmanaged_window*)(((LPCREATESTRUCT)lParam)->lpCreateParams);
//		SetWindowLongPtr(hwnd, GWLP_USERDATA, (LONG_PTR)uw);
//	}
//	else {
//		uw = (unmanaged_window*)GetWindowLongPtr(hwnd, GWLP_USERDATA);
//	}
//
//	return uw->WndProc(hwnd, msg, wParam, lParam);
//}