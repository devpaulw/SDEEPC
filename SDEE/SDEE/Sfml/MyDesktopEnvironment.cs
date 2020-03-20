using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class MyDesktopEnvironment : DesktopEnvironment
    {
        protected override Shape Shape
            => new RectangleShape(this.GetBasicShape())
            {
                FillColor = new Color(0, 0x80, 0b10000000)
            };


        public MyDesktopEnvironment()
        {
        }

        protected override void Init()
        {
            MyTaskbar myTaskbar = new MyTaskbar(this, 0.05f, new Color(0xC0, 0xC0, 0xC0));
            TaskbarExecutable testTe = new TaskbarExecutable(myTaskbar, @"c:\windows\system32\cmd.exe");
            //myTaskbar.Controls.Add(new SimpleRectControl() { Color = Color.Blue, Size = new Vector2i(100, 100) });
            myTaskbar.Controls.Add(testTe);
            myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\EXCEL.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\WINWORD.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\OUTLOOK.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\POWERPNT.EXE"));
            myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe"));
            myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"c:\windows\notepad.exe"));

            Controls.Add(myTaskbar);

            base.Init();
        }
    }
}
