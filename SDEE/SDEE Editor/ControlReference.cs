using SDEE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE_Editor
{
	class ControlReference
	{
		public ControlType Type { get; private set; }
		public uint Id { get; private set; }

		public ControlReference(ControlType type, uint no)
		{
			Type = type;
			Id = no;
		}

		public bool References(Control control)
		{
			return Id == control.Id && Type == control.Type;
		}
	}
}
