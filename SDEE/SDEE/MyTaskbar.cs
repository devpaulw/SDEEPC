using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    class MyTaskbar : Drawable
    {
        /// <summary>
        /// The height of the taskbar in percentage between 0.0 and 1.0
        /// </summary>
        public float Height { get; set; }
        public Color Color { get; set; }

        public MyTaskbar(float height, Color color)
        {
            Height = height;
            Color = color;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            RectangleShape rs = new RectangleShape()
            {
                Size = new Vector2f(target.Size.X, target.Size.Y * Height),
                Position = new Vector2f(0, target.Size.Y * (1 - Height)),
                FillColor = Color
            };

            target.Draw(rs);
        }
    }
}
