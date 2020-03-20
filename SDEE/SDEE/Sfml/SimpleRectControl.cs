using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class SimpleRectControl : Control
    {
        public SimpleRectControl(Control parent) : base(parent)
        {
        }

        protected override Shape Shape
            => new RectangleShape(this.GetBasicShape())
            {
                FillColor = Color,
                Texture = Texture
            };


        public Texture Texture { get; set; }

        public Color Color { get; set; }
    }
}
