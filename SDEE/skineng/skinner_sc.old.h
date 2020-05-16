#pragma once
#include "framework.h"

#define SKNG_UIDSUBCLASS 1452758807
#define SKNG_LOAD 1

BOOL setWndRegion(HWND hWnd);
LRESULT APIENTRY WndSkinProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);
BOOL SkinWindow(HWND hwnd);