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
        private readonly Texture iconTexture;

        public string ExecutablePath { get; set; }
        public Taskbar Taskbar => Parent as Taskbar;

        protected override Shape Shape 
            => new RectangleShape()
            {
                Position = (Vector2f)Position,
                Size = (Vector2f)Size,
                Texture = iconTexture
            };

        public TaskbarExecutableElement(string executablePath)
        {
            ExecutablePath = executablePath;
            iconTexture = new Texture(ExtractAssociatedIcon(ExecutablePath));
        }

        protected override void Init()
        {
            #region Position and Size
            int freeXpos = (from control in Taskbar.GetGraphicControls()
                            orderby control.Position.X descending
                            select control.Position.X + control.Size.X)
                            .FirstOrDefault();

            Position = new Vector2i(freeXpos, Taskbar.Position.Y);
            Size = new Vector2i(Taskbar.Size.Y, Taskbar.Size.Y);
            #endregion

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
