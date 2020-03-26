using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	public class ControlId
	{
		public ControlType Type { get; private set; }
		public uint No { get; set; } = 0;

		public ControlId(ControlType type, uint id)
		{
			Type = type;
			No = id;
		}

		public override bool Equals(object obj)
		{
			return obj is ControlId controlId &&
				   Type == controlId.Type &&
				   No == controlId.No;
		}

		public override int GetHashCode()
		{
			int hashCode = -1319371435;
			hashCode = hashCode * -1521134295 + Type.GetHashCode();
			hashCode = hashCode * -1521134295 + No.GetHashCode();
			return hashCode;
		}

		public static ControlId NotSavable
			=> new ControlId(ControlType.NotSavable, 0);
		
	public static bool operator ==(ControlId id1, ControlId id2)
		{
			return id1.Equals(id2);
		}
		public static bool operator !=(ControlId id1, ControlId id2)
			=> !(id1 == id2);
	}
}
