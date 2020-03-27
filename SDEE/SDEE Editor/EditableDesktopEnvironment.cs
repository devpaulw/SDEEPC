using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDEE;

namespace SDEE_Editor
{
	class EditableDesktopEnvironment : CustomDesktopEnvironment
	{
		public EditableDesktopEnvironment(string configurationName) : base(configurationName)
		{
		}

		public bool RemoveControl(ControlType type, uint id)
		{
			bool found = false;
			SearchControl(this);
			return found;

			void SearchControl(Control control)
			{
				if (!found)
				{
					for (int i = 0; i < control.Controls.Count; i++)
					{
						if (control.Controls[i].Id == id && control.Controls[i].Type == type)
						{
							control.Controls.RemoveAt(i);
							found = true;
							return;
						}
						Console.WriteLine(found);
						SearchControl(control.Controls[i]);
					}
				}
			}


		}
	}
}
