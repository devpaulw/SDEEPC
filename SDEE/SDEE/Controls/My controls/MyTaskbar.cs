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
    // TEMP : public to test some things
    // BBTODO: create a true taskbar type
    public class MyTaskbar : Control
    {
        public Color Color { get; set; }

        protected override Shape Shape {
            get => new RectangleShape(this.GetBasicShape())
            {
                FillColor = Color
            };
        }

        public override ControlType Type => ControlType.Taskbar;
        public override Dictionary<string, string> XmlAttributes
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {  $"{nameof(Size)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Size.X)}", $"{Size.X}" },
                    {  $"{nameof(Size)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Size.Y)}", $"{Size.Y}" },
                    {  $"{nameof(Color)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Color.R)}", $"{Color.R}" },
                    {  $"{nameof(Color)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Color.G)}", $"{Color.G}" },
                    {  $"{nameof(Color)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Color.B)}", $"{Color.B}" },
                    {  $"{nameof(Position)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Position.X)}", $"{Position.X}" },
                    {  $"{nameof(Position)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Position.Y)}", $"{Position.Y}" },
                };
            }
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
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\EXCEL.EXE"));
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\WINWORD.EXE"));
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\OUTLOOK.EXE"));
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Microsoft Office\root\Office16\POWERPNT.EXE"));
            //myTaskbar.Controls.Add(new TaskbarExecutable(myTaskbar, @"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe"));
            erc.Controls.Add(new TaskbarExecutable(this, @"c:\windows\system32\cmd.exe"));
            erc.Controls.Add(new TaskbarExecutable(this, @"c:\windows\notepad.exe"));

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
