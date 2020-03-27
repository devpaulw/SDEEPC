using SFML.Graphics;
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
		public string CurrentConfigurationDirectory { get => Path.Combine(DesktopEnvironmentStorage.ConfigurationDirectory, ConfigurationName); }
		private readonly KeyboardShortcutCollection keyboardShortcuts;

		public CustomDesktopEnvironment(string configurationName)
		{
			ConfigurationName = configurationName;
			keyboardShortcuts = new KeyboardShortcutCollection(this);
		}

		protected override Shape Shape => new RectangleShape(this.GetBasicShape())
		{
			FillColor = new Color(0, 0x80, 0b10000000)
		};

		protected override void Load()
		{
			string path = Path.Combine(DesktopEnvironmentStorage.ConfigurationDirectory, ConfigurationName, "de0.xml");
			XmlReaderSettings settings = new XmlReaderSettings();
			XmlReader reader = XmlReader.Create(path, settings);

			ReadDesktopEnvironment(reader);

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (!Enum.TryParse(reader.Name, out ControlType type))
						throw new FileLoadException(reader.BaseURI);

					switch (type)
					{
						case ControlType.SimpleRect:
							Controls.Add(ReadSimpleRect(reader, this));
							break;
						default:
							throw new NotSupportedException();
					}
				}
			}
		}

		private void ReadDesktopEnvironment(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (!Enum.TryParse(reader.Name, out ControlType type) || type != ControlType.DesktopEnvironment)
						throw new NoDEImplementedException();

					// BBHERE: initialize the desktopEnvironments attributes from XML
					return;
				}
			}

			throw new NoDEImplementedException();
		}

		private static SimpleRectControl ReadSimpleRect(XmlReader reader, DesktopEnvironment de)
		{
			Color color = new Color(
				byte.Parse(reader.GetAttribute($"{nameof(SimpleRectControl.Color)}{nameof(SimpleRectControl.Color.R)}")),
				byte.Parse(reader.GetAttribute($"{nameof(SimpleRectControl.Color)}{nameof(SimpleRectControl.Color.G)}")),
				byte.Parse(reader.GetAttribute($"{nameof(SimpleRectControl.Color)}{nameof(SimpleRectControl.Color.B)}")));

			return new SimpleRectControl(de) { Color = color };
		}

		protected override void OnKeyPressed(KeyEventArgs e)
		{
			DesktopEnvironmentCommand command = keyboardShortcuts.GetCommand(KeyCombinationFactory.FromKeyEventArgs(e));
			command?.Execute();
			base.OnKeyPressed(e);
		}
	}
}
