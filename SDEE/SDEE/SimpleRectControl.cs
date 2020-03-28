using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    public class SimpleRectControl : Control
    {
        private protected override Shape Shape {
            get {
                var rs = new RectangleShape();
                if (Texture == null)
                    rs.FillColor = Color;
                else
                    rs.Texture = Texture;
                return rs;
            }
        }

        public SimpleRectControl(Control parent, Texture texture, Vector2i position = default, Vector2i size = default) : base(parent, position, size)
        {
            Texture = texture;
        }

        public SimpleRectControl(Control parent, Color color, Vector2i position = default, Vector2i size = default) : base(parent, position, size)
        {
            Color = color;
        }

        protected override void OnClick(MouseButtonEventArgs e)
        {
            base.OnClick(e);
        }

        public Texture Texture { get; }

        public Color Color { get; }
    }
}
