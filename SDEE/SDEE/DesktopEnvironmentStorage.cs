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

		public static CustomDesktopEnvironment CreateDefaultConfiguration(string name)
		{
			var de = new CustomDesktopEnvironment(new KeyboardShortcutCollection());
			Directory.CreateDirectory(DesktopEnvironmentDirectory);
			return de;
		}

		public static CustomDesktopEnvironment LoadConfiguration(string name)
		{
			string filePath = GetDesktopEnvironmentFilePath(name);
			var de = new CustomDesktopEnvironment(new KeyboardShortcutCollection());

			XmlReaderSettings settings = new XmlReaderSettings();
			XmlReader reader = XmlReader.Create(filePath, settings);

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
								Control.Load(ReadSimpleRect());
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
				int x = int.Parse(reader.GetAttribute($"{nameof(control.Size)}{nameof(de.Size.X)}"));
				int y = int.Parse(reader.GetAttribute($"{nameof(control.Size)}{nameof(de.Size.Y)}"));
				control.Size = new Vector2i(x, y);

				if (x == ControlLayout.ScreenSize)
					control.FillParentWidth();
				if (y == ControlLayout.ScreenSize)
					control.FillParentHeight();

			}

			void ReadPosition(Control control)
			{
				int x = int.Parse(reader.GetAttribute($"{nameof(control.Position)}{nameof(de.Position.X)}"));
				int y = int.Parse(reader.GetAttribute($"{nameof(control.Position)}{nameof(de.Position.Y)}"));
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
