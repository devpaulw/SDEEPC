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
using SDEE.Sfml;

namespace SDEE
{
    class Program
    {
        static void Main(string[] args)
        {
            var de = new DesktopEnvironment(new Color(0, 0x80, 0b10000000));
            
            MyTaskbar myTaskbar = new MyTaskbar(0.05f, new Color(0xC0, 0xC0, 0xC0));
            TaskbarExecutable testTe = new TaskbarExecutable(@"c:\windows\system32\cmd.exe");
            myTaskbar.Controls.Add(testTe);
            myTaskbar.Controls.Add(new TaskbarExecutable(@"C:\Program Files (x86)\Microsoft Office\root\Office16\EXCEL.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(@"C:\Program Files (x86)\Microsoft Office\root\Office16\WINWORD.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(@"C:\Program Files (x86)\Microsoft Office\root\Office16\OUTLOOK.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(@"C:\Program Files (x86)\Microsoft Office\root\Office16\POWERPNT.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(@"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe"));
            myTaskbar.Controls.Add(new TaskbarExecutable(@"c:\windows\notepad.exe"));
            

            de.Controls.Add(myTaskbar);

            de.Start();
        }
    }
}
