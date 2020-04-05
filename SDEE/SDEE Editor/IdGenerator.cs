using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDEE;
using ControlNo = System.UInt32;

namespace SDEE_Editor
{
	class IdGenerator
	{
		private ControlNo id;

		public uint GenerateNextId()
		{
			return ++id;
		}

		public IdGenerator(DesktopEnvironment de)
		{
			ControlNo maxId = 1;
			FindMaxId(de);
			id = maxId;

			void FindMaxId(Control control)
			{
				foreach (var child in control.Controls)
				{
					if (child.Id > maxId)
						maxId = child.Id;

					FindMaxId(child);
				}
			}
		}
	}
}
