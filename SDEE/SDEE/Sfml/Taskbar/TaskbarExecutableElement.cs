using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class TaskbarExecutableElement : GraphicControl
    {
        public string ExecutablePath { get; set; }
        public Color Color { get; set; }
        public Taskbar Taskbar => Parent as Taskbar;

        public override Shape Shape 
            => new RectangleShape()
            {
                Position = (Vector2f)Position,
                Size = (Vector2f)Size,
                FillColor = Color
            };

        public TaskbarExecutableElement(string executablePath, Color color)
        {
            ExecutablePath = executablePath;
            Color = color;
        }

        protected override void Init()
        {
            int freeXpos = (from control in Taskbar.GetGraphicControls()
                            orderby control.Position.X descending
                            select control.Position.X + control.Size.X)
                            .FirstOrDefault();

            Position = new Vector2i(freeXpos, Taskbar.Position.Y);
            Size = new Vector2i(Taskbar.Size.Y, Taskbar.Size.Y);

            base.Init();
        }

        protected override void OnClick(MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                StartExe(ExecutablePath);
            }

            base.OnClick(e);
        }
    }
}
