using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDEE
{
	public static class DesktopEnvironmentStorage
	{
		public static string DesktopEnvironmentDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SDEE");

		public static string GetDesktopEnvironmentFilePath(string name)
			=> Path.Combine(DesktopEnvironmentDirectory, $"{name}.xml");

		public static void Save(string name, CustomDesktopEnvironment desktopEnvironment)
		{
			string filePath = GetDesktopEnvironmentFilePath(name);
			Directory.CreateDirectory(DesktopEnvironmentDirectory);

			XmlWriterSettings settings = new XmlWriterSettings();
			XmlWriter writer = XmlWriter.Create(filePath, settings);

			WriteToXml(desktopEnvironment);

			writer.Flush();
			writer.Close();

			void WriteToXml(Control control)
			{
				writer.WriteStartElement(control.Type.ToString());

				writer.WriteAttributeString(nameof(control.Id), $"{control.Id}");
				foreach (var attribute in control.Attributes)
					writer.WriteAttributeString(attribute.Key, attribute.Value);

				foreach (var child in control.Controls)
					WriteToXml(child);

				writer.WriteEndElement();
			}
		}

		public static CustomDesktopEnvironment CreateDefault()
		{
			var de = new CustomDesktopEnvironment(new KeyboardShortcutCollection());
			Directory.CreateDirectory(DesktopEnvironmentDirectory);
			return de;
		}

		public static CustomDesktopEnvironment Load(string name)
		{
			return DesktopEnvironmentXmlStorage.ParseFile(GetDesktopEnvironmentFilePath(name));
		}

		public static Color GetColorOrDefault(string hex)
		{
			try
			{
				return ColorConverter.ParseHex(hex);
			}
			catch (NotHexColorException)
			{
				return Color.Black;
			}
		}

	}
}
