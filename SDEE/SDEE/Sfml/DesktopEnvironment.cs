using SFML.Graphics;
using SFML.System;
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
    class DesktopEnvironment : Control
    {

        public Color Wallpaper { get; set; }

        protected override Shape Shape => null;

        public DesktopEnvironment(Color wallpaper)
        {
            Wallpaper = wallpaper;
        }

        public void Start()
        {
            using (var window = new SfW32DEWindow())
            {
                Position = window.Position;
                Size = (Vector2i)window.Size;

                Init(); // Init control events

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

        internal new event EventHandler<KeyEventArgs> KeyPressed;
        internal new event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
        // ... TO ADD Needed EventHandlers
    }
}
