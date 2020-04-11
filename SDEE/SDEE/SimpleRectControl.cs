using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlNo = System.UInt32;

namespace SDEE
{
	public class SimpleRectControl : Control
	{
		private protected override Shape Shape
		{
			get
			{
				var rs = new RectangleShape();
				if (Texture == null)
					rs.FillColor = Color;
				else
					rs.Texture = Texture;
				return rs;
			}
		}

		public SimpleRectControl(Control parent, Texture texture, Vector2i position = default, Vector2i size = default, ControlNo id = 0) : base(id, parent, position, size)
		{
			Texture = texture;
			Initialize();
		}

		private void Initialize()
		{
			Attributes.Add($"{nameof(Size)}{nameof(Size.X)}", $"{Size.X}");
			Attributes.Add($"{nameof(Size)}{nameof(Size.Y)}", $"{Size.Y}");
			Attributes.Add($"{nameof(Position)}{nameof(Position.X)}", $"{Position.X}");
			Attributes.Add($"{nameof(Position)}{nameof(Position.Y)}", $"{Position.Y}");
		}

		public SimpleRectControl(Control parent, Color color, Vector2i position = default, Vector2i size = default, ControlNo id = 0) : base(id, parent, position, size)
		{
			Attributes.Add($"{nameof(Color)}", $"{color.ToHex()}");
			Initialize();
		}

		protected override void OnClick(MouseButtonEventArgs e)
		{
			base.OnClick(e);
		}

		public Texture Texture { get; }

		public Color Color { get => ColorConverter.ParseHex(Attributes[nameof(Color)]); }

		public override ControlType Type => ControlType.SimpleRect;
	}
}
