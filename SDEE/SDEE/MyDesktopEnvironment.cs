﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    class MyDesktopEnvironment : DesktopEnvironment
    {
        private protected override Shape Shape => new RectangleShape()
        {
            FillColor = new Color(0, 0x80, 0b10000000)
        };

        public MyDesktopEnvironment()
        {
            MyTaskbar myTaskbar = new MyTaskbar(this, new Color(0xC0, 0xC0, 0xC0));
            myTaskbar.SetZ(ZOrder.Top);

            MyStartMenu myStartMenu = new MyStartMenu(this)
            {
                Position = new Vector2i(0, Size.Y - 400 - myTaskbar.Size.Y),
                IsEnabled = false
            };
            myStartMenu.SetZ(ZOrder.Top);

            myTaskbar.ToggleStartMenu += (s, e) => myStartMenu.IsEnabled ^= true;

            #region Tests

            var tc = new TableContainer(this, 4, 4, 50, 50, 5);
            var executablesSize = new Vector2i(50, 50);

            Load(new Executable(tc, @"c:\windows\notepad.exe") { Size = executablesSize }, new Vector2u(0, 0));
            Load(new Executable(tc, @"c:\windows\system32\cmd.exe") { Size = executablesSize }, new Vector2u(1, 0));
            Load(new Executable(tc, @"c:\windows\regedit.exe") { Size = executablesSize }, new Vector2u(0, 1));
            Load(new Executable(tc, @"c:\windows\system32\winver.exe") { Size = executablesSize }, new Vector2u(1, 1));
            Load(new Executable(tc, @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe") { Size = executablesSize }, new Vector2u(3, 4));

            #endregion

            Load(tc);
            Load(myTaskbar);
            Load(myStartMenu);
        }

        //public override void Load()
        //{
        //    base.Load();
        //}
    }

    //class TZC : ZControl
    //{
    //    public override Shape Shape =>
    //        new RectangleShape(this.GetBasicShape())
    //        {
    //            FillColor = Color.Blue
    //        };

    //    public TZC(DesktopEnvironment parent) : base(parent)
    //    {
    //        Position = new Vector2i(10 , 10);
    //        Size = new Vector2i(50, 70);
    //    }

    //    protected override void OnClick(MouseButtonEventArgs e)
    //    {
    //        MessageBox("Hey", null, MessageBoxIcon.Asterisk);

    //        base.OnClick(e);
    //    }
    //}
}
