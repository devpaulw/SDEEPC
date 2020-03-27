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
		public EditableDesktopEnvironment DesktopEnvironment { get; private set; }
		private static uint nextId = 0;

		public CommandParser(EditableDesktopEnvironment de)
		{
			DesktopEnvironment = de;

		}

		public void Parse(string line)
		{
			string output = $"The command didn't succeeded.";
			string[] command = line.Split(' ');

			if (command[0] == CommandSyntax.Commands.Append)
			{
				if (command[1] == CommandSyntax.Controls.SimpleRect)
				{
					var control = new SimpleRectControl(DesktopEnvironment, nextId++)
					{
						Color = Color.Blue
					};
					DesktopEnvironment.Controls.Add(control);
					output = $"Created {ControlType.SimpleRect} (Id = {control.Id})";
				}
			}
			else if (command[0] == CommandSyntax.Commands.Delete)
			{
				if (command[1] == CommandSyntax.Controls.SimpleRect)
				{
					try
					{
						uint id = uint.Parse(command[2]);

						if (DesktopEnvironment.RemoveControl(ControlType.SimpleRect, id))
						{
							output = $"Deleted {ControlType.SimpleRect} (Id = {command[2]})";
						}
						else
						{
							output = $"{ControlType.SimpleRect} (Id = {command[2]}) doesn't exist";
						}
					}
					catch (Exception)
					{
						output = $"The third argument must be an integer.";
					}

				}
			}

			Console.WriteLine(output);
		}
	}
}
