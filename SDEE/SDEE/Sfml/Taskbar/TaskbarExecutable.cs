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
    class TaskbarExecutable : Control
    {
        private readonly Texture iconTexture;

        public MyTaskbar Taskbar => Parent as MyTaskbar;
        public string ExecutablePath { get; set; }

        protected override Shape Shape
            => new RectangleShape(this.GetBasicShape())
            {
                Texture = iconTexture
            };

        public TaskbarExecutable(string executablePath)
        {
            ExecutablePath = executablePath;
            iconTexture = new Texture(ExtractAssociatedIcon(ExecutablePath));
        }

        protected override void Init()
        {
            if (!(Parent is MyTaskbar))
                throw new Exception("This taskbar executable should be used only on taskbars"); // DOLATER Create managed exceptions

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

        private static Image ExtractAssociatedIcon(string executablePath)
        {
            var icon = System.Drawing.Icon.ExtractAssociatedIcon(executablePath);
            var image = new Image((uint)icon.Width, (uint)icon.Height);

            for (int x = 0; x < icon.Width; x++)
            {
                for (int y = 0; y < icon.Height; y++)
                {
                    System.Drawing.Color sdColor = icon.ToBitmap().GetPixel(x, y);
                    image.SetPixel((uint)x, (uint)y, new Color(sdColor.R, sdColor.G, sdColor.B, sdColor.A));
                }
            }

            return image;
        }
    }
}
