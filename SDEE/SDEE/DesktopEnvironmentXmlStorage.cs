using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ControlNo = System.UInt32;

namespace SDEE
{
	public static class DesktopEnvironmentXmlStorage
	{
		public static CustomDesktopEnvironment ParseFile(string filePath)
		{
			CustomDesktopEnvironment desktopEnvironment;
			XmlReaderSettings settings = new XmlReaderSettings();
			XmlReader reader = XmlReader.Create(filePath, settings);

			try
			{
				desktopEnvironment = ReadDesktopEnvironment();

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

			return desktopEnvironment;


			void ReadSizeAttribute(Control control)
			{
				int x = int.Parse(reader.GetAttribute($"{nameof(control.Size)}{nameof(control.Size.X)}"));
				int y = int.Parse(reader.GetAttribute($"{nameof(control.Size)}{nameof(control.Size.Y)}"));
				control.Size = new Vector2i(x, y);

				if (x == ControlLayout.ScreenSize)
					control.FillParentWidth();
				if (y == ControlLayout.ScreenSize)
					control.FillParentHeight();
			}

			void ReadPositionAttribute(Control control)
			{
				int x = int.Parse(reader.GetAttribute($"{nameof(control.Position)}{nameof(control.Position.X)}"));
				int y = int.Parse(reader.GetAttribute($"{nameof(control.Position)}{nameof(control.Position.Y)}"));
				control.Position = new Vector2i(Math.Abs(x), Math.Abs(y));

				if (ControlLayout.IsStickedToRightOrBottom(x))
					control.StickToRight();
				if (ControlLayout.IsStickedToRightOrBottom(y))
					control.StickToBottom();
			}

			ControlNo ReadIdAttribute()
			{
				try
				{
					return uint.Parse(reader.GetAttribute($"{nameof(Control.Id)}"));
				}
				catch (Exception)
				{
					return 0;
				}
			}

			CustomDesktopEnvironment ReadDesktopEnvironment()
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (!Enum.TryParse(reader.Name, out ControlType type) || type != ControlType.DesktopEnvironment)
							throw new FileLoadException(reader.BaseURI);

						// BBHERE: initialize the desktopEnvironments attributes from XML
						
						return new CustomDesktopEnvironment(new KeyboardShortcutCollection());
					}
				}

				throw new FileLoadException(reader.BaseURI);
			}

			SimpleRectControl ReadSimpleRect()
			{
				Color color = DesktopEnvironmentStorage.GetColorOrDefault(reader.GetAttribute($"{nameof(SimpleRectControl.Color)}"));
				var control = new SimpleRectControl(desktopEnvironment, color, id: ReadIdAttribute());

				ReadPositionAttribute(control);
				ReadSizeAttribute(control);

				return control;
			}
		}
	}
}
