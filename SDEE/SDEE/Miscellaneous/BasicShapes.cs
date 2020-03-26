using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    static class BasicShapes
    {
        public static RectangleShape GetBasicShape(this Control ctrl)
        {
            return new RectangleShape()
            {
                Position = (Vector2f)ctrl.AbsolutePosition,
                Size = (Vector2f)ctrl.Size
            };
        }
    }
}
