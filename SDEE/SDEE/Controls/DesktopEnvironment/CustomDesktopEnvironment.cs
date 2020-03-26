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
		public readonly static char AttributeSeparator = '-';
		public string ConfigurationName { get; private set; }

		public CustomDesktopEnvironment(string configurationName)
		{
			ConfigurationName = configurationName;
		}

		protected override Shape Shape => new RectangleShape(this.GetBasicShape())
		{
			FillColor = new Color(0, 0x80, 0b10000000)
		};

		protected override void Load()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			XmlReader reader = XmlReader.Create(Path.Combine(DesktopEnvironmentXml.ConfigurationDirectory, $"{ConfigurationName}.xml"), settings);

			ReadDesktopEnvironment(reader);

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (!Enum.TryParse(reader.Name, out ControlType type))
						throw new FileLoadException(Path.Combine(DesktopEnvironmentXml.ConfigurationDirectory, $"{ConfigurationName}.xml"));

					switch (type)
					{
						case ControlType.Taskbar:
							Controls.Add(ReadTaskbar(reader, this));
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

					// BBNEXT: initialize the desktopEnvironments attributes from XML
					return;
				}
			}

			throw new NoDEImplementedException();
		}

		private static MyTaskbar ReadTaskbar(XmlReader reader, DesktopEnvironment de)
		{
			Color color = new Color(
				byte.Parse(reader.GetAttribute($"{nameof(MyTaskbar.Color)}{DesktopEnvironmentXml.AttributeSeparator}{nameof(MyTaskbar.Color.R)}")),
				byte.Parse(reader.GetAttribute($"{nameof(MyTaskbar.Color)}{DesktopEnvironmentXml.AttributeSeparator}{nameof(MyTaskbar.Color.G)}")),
				byte.Parse(reader.GetAttribute($"{nameof(MyTaskbar.Color)}{DesktopEnvironmentXml.AttributeSeparator}{nameof(MyTaskbar.Color.B)}")));

			return new MyTaskbar(de, color);
		}
	}
}
