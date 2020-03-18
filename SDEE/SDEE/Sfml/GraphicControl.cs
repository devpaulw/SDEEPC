using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE.Sfml
{
    abstract class GraphicControl : Control, Drawable
    {
        public RenderTarget RenderTarget 
            => DesktopEnvironment.RenderTarget;

        public GraphicControl()
        {
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var control in Controls)
                if (control is GraphicControl drawableControl)
                    target.Draw(drawableControl);
        }
    }
}
