﻿using System;
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
		public static void Save(string configurationName, DesktopEnvironment desktopEnvironment)
		{
			const string fileName = "de0.xml";

			string directoryPath = Path.Combine(DesktopEnvironmentStorage.ConfigurationDirectory, configurationName);
			Directory.CreateDirectory(directoryPath);

			XmlWriterSettings settings = new XmlWriterSettings();
			XmlWriter writer = XmlWriter.Create(Path.Combine(directoryPath, fileName), settings);

			WriteToXml(desktopEnvironment);

			writer.Flush();
			writer.Close();

			void WriteToXml(Control control)
			{
				writer.WriteStartElement(control.Type.ToString());
				writer.WriteAttributeString(nameof(control.Id), $"{control.Id}");

				foreach (var attribute in control.GetXmlAttributes())
					writer.WriteAttributeString(attribute.Key, attribute.Value);

				foreach (var child in control.Controls)
					WriteToXml(child);

				writer.WriteEndElement();
			}
		}
	}
}