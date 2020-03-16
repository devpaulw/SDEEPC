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

            var de = new SfmlDesktopEnvironment(new Color(0, 0x80, 0b10000000));

            MyTaskbar myTaskbar = new MyTaskbar(0.05f, new Color(0xC0, 0xC0, 0xC0));
            de.GuiElements.Add(myTaskbar);

            de.Start();
        }
    }
}
