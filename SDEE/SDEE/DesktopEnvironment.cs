using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE
{
    abstract class DesktopEnvironment<TElement>
    {
        public Collection<TElement> GuiElements { get; } = new Collection<TElement>();

        public DesktopEnvironment()
        {
        }

        public abstract void Start();
    }
}