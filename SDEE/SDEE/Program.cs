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

            MyTaskbar myTaskbar = new MyTaskbar(de, 0.05f, new Color(0xC0, 0xC0, 0xC0));
            TaskbarElement testTe = new TaskbarElement(myTaskbar);
            myTaskbar.Controls.Add(testTe);

            de.Controls.Add(myTaskbar);

            de.Start();
        }
    }
}
