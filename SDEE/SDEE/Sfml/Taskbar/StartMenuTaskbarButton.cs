using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml.Taskbar
{
    class StartMenuTaskbarButton : Control
    {
        protected override Shape Shape => throw new NotImplementedException();

        protected override void Init()
        {
            Size = new Vector2i(1, 1);

            base.Init();
        }
    }
}
