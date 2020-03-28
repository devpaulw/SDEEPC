using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    class MyTaskbar : SimpleRectControl
    {
        public MyTaskbar(DesktopEnvironment parent, Color color) : base(parent, 
            color,
            position: new Vector2i(0, parent.Size.Y - 30), 
            size: new Vector2i(parent.Size.X, 30))
        {
            int borderLength = 5;

            var startMenuButton = new SimpleRectControl(this, Color.Blue, 
                position: new Vector2i(0, 0), 
                size: new Vector2i(Size.Y, Size.Y));

            startMenuButton.Click += (s, e) => ToggleStartMenu(s, EventArgs.Empty);

            var gErc = new ExtensibleRowContainer(this, borderLength);
            gErc.Controls.Add(startMenuButton);

            var erc = new ExtensibleRowContainer(gErc, borderLength);
            //erc.Position = new Vector2i(startMenuButton.Size.X + borderLength * 2, 0);
            var iconsSize = new Vector2i(Size.Y, Size.Y);
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe"));
            erc.Controls.Add(new Executable(erc, @"c:\windows\system32\cmd.exe") { Size = iconsSize }); // DOLATER Do an auto size prop
            erc.Controls.Add(new Executable(erc, @"c:\windows\notepad.exe") { Size = iconsSize });

            gErc.Controls.Add(erc);

            Controls.Add(gErc);
        }

        public event EventHandler ToggleStartMenu;
    }
}
