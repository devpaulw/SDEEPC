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
			XmlWriterSettings settings = new XmlWriterSettings();

			string directoryPath = Path.Combine(DesktopEnvironmentStorage.ConfigurationDirectory, configurationName);
			Directory.CreateDirectory(directoryPath);
			XmlWriter writer = XmlWriter.Create(Path.Combine(directoryPath, $"de0.xml"), settings);
			string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			WriteToXml(desktopEnvironment);

			writer.Flush();
			writer.Close();

			void WriteToXml(Control control)
			{
				writer.WriteStartElement(control.Type.ToString());

				foreach (var attribute in control.XmlAttributes)
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
