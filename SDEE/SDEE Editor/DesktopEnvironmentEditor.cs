using SFML.Graphics;
using System;
using System.IO;
using SDEE;
using System.Text.RegularExpressions;
using SFML.System;
using ControlNo = System.UInt32;

namespace SDEE_Editor
{
	class DesktopEnvironmentEditor
	{
		public EditableDesktopEnvironment DesktopEnvironment { get; private set; }
		public Control FocusedControl { get; set; }
		// BBNEXT: generator that checks the max id from the desktopEnvironment
		private static ControlNo nextId = 1;

		public void ReadCommands()
		{
			string configurationName = "config1";
			// UNCOMMENT_TO: ask for configuration name
			//Console.WriteLine("configuration name: "), configurationName = Console.ReadLine();
			Console.WriteLine($"[Configuration: {configurationName}]");

			try
			{
				DesktopEnvironment = EditableDesktopEnvironment.LoadConfiguration(configurationName);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("The configuration file was not found. A new file will be created.");
				DesktopEnvironment = EditableDesktopEnvironment.CreateDefaultConfiguration(configurationName);
			}
			catch (FileLoadException)
			{
				Console.WriteLine("The configuration file is badly formatted. A new file will overwrite it.");
				DesktopEnvironment = EditableDesktopEnvironment.CreateDefaultConfiguration(configurationName);
			}

			FocusedControl = DesktopEnvironment;

			while (true)
			{
				Console.Write($"({FocusedControl} focused) >");
				string line = Console.ReadLine().ToLower();

				if (line.Length == 0)
					break;

				try
				{
					Console.WriteLine(ExecuteCommand(line));
				}
				catch (FormatException)
				{
					Console.WriteLine("Format of the command incorrect.");
				}
				catch (InvalidControlException)
				{
					Console.WriteLine("This control doesn't exist.");
				}
				catch(InvalidCommandException)
				{
					Console.WriteLine("This command doesn't exist.");
				}
			}

			DesktopEnvironmentSaver.Save(configurationName, DesktopEnvironment);
		}

		private string ExecuteCommand(string line)
		{
			const string duplicateSpacesRegex = @"\s+";
			string[] commandWords = Regex.Replace(line, duplicateSpacesRegex, " ").Split(' ');
			string output = "";

			switch (commandWords[0])
			{
				case CommandSyntax.Commands.Tree:
					{
						output = $"{FocusedControl}\n";
						Tree(FocusedControl, 1);

						void Tree(Control control, int indentation)
						{
							foreach (var child in control.Controls)
							{
								output += $"{new string('\t', indentation)}{child}\n";
								Tree(child, indentation + 1);
							}
						}
					}
					break;
				case CommandSyntax.Commands.Append:
					{
						if (!TryGetControl(out Control control))
							throw new FormatException();
						Control.Load(control);
					}
					break;
				case CommandSyntax.Commands.Attributes:
					{
						var attributes = FocusedControl.Attributes.Keys;
						int i = 0;

						output = $"{FocusedControl.Type} {{ ";
						foreach (var attribute in attributes)
						{
							i++;
							output += $"{attribute} {(i != attributes.Count ? ", " : "")}";
						}
						output += " }";
					}
					break;
				case CommandSyntax.Commands.Focus:
					{
						if (!TryGetReference(out ControlReference reference))
							throw new FormatException();
						if (!DesktopEnvironment.TryGetControl(reference, out Control control))
							throw new InvalidControlException();

						FocusedControl = control;
					}
					break;
				case CommandSyntax.Commands.Delete:
					{
						if (!TryGetReference(out ControlReference reference))
							throw new FormatException();

						if (!DesktopEnvironment.TryDelete(reference))
							throw new InvalidControlException();
					}
					break;
				default:
					throw new InvalidCommandException();
			}

			return output;

			bool TryGetControl(out Control control)
			{
				control = null;

				if (!Enum.TryParse(commandWords[1], ignoreCase: true, out ControlType type))
					return false;

				switch (type)
				{
					case ControlType.SimpleRect:
						// BBNEXT
						control = new SimpleRectControl(FocusedControl, Color.Blue, id:nextId)
						{
							Position = new Vector2i(ControlLayout.ScreenSize, ControlLayout.StickToRightOrBottom(30)),
							Size = new Vector2i(ControlLayout.ScreenSize, 30)
						};
						break;
					default:
						return false;
				}

				nextId++;
				return true;
			}

			bool TryGetReference(out ControlReference reference)
			{
				reference = null;
				if (!Enum.TryParse(commandWords[1], ignoreCase: true, out ControlType type))
					return false;

				if (!uint.TryParse(commandWords[2], out uint no))
					return false;

				reference = new ControlReference(type, no);
				return true;
			}
		}

	}
}