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
        private protected override Shape Shape => new RectangleShape()
        {
            FillColor = new Color(120, 120, 120)
        };

        public MyStartMenu(DesktopEnvironment parent) : base(parent: parent, size: new Vector2i(300, 400)) { }
    }
}
