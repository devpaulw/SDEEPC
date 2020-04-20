using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	public class KeyboardShortcutCollection
	{
		private readonly Dictionary<KeyCombination, DesktopEnvironmentCommand> keyboardShortcuts = new Dictionary<KeyCombination, DesktopEnvironmentCommand>();
		public CustomDesktopEnvironment DesktopEnvironment { get; set; }

		public KeyboardShortcutCollection()
		{
		}

		public DesktopEnvironmentCommand GetCommand(KeyCombination combination)
		{
			if (!keyboardShortcuts.ContainsKey(combination))
				return null;

			return keyboardShortcuts[combination];
		}
	}
}
