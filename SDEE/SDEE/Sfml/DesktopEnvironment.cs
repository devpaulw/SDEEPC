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
		private KeyboardShortcutComponent keyboardShortcuts;

		public DesktopEnvironment(Color wallpaper) : base(null)
		{
			Wallpaper = wallpaper;
			keyboardShortcuts = new KeyboardShortcutComponent();
		}

		public void Start()
		{
			using (var window = new SfW32DEWindow())
			{
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

		protected override void OnKeyPressed(KeyEventArgs e)
		{
			DesktopEnvironmentCommand command = keyboardShortcuts.GetCommand(KeyCombinationFactory.FromKeyEventArgs(e));
			if (command is null)
				return;
			if (command is ExecuteProgramCommand concreteCommand)
				StartExe(concreteCommand.ExecutablePath);
			base.OnKeyPressed(e);
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
