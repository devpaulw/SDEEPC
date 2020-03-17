using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using Win32;
using static Win32.User;
using static Win32.Kernel;
using System.Runtime.InteropServices;

namespace SDEE
{
    class Program
    {
        static void Main(string[] args)
        {
            //STARTUPINFO si = new STARTUPINFO();
            //PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            //SECURITY_ATTRIBUTES sa = default;
            //Console.WriteLine(Win32.Kernel.CreateProcess(@"C:\Windows\notepad.exe",
            //    null, ref sa, ref sa, 0, 0, IntPtr.Zero, null, ref si, ref pi));

            //using (Process process = new Process())
            //{
            //    process.StartInfo.FileName = @"C:\Windows\notepad.exe";
            //    process.Start();

            //    IntPtr hwnd = IntPtr.Zero;

            //    while (!process.HasExited)
            //    {
            //        process.Refresh();
            //        if (process.MainWindowHandle.ToInt32() != 0)
            //        {
            //            hwnd = process.MainWindowHandle;
            //            break;
            //        }
            //    }

            //    Console.WriteLine(hwnd);
            //    //Console.WriteLine(process.MainWindowTitle);
            //    //var hwnd2 = (IntPtr)User.FindWindow(null, "Snipping Tool");
            //    //Console.WriteLine(hwnd2);

            //    User.SetWindowLong(hwnd,
            //      User.GWL_STYLE, User.GetWindowLong(hwnd, User.GWL_STYLE) & ~User.WS_OVERLAPPEDWINDOW);

            //    User.SetWindowText(hwnd, "HACK WA");
            //}

            TestWin();
            return;

            var de = new SfmlDesktopEnvironment(new Color(0, 0x80, 0b10000000));

            MyTaskbar myTaskbar = new MyTaskbar(0.05f, new Color(0xC0, 0xC0, 0xC0));
            de.GuiElements.Add(myTaskbar);

            de.Start();
        }

        //private delegate int WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        static void TestWin()
        {
            const int WS_EX_TOOLWINDOW = 0x80,
                    WS_EX_APPWINDOW = 0x40000;

            IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
            {
                return (IntPtr)DefWindowProc(hWnd, (int)msg, (int)wParam, (int)lParam);
            }

            int WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
            {
                switch (msg)
                {
                    case WM_CLOSE:
                        DestroyWindow(hWnd);
                        break;
                    case WM_DESTROY:
                        PostQuitMessage(0);
                        break;
                    default:
                        return DefWindowProc(hWnd, (int)msg, (int)wParam, (int)lParam);
                }
                return 0;
            }

            string g_szClassName = "myWindowClass";

            IntPtr hInstance = new IntPtr(GetModuleHandle(null));

            WNDCLASSEX wc;
            IntPtr hwnd;
            MSG Msg = new MSG();

            //Step 1: Registering the Window Class
            wc.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));
            wc.style = 0;
            wc.lpfnWndProc = (int)Marshal.GetFunctionPointerForDelegate((WndProc)CustomWndProc);
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hInstance = hInstance;
            wc.hIcon = (IntPtr)LoadIcon(IntPtr.Zero, ((char)IDI_APPLICATION).ToString());
            wc.hCursor = (IntPtr)LoadCursor(IntPtr.Zero, ((char)IDC_ARROW).ToString());
            wc.hbrBackground = (IntPtr)(COLOR_WINDOW + 1);
            wc.lpszMenuName = null;
            wc.lpszClassName = g_szClassName;
            wc.hIconSm = (IntPtr)LoadIcon(IntPtr.Zero, ((char)IDI_APPLICATION).ToString());

            if (RegisterClassEx(ref wc) == 0)
            {
                MessageBox(IntPtr.Zero, "Window Registration Failed!", "Error!",
                    MB_ICONEXCLAMATION | MB_OK);
                throw new Exception();
            }

            // Step 2: Creating the Window
            hwnd = (IntPtr)CreateWindowEx(
                WS_EX_APPWINDOW/* | WS_EX_TOPMOST*/,
                g_szClassName,
                "The title of my window",
                WS_OVERLAPPEDWINDOW,
                10, 10, 800, 600,
                IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                MessageBox(IntPtr.Zero, "Window Creation Failed!", "Error!",
                    MB_ICONEXCLAMATION | MB_OK);
                throw new Exception();
            }

            ShowWindow(hwnd, SW_SHOWDEFAULT);
            UpdateWindow(hwnd);

            // Step 3: The Message Loop
            while (GetMessage(ref Msg, IntPtr.Zero, 0, 0) > 0)
            {
                TranslateMessage(ref Msg);
                DispatchMessage(ref Msg);
            }
        }
    }
}
