using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	public class SimpleRectControl : Control
	{

		public SimpleRectControl(Control parent, uint id = 0) : base(parent, id)
		{
		}

		protected override Shape Shape
			=> new RectangleShape(this.GetBasicShape())
			{
				FillColor = Color,
				Texture = Texture
			};

		public override Dictionary<string, string> XmlAttributes
		{
			get
			{
				return new Dictionary<string, string>()
				{
					{  $"{nameof(Size)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Size.X)}", $"{Size.X}" },
					{  $"{nameof(Size)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Size.Y)}", $"{Size.Y}" },
					{  $"{nameof(Color)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Color.R)}", $"{Color.R}" },
					{  $"{nameof(Color)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Color.G)}", $"{Color.G}" },
					{  $"{nameof(Color)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Color.B)}", $"{Color.B}" },
					{  $"{nameof(Position)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Position.X)}", $"{Position.X}" },
					{  $"{nameof(Position)}{DesktopEnvironmentStorage.XmlAttributeSeparator}{nameof(Position.Y)}", $"{Position.Y}" },
				};
			}
		}

		public Texture Texture { get; set; }

		public Color Color { get; set; }

		public override ControlType Type => ControlType.SimpleRect;
	}
}
