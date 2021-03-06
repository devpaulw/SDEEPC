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
    public sealed class Executable : SimpleRectControl
    {
        public string ExecutablePath { get; set; }

        public Executable(Control parent, string executablePath) : base(parent, 
            texture: new Texture(ExtractAssociatedIcon(executablePath)))
        {
            ExecutablePath = executablePath;
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
