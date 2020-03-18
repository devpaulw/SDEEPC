using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class Taskbar : GraphicControl
    {
        public float Height { get; set; }

        public Taskbar(float height)
        {
            Height = height;
        }
    }
}
