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
    class SimpleRectControl : Control
    {
        public SimpleRectControl(Control parent) : base(parent)
        {
        }

        public override ControlDrawing Drawing => new ControlDrawing(this, new RectangleShape()
        {
            FillColor = Color,
            Texture = Texture
        });

        protected override void OnClick(MouseButtonEventArgs e)
        {
            base.OnClick(e);
        }

        public Texture Texture { get; set; }

        public Color Color { get; set; }
    }
}
