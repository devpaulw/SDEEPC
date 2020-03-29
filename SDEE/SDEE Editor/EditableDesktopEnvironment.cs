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
	class EditableDesktopEnvironment : CustomDesktopEnvironment
	{
		protected EditableDesktopEnvironment(string configurationName) : base(configurationName)
		{
		}

		public static EditableDesktopEnvironment CreateDefaultConfiguration(string configurationName)
		{
			var de = new EditableDesktopEnvironment(configurationName);
			Directory.CreateDirectory(de.ConfigurationDirectory);
			return de;
		}
		public static EditableDesktopEnvironment LoadConfiguration(string configurationName)
		{
			EditableDesktopEnvironment de = (EditableDesktopEnvironment)CustomDesktopEnvironment.LoadConfiguration(configurationName);

			return de;
		}

		public bool TryGetControl(ControlReference reference, out Control control)
		{
			Control controlFound = null;
			SearchControl(this);
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

		public bool TryDelete(ControlReference id)
		{
			bool deleted = false;
			SearchControl(this);
			return deleted;

			void SearchControl(Control control)
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

						SearchControl(child);
					}
				}
			}


		}
	}
}
