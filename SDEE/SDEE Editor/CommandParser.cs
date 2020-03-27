using SDEE;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE_Editor
{
	class CommandParser
	{
		public CustomDesktopEnvironment DesktopEnvironment { get; private set; }
		private static uint nextId = 0;

		public CommandParser(CustomDesktopEnvironment de)
		{
			DesktopEnvironment = de;

		}

		public void Parse(string line)
		{
			bool commandSucceeded = false;
			string[] command = line.Split(' ');

			if (command[0] == CommandSyntax.Commands.Create)
			{
				if (command[1] == CommandSyntax.Controls.SimpleRect)
				{
					DesktopEnvironment.Controls.Add(new SimpleRectControl(DesktopEnvironment, nextId++) { Color = Color.Blue });
					Console.WriteLine("simple_rect created !");
					commandSucceeded = true;


				}
			}

			if (!commandSucceeded)
			{
				Console.WriteLine("Command not valid.");
			}
		}
	}
}
