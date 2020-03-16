using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    class Program
    {
        static void Main(string[] args)
        {
            var de = new DesktopEnvironment(new Color(0, 0x80, 0b10000000));

            MyTaskbar myTaskbar = new MyTaskbar(0.05f, new Color(0xC0, 0xC0, 0xC0));
            de.GuiElements.Add(myTaskbar);

            de.Start();
        }
    }
}
