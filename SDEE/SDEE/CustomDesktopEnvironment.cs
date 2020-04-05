using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDEE
{
	public class CustomDesktopEnvironment : DesktopEnvironment
	{
		public string ConfigurationName { get; private set; }
		public Color BackgroundColor { get; set; } = Color.Black;
		public string ConfigurationDirectory { get => Path.Combine(DesktopEnvironmentStorage.ConfigurationDirectory, ConfigurationName); }
		public string FilePath { get => Path.Combine(ConfigurationDirectory, "de0.xml"); }
		public override Dictionary<string, string> Attributes => new Dictionary<string, string>()
			{
				{ nameof(BackgroundColor), BackgroundColor.ToHex() }
			};
		private readonly KeyboardShortcutCollection _keyboardShortcuts;

		private protected override Shape Shape => new RectangleShape()
		{
			FillColor = BackgroundColor
		};

		protected CustomDesktopEnvironment(string configurationName)
		{
			ConfigurationName = configurationName;
			_keyboardShortcuts = new KeyboardShortcutCollection(this);
		}

		protected override void OnKeyPressed(KeyEventArgs e)
		{
			DesktopEnvironmentCommand command = _keyboardShortcuts.GetCommand(KeyCombinationFactory.FromKeyEventArgs(e));
			command?.Execute();
			base.OnKeyPressed(e);
		}

		public static CustomDesktopEnvironment CreateDefaultConfiguration(string configurationName)
		{
			var de = new CustomDesktopEnvironment(configurationName);
			Directory.CreateDirectory(de.ConfigurationDirectory);
			return de;
		}

		public static CustomDesktopEnvironment LoadConfiguration(string configurationName)
		{
			var de = new CustomDesktopEnvironment(configurationName);

			XmlReaderSettings settings = new XmlReaderSettings();
			XmlReader reader = XmlReader.Create(de.FilePath, settings);

			try
			{
				ReadDesktopEnvironment();

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (!Enum.TryParse(reader.Name, out ControlType type))
							throw new FileLoadException(reader.BaseURI);

						switch (type)
						{
							case ControlType.SimpleRect:
								Load(ReadSimpleRect());
								break;
							default:
								throw new FileLoadException(reader.BaseURI);
						}
					}
				}
			}
			catch (XmlException)
			{
				throw new FileLoadException(reader.BaseURI);
			}
			finally
			{
				reader.Close();
			}

			return de;

			void ReadDesktopEnvironment()
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (!Enum.TryParse(reader.Name, out ControlType type) || type != ControlType.DesktopEnvironment)
							throw new FileLoadException(reader.BaseURI);

						// BBHERE: initialize the desktopEnvironments attributes from XML
						return;
					}
				}

				throw new FileLoadException(reader.BaseURI);
			}

			SimpleRectControl ReadSimpleRect()
			{
				Color color = GetColorOrDefault(reader.GetAttribute($"{nameof(SimpleRectControl.Color)}"));
				var control = new SimpleRectControl(de, color, id: ReadId());

				ReadPosition(control);
				ReadSize(control);
				return control;
			}

			void ReadSize(Control control)
			{
				int x = int.Parse(reader.GetAttribute($"{nameof(Size)}{nameof(de.Size.X)}"));
				int y = int.Parse(reader.GetAttribute($"{nameof(Size)}{nameof(de.Size.Y)}"));
				control.Size = new Vector2i(x, y);

				if (x == ControlLayout.ScreenSize)
					control.FillParentWidth();
				if (y == ControlLayout.ScreenSize)
					control.FillParentHeight();

			}

			void ReadPosition(Control control)
			{
				int x = int.Parse(reader.GetAttribute($"{nameof(Position)}{nameof(de.Position.X)}"));
				int y = int.Parse(reader.GetAttribute($"{nameof(Position)}{nameof(de.Position.Y)}"));
				control.Position = new Vector2i(Math.Abs(x), Math.Abs(y));

				if (ControlLayout.IsStickedToRightOrBottom(x))
					control.StickToRight();
				if (ControlLayout.IsStickedToRightOrBottom(y))
					control.StickToBottom();
			}

			uint ReadId()
			{
				string idAttribute = reader.GetAttribute($"{nameof(Control.Id)}");

				if (string.IsNullOrEmpty(idAttribute))
					return 0;

				return uint.Parse(idAttribute);
			}

			Color GetColorOrDefault(string attribute)
			{
				try
				{
					return string.IsNullOrEmpty(attribute) ? Color.Black : ColorConverter.ParseHex(attribute);
				}
				catch (NotHexColorException)
				{
					return Color.Black;
				}
			}
		}
	}
}
