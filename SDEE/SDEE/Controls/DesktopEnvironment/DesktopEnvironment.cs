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

namespace SDEE
{
    public class DesktopEnvironment : Control
    {
		protected override Shape Shape => null;
		public override ControlType Type => ControlType.DesktopEnvironment;

		public DesktopEnvironment() : base(null)
        {
		}

        public void Start()
        {
            using (var window = new SfW32DEWindow())
            {
                Position = window.Position;
                Size = (Vector2i)window.Size;

				Load(); // Init control events

                window.Closed += (s, e) => window.Close();
                window.KeyPressed += (s, e) => OnKeyPressed(e);
                window.MouseButtonPressed += (s, e) => OnMouseButtonPressed(e);
                // ... TO ADD Needed EventHandlers

				while (window.IsOpen) // MAIN LOOP
				{
					window.DispatchSystemMessage();
					window.DispatchEvents();
					window.Clear();
					window.Draw(this);
					window.Display();
				}
			}
		}
	}
}
