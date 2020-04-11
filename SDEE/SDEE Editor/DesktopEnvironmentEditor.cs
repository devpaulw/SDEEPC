using SFML.Graphics;
using System;
using System.IO;
using SDEE;
using System.Text.RegularExpressions;
using SFML.System;

namespace SDEE_Editor
{
	class DesktopEnvironmentEditor
	{
		public CustomDesktopEnvironment DesktopEnvironment { get; private set; }
		public Control FocusedControl { get; set; }
		private IdGenerator _idGenerator;

		public void ReadCommands()
		{
			string deName = "de0";
			// UNCOMMENT_TO: ask for configuration name
			//Console.WriteLine("configuration name: "), configurationName = Console.ReadLine();
			Console.WriteLine($"[Configuration: {deName}]");

			try
			{
				DesktopEnvironment = DesktopEnvironmentStorage.Load(deName);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("The configuration file was not found. A new file will be created.");
				DesktopEnvironment = DesktopEnvironmentStorage.CreateDefault();
			}
			catch (FileLoadException)
			{
				Console.WriteLine("The configuration file is badly formatted. A new file will overwrite it.");
				DesktopEnvironment = DesktopEnvironmentStorage.CreateDefault();
			}

			FocusedControl = DesktopEnvironment;
			_idGenerator = new IdGenerator(DesktopEnvironment);

			while (true)
			{
				Console.Write($"({FocusedControl} focused) >");
				string line = Console.ReadLine();

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
				catch (InvalidCommandException)
				{
					Console.WriteLine("This command doesn't exist.");
				}
			}

			DesktopEnvironmentStorage.Save(deName, DesktopEnvironment);
		}

		private string ExecuteCommand(string line)
		{
			const string duplicateSpacesRegex = @"\s+";
			string[] commandWords = Regex.Replace(line, duplicateSpacesRegex, " ").Split(' ');
			string output = "";

			switch (commandWords[0].ToLower())
			{
				case CommandSyntax.Commands.Set:
					if (!FocusedControl.Attributes.ContainsKey(commandWords[1]))
						throw new FormatException(); // InvalidAttributeException

					FocusedControl.Attributes[commandWords[1]] = commandWords[2];
					break;
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
						output = $"{FocusedControl.Type}\n";
						foreach (var attribute in FocusedControl.Attributes)
						{
							output += $"{attribute}\n";
						}
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
						control = new SimpleRectControl(FocusedControl, Color.Blue, id: _idGenerator.GenerateNextId())
						{
							Position = new Vector2i(ControlLayout.ScreenSize, ControlLayout.StickToRightOrBottom(30)),
							Size = new Vector2i(ControlLayout.ScreenSize, 30)
						};
						break;
					default:
						return false;
				}

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