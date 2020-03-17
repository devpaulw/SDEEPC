using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class TaskbarElement : GraphicControl
    {
        public TaskbarElement(MyTaskbar parentTaskbar) : base(parentTaskbar)
        {

        } 

        public override void Draw(RenderTarget target, RenderStates states)
        {
            // UNDONE

            base.Draw(target, states);
        }
    }
}
