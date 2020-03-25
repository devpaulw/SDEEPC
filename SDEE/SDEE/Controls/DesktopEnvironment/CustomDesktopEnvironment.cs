using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	class CustomDesktopEnvironment : DesktopEnvironment
	{

		protected override void Load()
		{
			
			base.Load();
		}

		protected override void OnClick(MouseButtonEventArgs e)
		{
			base.OnClick(e);
		}

		protected override void OnControlAdded(Control control)
		{
			base.OnControlAdded(control);
		}

		protected override void OnKeyPressed(KeyEventArgs e)
		{
			base.OnKeyPressed(e);
		}

		protected override void OnMouseButtonPressed(MouseButtonEventArgs e)
		{
			base.OnMouseButtonPressed(e);
		}
	}
}
