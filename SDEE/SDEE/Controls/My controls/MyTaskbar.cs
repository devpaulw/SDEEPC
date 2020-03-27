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
    class MyTaskbar : Control
    {
        public Color Color { get; set; }

        protected override Shape Shape => new RectangleShape()
        {
            FillColor = Color
        };

        public override bool NoSize => true;
        public override bool NoMove => true;

        public MyTaskbar(DesktopEnvironment parent, Color color) : base(parent)
        {
            Color = color;
            Position = new Vector2i(0, (Parent.Size.Y - 30));
            Size = new Vector2i(Parent.Size.X, 30);

            int borderLength = 5;

            var startMenuButton = new SimpleRectControl(this)
            {
                Color = Color.Blue,
                Size = new Vector2i(Size.Y, Size.Y),
                Position = new Vector2i(0, 0)
            };
            startMenuButton.Click += (s, e) => ToggleStartMenu(s, EventArgs.Empty);

            var gErc = new ExtensibleRowContainer(this, borderLength);
            gErc.Controls.Add(startMenuButton);

            var erc = new ExtensibleRowContainer(gErc, borderLength);
            //erc.Position = new Vector2i(startMenuButton.Size.X + borderLength * 2, 0);
            var iconsSize = new Vector2i(Size.Y, Size.Y);
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe"));
            erc.Controls.Add(new Executable(erc, @"c:\windows\system32\cmd.exe") { Size = iconsSize }); // TODO Do an auto size prop
            erc.Controls.Add(new Executable(erc, @"c:\windows\notepad.exe") { Size = iconsSize });

            gErc.Controls.Add(erc);

            Controls.Add(gErc);
        }

        //public override void Load()
        //{

            

        //    base.Load();
        //}

        protected override void OnKeyPressed(KeyEventArgs e)
        {
            base.OnKeyPressed(e);
        }

        protected override void OnClick(MouseButtonEventArgs e)
        {
            base.OnClick(e);
        }

        protected override void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            //if (e.Button != Mouse.Button.Left)
            //    MessageBox("Tu viens de cliquer sur le bouton " + e.Button + " de ta souris.\nFrom " + GetType(),
            //        "SDEE test", MessageBoxIcon.Information); // TEMP

            base.OnMouseButtonPressed(e);
        }

        public event EventHandler ToggleStartMenu;
    }
}
