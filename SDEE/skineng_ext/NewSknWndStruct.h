#pragma once
#include <Windows.h>

struct SknWndPropsStruct { // TODO rename the file too
	BOOL hasMaximizeBox;
	BOOL hasMinimizeBox;
	BOOL hasCloseBox;
	HWND baseHwnd;
	MINMAXINFO mmi;
};