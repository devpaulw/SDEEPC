
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

        protected override Shape Shape => new RectangleShape()
        {
            FillColor = new Color(120, 120, 120)
        };

        public override bool NoSize => true;

        public override bool NoMove => false;

        public MyStartMenu(DesktopEnvironment parent) : base(parent)
        {
            Size = new Vector2i(300, 400);
        }

        //public override void Load()
        //{

        //    base.Load();
        //}
    }
}
