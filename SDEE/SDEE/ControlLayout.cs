using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	public static class ControlLayout
	{
		public const int ScreenSize = 0;
		public static int StickToRightOrBottom(int position)
			=> -position;
		public static bool IsStickedToRightOrBottom(int i)
			=> i < 0;

		public static void FillParentWidth(this Control control)
		{
			control.Size = new Vector2i(control.DeskEnv.Size.X, control.Size.Y);
		}

		public static void FillParentHeight(this Control control)
		{
			control.Size = new Vector2i(control.Size.X, control.Parent.Size.Y);
		}

		public static void StickToBottom(this Control control)
		{
			if ((control.StickedBorders & StickedBorders.Bottom) != StickedBorders.Bottom)
			{
				control.Position = new Vector2i(control.Position.X, control.Parent.Size.Y - control.Position.Y);
				control.StickedBorders |= StickedBorders.Bottom;
			}
		}

		public static void StickToTop(this Control control)
		{
			if ((control.StickedBorders & StickedBorders.Top) != StickedBorders.Top)
			{
				control.Position = new Vector2i(control.Position.X, control.Position.Y - control.Parent.Size.Y);
				control.StickedBorders |= (StickedBorders.Top & StickedBorders.Right);
			}
		}

		public static void StickToLeft(this Control control)
		{
			if ((control.StickedBorders & StickedBorders.Left) != StickedBorders.Left)
			{
				control.Position = new Vector2i(control.Position.X - control.Parent.Size.X, control.Position.Y);
				control.StickedBorders |= (StickedBorders.Left & StickedBorders.Bottom);
			}
		}

		public static void StickToRight(this Control control)
		{
			if ((control.StickedBorders & StickedBorders.Right) != StickedBorders.Right)
			{
				control.Position = new Vector2i(control.Position.X - control.Parent.Size.X, control.Position.Y);
				control.StickedBorders |= StickedBorders.Right;
			}
		}
	}
}
