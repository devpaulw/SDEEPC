using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    /// <summary>
    /// The drawable that makes the ability to draw a control and its children
    /// </summary>
    public class ControlDrawing : Drawable
    {
        private Shape shape;

        public ControlDrawing(Control ctrl, Shape shape)
        {
            Owner = ctrl;
            Shape = shape;
        }

        public Control Owner { get; }

        public Shape Shape {
            get => shape;
            set {
                shape = value;
                shape.Position = DrawPosition;
                if (shape is RectangleShape rs)
                    rs.Size = (Vector2f)Owner.Size;
            }
        }

        private Vector2f DrawPosition {
            get {
                Control parent = Owner;

                if (Owner is ZControl)
                    return new Vector2f(0, 0);

                Vector2i sp = default;
                while (parent != null && !(parent is ZControl))
                {
                    sp += parent.Position;
                    parent = parent.Parent;
                }
                return (Vector2f)sp;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (!Owner.IsEnabled) return;

            if (Shape != null)
                target.Draw(Shape);

            foreach (var child in Owner.Controls)
            {
                if (child.Drawing != null)
                    target.Draw(child.Drawing);
                else
                {
                    foreach (var childChild in child.Controls) // Bypass a null ControlDrawing (working recursively)
                        target.Draw(childChild.Drawing);
                }
            }
        }
    }
}
