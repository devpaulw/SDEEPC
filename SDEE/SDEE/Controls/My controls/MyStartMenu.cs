﻿
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    class MyStartMenu : Control
    {
        protected override Shape Shape
            => new RectangleShape(this.GetBasicShape())
            {
                FillColor = new Color(120, 120, 120)
            };

        public MyStartMenu(DesktopEnvironment parent) : base(parent)
        {
            Position = new Vector2i(0, parent.Size.Y - 400);
            Size = new Vector2i(300, 400);
        }


    }
}