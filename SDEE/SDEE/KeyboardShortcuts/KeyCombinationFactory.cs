using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace SDEE
{
	static class KeyCombinationFactory
	{
		// WILLBE: moved in the class that creates the shortcut files
		public const char KeySeparator = '+';
		public const string Control = "ctrl";
		public const string Alt = "alt";
		public const string Shift = "shift";
		public const string System = "super";

		public static KeyCombination ParseString(string keyCombinationString)
		{
			var keyCombination = new KeyCombination();
			string[] keyStrings = keyCombinationString.Split(KeySeparator);
			var keys = new List<Keyboard.Key>();

			foreach (var keyString in keyStrings)
			{
				switch (keyString)
				{
					case Control:
						keyCombination.Control = true;
						break;
					case Shift:
						keyCombination.Shift = true;
						break;
					case System:
						keyCombination.System = true;
						break;
					case Alt:
						keyCombination.Alt = true;
						break;
					default:
						Keyboard.Key key;

						if (!Enum.TryParse(keyString, ignoreCase: true, out key))
							throw new FormatException(nameof(keyCombinationString));

						keys.Add(key);
						break;
				}
			}

			keyCombination.Keys = keys;
			return keyCombination;
		}

		public static KeyCombination FromKeyEventArgs(KeyEventArgs keyEventArgs)
		{
			return new KeyCombination
			{
				Alt = keyEventArgs.Alt,
				Control = keyEventArgs.Control,
				Shift = keyEventArgs.Shift,
				System = keyEventArgs.System,
				Keys = new Keyboard.Key[] { keyEventArgs.Code }
			};
		}
	}
}
