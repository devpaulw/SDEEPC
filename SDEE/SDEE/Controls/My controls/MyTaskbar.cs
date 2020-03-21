﻿using SFML.Graphics;
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

        protected override Shape Shape {
            get => new RectangleShape(this.GetBasicShape())
            {
                FillColor = Color
            };
        }

        public MyTaskbar(DesktopEnvironment parent, Color color) : base(parent)
        {
            Color = color;

            Position = new Vector2i(0, (parent.Size.Y - 30));
            Size = new Vector2i(parent.Size.X, 30);
        }

        protected override void Load()
        {
            int borderLength = 5;

            var startMenuButton = new SimpleRectControl(this)
            {
                Color = Color.Blue,
                Size = new Vector2i(Size.Y, Size.Y),
                Position = new Vector2i(borderLength, Position.Y)
            };
            startMenuButton.Click += (s, e) => ToggleStartMenu(s, e);
            
            Controls.Add(startMenuButton);

            var erc = new ExtensibleRowContainer(this, borderLength);
            erc.Position = new Vector2i(startMenuButton.Size.X + startMenuButton.Position.X + borderLength, Position.Y); // TODO Put an e.r.c. in a e.r.c. to avoid this ugly

            var iconsSize = new Vector2i(Size.Y, Size.Y);
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe"));
            erc.Controls.Add(new Executable(this, @"c:\windows\system32\cmd.exe") { Size = iconsSize }); // TODO Do an auto size prop
            erc.Controls.Add(new Executable(this, @"c:\windows\notepad.exe") { Size = iconsSize });

            Controls.Add(erc);

            base.Load();
        }

        protected override void OnKeyPressed(KeyEventArgs e)
        {

            base.OnKeyPressed(e);
        }

        protected override void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            if (e.Button != Mouse.Button.Left)
                MessageBox("Tu viens de cliquer sur le bouton " + e.Button + " de ta souris.\nFrom " + GetType(),
                    "SDEE test", MessageBoxIcon.Information); // TEMP

            base.OnMouseButtonPressed(e);
        }

        public event EventHandler ToggleStartMenu;
    }
}
