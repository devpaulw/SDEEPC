using SFML.Graphics;
using SFML.Window;
using System;
using System.Runtime.InteropServices;
using Win32;
using static Win32.User32;

namespace SDEE
{
    /// <summary>
    /// A class that manages well the relations between Win32 and SFML,
    /// suited for a SDEE Desktop Environment
    /// </summary>
    internal class SfW32DEWindow : RenderWindow
    {
        public SfW32DEWindow() : base(GetHandleWindow())
        {
        }

        /// <summary>
        /// Dispatch a message of Win32 if there was a message to peek
        /// </summary>
        public void DispatchSystemMessage()
        {
            MSG msg = new MSG();

            if (PeekMessage(ref msg, IntPtr.Zero, 0, 0, PM_REMOVE) > 0)
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        const int WS_EX_TOOLWINDOW = 0x80,
                  WS_EX_APPWINDOW = 0x40000;

        const string szClassName = "sdee";

        private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private static readonly WndProc myWndProcDelegate = MyWndProc;

        private static IntPtr MyWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_SETFOCUS: // When we try to focus the DE (whereas others windows might be above)
                    SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);// | SWP_NOOWNERZORDER);
                    break;

                //case WM_WINDOWPOSCHANGING: // Second solution of, but lags : When we try to focus the DE (whereas others windows might be above)
                //    var windowPos = Marshal.PtrToStructure<WINDOWPOS>(lParam);
                //    windowPos.flags |= SWP_NOZORDER;
                //    Marshal.StructureToPtr(windowPos, lParam, false);
                //    break;
                case WM_CLOSE:
                    DestroyWindow(hWnd);
                    break;
                case WM_DESTROY:
                    PostQuitMessage(0);
                    break;
                default:
                    return (IntPtr)DefWindowProc(hWnd, (int)msg, wParam.ToInt32(), lParam.ToInt32());
            }
            return IntPtr.Zero;
        }

        private static IntPtr GetHandleWindow()
        {
            var hInstance = new IntPtr(Kernel.GetModuleHandle(null));

            WNDCLASSEX wc = new WNDCLASSEX()
            {
                cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
                style = 0,
                lpfnWndProc = (int)Marshal.GetFunctionPointerForDelegate(myWndProcDelegate),
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = hInstance,
                hIcon = IntPtr.Zero,
                hCursor = IntPtr.Zero,
                hbrBackground = new IntPtr(COLOR_BACKGROUND),
                lpszMenuName = null,
                lpszClassName = szClassName,
                hIconSm = IntPtr.Zero
            };

            if (RegisterClassEx(ref wc) == 0)
            {
                MessageBox(IntPtr.Zero, "Window Registration Failed!", "Error!",
                    MB_ICONEXCLAMATION | MB_OK);
                throw new Exception();
            }

            IntPtr hWnd = (IntPtr)CreateWindowEx(
                WS_EX_TOOLWINDOW, // Remove window from Alt Tab
                szClassName,
                null,
                WS_POPUPWINDOW, // PWTD Make rather a fake fullscreen windowed (without border)s // DOLATER Remove borders without disturb good mecanism
                0, 0,
                (int)VideoMode.DesktopMode.Width, (int)VideoMode.DesktopMode.Height,
                IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);


            if (hWnd == IntPtr.Zero)
            {
                MessageBox(IntPtr.Zero, "Window Creation Failed!", "Error!",
                    MB_ICONEXCLAMATION | MB_OK);
                throw new Exception();
            }

            return hWnd;
        }
    }

    internal class SfW32ZCtrlWindow : RenderWindow
    {
        public SfW32ZCtrlWindow(VideoMode mode) : base(GetHandleWindow(mode))
        {
        }

        /// <summary>
        /// Dispatch a message of Win32 if there was a message to peek
        /// </summary>
        public void DispatchSystemMessage()
        {
            MSG msg = new MSG();

            if (PeekMessage(ref msg, IntPtr.Zero, 0, 0, PM_REMOVE) > 0)
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        const int WS_EX_TOOLWINDOW = 0x80,
                  WS_EX_APPWINDOW = 0x40000;

        const string szClassName = "sdee";

        private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private static readonly WndProc myWndProcDelegate = MyWndProc;

        private static IntPtr MyWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_SETFOCUS: // When we try to focus the DE (whereas others windows might be above)
                    SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER);// | SWP_NOOWNERZORDER);
                    break;
                case WM_WINDOWPOSCHANGED:
                    SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
                    break;
                //case WM_WINDOWPOSCHANGING: // Second solution of, but lags : When we try to focus the DE (whereas others windows might be above)
                //    var windowPos = Marshal.PtrToStructure<WINDOWPOS>(lParam);
                //    windowPos.flags |= SWP_NOZORDER;
                //    Marshal.StructureToPtr(windowPos, lParam, false);
                //    break;
                case WM_CLOSE:
                    DestroyWindow(hWnd);
                    break;
                case WM_DESTROY:
                    PostQuitMessage(0);
                    break;
                default:
                    return (IntPtr)DefWindowProc(hWnd, (int)msg, wParam.ToInt32(), lParam.ToInt32());
            }
            return IntPtr.Zero;
        }

        private static IntPtr GetHandleWindow(VideoMode videoMode)
        {
            var cn = "test" + new Random().Next(0, 1000); // tmp

            var hInstance = new IntPtr(Kernel.GetModuleHandle(null));

            WNDCLASSEX wc = new WNDCLASSEX()
            {
                cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
                style = 0,
                lpfnWndProc = (int)Marshal.GetFunctionPointerForDelegate(myWndProcDelegate),
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = hInstance,
                hIcon = IntPtr.Zero,
                hCursor = IntPtr.Zero,
                hbrBackground = new IntPtr(COLOR_BACKGROUND),
                lpszMenuName = null,
                lpszClassName = cn,
                hIconSm = IntPtr.Zero
            };

            if (RegisterClassEx(ref wc) == 0)
            {
                MessageBox(IntPtr.Zero, "Window Registration Failed!", "Error!",
                    MB_ICONEXCLAMATION | MB_OK);
                throw new Exception();
            }

            IntPtr hWnd = (IntPtr)CreateWindowEx(
                WS_EX_TOOLWINDOW, // Remove window from Alt Tab
                cn,
                null,
                WS_BORDER &~ WS_OVERLAPPEDWINDOW, // PWTD Make rather a fake fullscreen windowed (without border)s // DOLATER Remove borders without disturb good mecanism
                0, 0,
                (int)videoMode.Width, (int)videoMode.Height,
                IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);


            if (hWnd == IntPtr.Zero)
            {
                MessageBox(IntPtr.Zero, "Window Creation Failed!", "Error!",
                    MB_ICONEXCLAMATION | MB_OK);
                throw new Exception();
            }

            return hWnd;
        }
    }
}
