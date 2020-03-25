using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SDEE;

namespace SDEE_Editor
{
	static class DesktopEnvironmentSaver
	{
		public static void Save(string configurationName, EditableDesktopEnvironment desktopEnvironment)
		{
			XmlWriterSettings settings = new XmlWriterSettings();

			Directory.CreateDirectory(DesktopEnvironmentLoader.FilePath);
			XmlWriter writer = XmlWriter.Create(Path.Combine(DesktopEnvironmentLoader.FilePath, $"{configurationName}.xml"), settings);

			WriteToXml(desktopEnvironment);

			writer.Flush();
			writer.Close();

			void WriteToXml(Control control)
			{
				writer.WriteStartElement(control.GetType().Name);

				foreach (var attribute in control.GetXmlAttributes())
				{
					writer.WriteAttributeString(attribute.Key, attribute.Value);
				}

				foreach (var child in control.Controls)
				{
					WriteToXml(child);
				}
				writer.WriteEndElement();
			}
		}
	}
}
