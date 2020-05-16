#include "DEWindow.h"

using namespace System;
using namespace System::Drawing;
using namespace System::Runtime::InteropServices;

SDEE::CLI::Win32Lib::DEWindow::DEWindow()
{
	hInstance = GetModuleHandle(NULL);

	wndProcDelegate = gcnew WndProcDelegate(this, &DEWindow::WndProc);

	register_class();
	init_instance();

	isOpen = true;
}

void SDEE::CLI::Win32Lib::DEWindow::DispatchMessages()
{
	peek_msg();
}

void SDEE::CLI::Win32Lib::DEWindow::Close()
{
	if (!isOpen)
		return;

	DestroyWindow(hWnd);
}

void SDEE::CLI::Win32Lib::DEWindow::register_class()
{
	WNDCLASSEX wc;

	wc.cbSize = sizeof(WNDCLASSEX);
	wc.style = 0;
	wc.lpfnWndProc = (WNDPROC)(Marshal::GetFunctionPointerForDelegate(wndProcDelegate).ToPointer());
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = hInstance;
	wc.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	wc.lpszMenuName = NULL;
	wc.lpszClassName = szClassName;
	wc.hIconSm = LoadIcon(NULL, IDI_APPLICATION);

	if (!RegisterClassEx(&wc)) {
		DWORD lastError = GetLastError();
		if (!(lastError == ERROR_CLASS_ALREADY_EXISTS)) // Be tolerant when class already registered
			throw gcnew WindowCreationException(lastError, "Could not register the window class.");
	}
}

void SDEE::CLI::Win32Lib::DEWindow::init_instance()
{
	hWnd = CreateWindowEx(WS_EX_TOOLWINDOW, szClassName, NULL, WS_VISIBLE, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
		NULL, NULL, hInstance, NULL); /*TODO adapt*/

	if (!hWnd)
		throw gcnew WindowCreationException(GetLastError(), "Could not create the Window.");
}

void SDEE::CLI::Win32Lib::DEWindow::peek_msg()
{
	MSG msg;

	if (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE)) {
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
}

LRESULT SDEE::CLI::Win32Lib::DEWindow::WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg) {
	case WM_PAINT: {
		PAINTSTRUCT ps;
		HDC hdc = BeginPaint(hwnd, &ps);

		Graphics^ g = Graphics::FromHdc(IntPtr(hdc));
		Paint(this, g);

		EndPaint(hwnd, &ps);

		break;
	}
	case WM_LBUTTONUP:
		break;
	case WM_DESTROY:
		isOpen = false;
		break;
	}

	return DefWindowProc(hwnd, msg, wParam, lParam);
}

SDEE::CLI::Win32Lib::DEWindow::~DEWindow()
{
	Destroy();
}

SDEE::CLI::Win32Lib::DEWindow::!DEWindow()
{
	Destroy();
}

void SDEE::CLI::Win32Lib::DEWindow::Destroy()
{
	Close();
}