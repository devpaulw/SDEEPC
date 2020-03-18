using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class DesktopEnvironment : GraphicControl
    {
        public Color Wallpaper { get; set; }
        public new RenderTarget RenderTarget { get; private set; }

        public DesktopEnvironment(Color wallpaper)
        {
            Wallpaper = wallpaper;
        }

        public void Start()
        {
            using (var window = new SfW32DEWindow())
            {
                RenderTarget = window;
                InitEvents(); // Init control events

                window.Closed += (s, e) => window.Close();
                window.KeyPressed += KeyPressed;
                window.MouseButtonPressed += MouseButtonPressed;

                while (window.IsOpen) // MAIN LOOP
                {
                    window.DispatchSystemMessage();
                    window.DispatchEvents();
                    window.Clear(Wallpaper);
                    window.Draw(this);
                    window.Display();
                }
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            // Draw every elements on the DE
            foreach (var child in Controls)
            {
                if (child is GraphicControl graphicChild)
                    target.Draw(graphicChild);
            }
        }

        internal event EventHandler<KeyEventArgs> KeyPressed;
        internal event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
        // ... TO ADD Needed EventHandlers
    }
}
