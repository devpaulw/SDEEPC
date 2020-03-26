using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDEE;
using SFML.Graphics;

namespace SDEE_Editor
{
	class Program
	{
		static void Main(string[] args)
		{
			string configurationName = "config1";

			/*
            // ask for configuration name
            Console.WriteLine("configuration name: ");
            configurationName = Console.ReadLine();
            */
			Console.WriteLine($"[Configuration: {configurationName}]");
			
			var de = new DesktopEnvironment();

			while (true)
			{
				string line = Console.ReadLine().ToLower();

				if (line.Length == 0)
					break;

				string[] command = line.Split(' ');

				if (command[0] == Syntax.Command.Create)
				{
					if (command[1] == Syntax.Control.Taskbar)
					{
						de.Controls.Add(new MyTaskbar(de, Color.Black));
						Console.WriteLine("Taskbar created !");
					}
				}
			}

			DesktopEnvironmentSaver.Save(configurationName, de);
		}

		static class Syntax
		{
			public static class Command
			{
				public const string Create = "create";
				public const string Delete = "delete";
			}
			public static class Control
			{
				public const string Taskbar = "taskbar";
			}
		}
	}
}
