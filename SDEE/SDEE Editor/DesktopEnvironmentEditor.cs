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
			/*
            // UNCOMMENT_TO: ask for configuration name
            Console.WriteLine("configuration name: "), configurationName = Console.ReadLine();
            */

			var parser = new CommandParser(new CustomDesktopEnvironment(configurationName));
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