using SFML.Graphics;
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
        public override ControlDrawing Drawing => new ControlDrawing(this, new RectangleShape()
        {
            FillColor = new Color(0, 0x80, 0b10000000)
        });

        public MyDesktopEnvironment()
        {
            // TODO Configure Size and Position even here
        }


        public override void Load()
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
            tc.Controls.Add(new Executable(tc, @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe") { Size = executablesSize }, new Vector2u(3, 4));
            Controls.Add(tc);
            #endregion

            ZControls.Add(myTaskbar); // UNDONE Do it ZControl
            ZControls.Add(myStartMenu);

            //var tzc = new TZC(this);
            //ZControls.Add(tzc);

            base.Load();
        }
    }

    // TODO ZOrder integrated
    // TODO UserControl
    // TODO Remove ask parent
    // TODO First windows

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
