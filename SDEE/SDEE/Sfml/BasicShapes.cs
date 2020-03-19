using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    static class BasicShapes
    {
        public static RectangleShape GetBasicShape(this Control graphicControl)
        {
            return new RectangleShape()
            {
                Position = (Vector2f)graphicControl.Position,
                Size = (Vector2f)graphicControl.Size
            };
        }
    }
}
