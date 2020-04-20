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
        public MyTaskbar(DesktopEnvironment parent, Color color) : base(
            parent, 
            color,
            position: new Vector2i(0, parent.Size.Y - 30), 
            size: new Vector2i(parent.Size.X, 30))
        {
            int borderLength = 5;
            Vector2i iconsSize = new Vector2i(Size.Y, Size.Y);

            var gErc = new ExtensibleRowContainer(this, borderLength);

            var startMenuButton = new SimpleRectControl(gErc, Color.Blue,
                position: new Vector2i(0, 0),
                size: new Vector2i(Size.Y, Size.Y));

            startMenuButton.Click += (s, e) => ToggleStartMenu(s, EventArgs.Empty);

            var erc = new ExtensibleRowContainer(gErc, borderLength);
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe"));
            var cmdExec = new Executable(erc, @"c:\windows\system32\cmd.exe") { Size = iconsSize };
            var notepadExec = new Executable(erc, @"c:\windows\notepad.exe") { Size = iconsSize };
 
            Load(startMenuButton);
            Load(gErc);
            Load(erc);
            Load(cmdExec);
            Load(notepadExec);
        }


        public event EventHandler ToggleStartMenu;
    }
}
// DOLATER Do an auto size prop