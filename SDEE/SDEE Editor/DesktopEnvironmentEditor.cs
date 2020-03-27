using SFML.Graphics;
using System;
using SDEE;

namespace SDEE_Editor
{
	class DesktopEnvironmentEditor
	{
		public void ReadCommands()
		{
			string configurationName = "config1";
            //Console.WriteLine("configuration name: "), configurationName = Console.ReadLine();  // UNCOMMENT_TO: ask for configuration name

			var parser = new CommandParser(new EditableDesktopEnvironment(configurationName));
			Console.WriteLine($"[Configuration: {configurationName}]");

			while (true)
			{
				string line = Console.ReadLine().ToLower();

				if (line.Length == 0)
					break;

				parser.Parse(line);
			}

			DesktopEnvironmentSaver.Save(configurationName, parser.DesktopEnvironment);
		}
	}
}