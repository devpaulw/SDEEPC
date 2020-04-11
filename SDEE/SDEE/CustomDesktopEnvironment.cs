using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDEE
{
	public class CustomDesktopEnvironment : DesktopEnvironment
	{
		public Color BackgroundColor { get; set; } = Color.Black;

		public KeyboardShortcutCollection KeyboardShortcuts { get; private set; }

		private protected override Shape Shape => new RectangleShape()
		{
			FillColor = BackgroundColor
		};

		public CustomDesktopEnvironment(KeyboardShortcutCollection keyboardShortcuts)
		{
			KeyboardShortcuts = keyboardShortcuts;
			KeyboardShortcuts.DesktopEnvironment = this;

			Attributes.Add(nameof(BackgroundColor), BackgroundColor.ToHex());
		}

		protected override void OnKeyPressed(KeyEventArgs e)
		{
			DesktopEnvironmentCommand command = KeyboardShortcuts.GetCommand(KeyCombinationFactory.FromKeyEventArgs(e));
			command?.Execute();
			base.OnKeyPressed(e);
		}

	}
}
