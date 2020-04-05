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
	static class CustomDesktopEnvironmentExtension
	{

		public static bool TryGetControl(this CustomDesktopEnvironment de, ControlReference reference, out Control control)
		{
			Control controlFound = null;
			SearchControl(de);
			control = controlFound;
			return controlFound != null;

			void SearchControl(Control currentControl)
			{
				if (controlFound is null)
				{
					foreach (var child in currentControl.Controls)
					{
						if (reference.References(child))
						{
							controlFound = child;
							return;
						}
						SearchControl(child);
					}
				}
			}
		}

		public static bool TryDelete(this CustomDesktopEnvironment de, ControlReference id)
		{
			bool deleted = false;
			DeleteControl(de);
			return deleted;

			void DeleteControl(Control control)
			{
				if (!deleted)
				{
					foreach (var child in control.Controls)
					{
						if (id.References(child))
						{
							Control.Remove(child);
							deleted = true;
							return;
						}

						DeleteControl(child);
					}
				}
			}
		}
	}
}
