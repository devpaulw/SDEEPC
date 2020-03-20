using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	class KeyboardShortcutComponent
	{
		private readonly Dictionary<KeyCombination, DesktopEnvironmentCommand> keyboardShortcuts = new Dictionary<KeyCombination, DesktopEnvironmentCommand>();

		public KeyboardShortcutComponent()
		{
			Initialize();
		}

		private void Initialize()
		{
			const string filePath = "keyboard-shortcuts.txt";
			const char dataSeparator = '>';

			string line;
			StreamReader file;

			try
			{
				file = new StreamReader(filePath);
			}
			catch (FileNotFoundException)
			{
				return; // no shortcuts will be managed if the file doesn't exist
			}

			while ((line = file.ReadLine()) != null)
			{
				// ASSUME : there is not the same key combination more than one time
				string[] split = line.Split(new char[] { dataSeparator }, 2);
				keyboardShortcuts.Add(KeyCombinationFactory.ParseString(split[0]), new ExecuteProgramCommand(split[1]));
			}

			file.Close();
		}

		public DesktopEnvironmentCommand GetCommand(KeyCombination combination)
		{
			if (!keyboardShortcuts.ContainsKey(combination))
				return null;

			return keyboardShortcuts[combination];
		}
	}
}
