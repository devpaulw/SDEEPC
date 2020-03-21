using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
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

        protected override void Load()
        {
            MyTaskbar myTaskbar = new MyTaskbar(this, new Color(0xC0, 0xC0, 0xC0));

            MyStartMenu myStartMenu = new MyStartMenu(this);
            myStartMenu.Position = new Vector2i(myStartMenu.Position.X, myStartMenu.Position.Y - myTaskbar.Size.Y);
            myStartMenu.IsEnabled = false;

            myTaskbar.ToggleStartMenu += (s, e) => myStartMenu.IsEnabled ^= true;

            #region Tests

            var tc = new TableContainer(this, 4, 4, 50, 50, 5);
            var executablesSize = new Vector2i(50, 50);
            tc.Controls.Add(new Executable(tc, @"c:\windows\notepad.exe") { Size = executablesSize }, new Vector2u(0, 0));
            tc.Controls.Add(new Executable(tc, @"c:\windows\system32\cmd.exe") { Size = executablesSize }, new Vector2u(1, 0));
            tc.Controls.Add(new Executable(tc, @"c:\windows\regedit.exe") { Size = executablesSize }, new Vector2u(0, 1));
            tc.Controls.Add(new Executable(tc, @"c:\windows\system32\winver.exe") { Size = executablesSize }, new Vector2u(1, 1));
            Controls.Add(tc);
            #endregion

            Controls.Add(myTaskbar);
            Controls.Add(myStartMenu);

            base.Load();
        }
    }
}
