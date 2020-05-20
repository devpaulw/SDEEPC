#include <windows.h>
#include "skineng_ext.h"

#define SkinEngine32DllPath TEXT("skineng32.dll")
#define SkinEngine32EntryFunc "SetHooks"

BOOL(*skinEngSetHooks)(BOOL);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance, 
    _In_opt_  HINSTANCE hPrevInstance,
    _In_ LPWSTR szCmdLine, 
    _In_ int iCmdShow)
{
    HANDLE hAck32;

    while (*szCmdLine == L' ') {
        szCmdLine += sizeof(TCHAR);
    }
    if (lstrcmp(szCmdLine, SKNGEXT_DESKENV_RUN32CMDLINE) != 0) {
        MessageBox(NULL, L"Don't run this executable directly.", L"skineng_run32.exe",
            MB_OK | MB_ICONEXCLAMATION | MB_SETFOREGROUND | MB_TOPMOST);
        return 1;
    }

    HMODULE engDll = LoadLibrary(SkinEngine32DllPath);

    if (engDll == NULL) {
        MessageBox(NULL, L"The x86 Skin Engine DLL could not be loaded.", L"skineng_run32.exe", // TODO Give Events too here
            MB_OK | MB_ICONERROR | MB_SETFOREGROUND | MB_TOPMOST);
        return 1;
    }

    skinEngSetHooks = (BOOL(*)(BOOL))GetProcAddress(engDll, SkinEngine32EntryFunc);

    if (skinEngSetHooks == NULL) {

        MessageBox(NULL, L"The Skin Engine DLL's entry function could not be found.", L"skineng_run32.exe",
            MB_OK | MB_ICONERROR | MB_SETFOREGROUND | MB_TOPMOST);
        return 1;
    }

    hAck32 = OpenEvent(EVENT_ALL_ACCESS, FALSE, SKNGEXT_DESKENV_RUN32EVENT);
    if (hAck32 == NULL)
        return 1;

    if (!skinEngSetHooks(TRUE)) {
        MessageBox(NULL, L"An issue has occurred while trying to start the Skin Engine", L"skineng_run32.exe",
            MB_OK | MB_ICONERROR | MB_SETFOREGROUND | MB_TOPMOST);
        return 1;
    }
    
    PulseEvent(hAck32);
    for (;;)
    {
        MSG msg;
        DWORD r;
        while (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE)) {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
        r = MsgWaitForMultipleObjects(1, &hAck32, FALSE, INFINITE, QS_ALLINPUT);
        if (r != WAIT_OBJECT_0 + 1) // TODO Put another security if necessarry
            break;
    }

    skinEngSetHooks(FALSE);
    CloseHandle(hAck32);

    return 0;
}