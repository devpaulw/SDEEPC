using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
	[Serializable]
	public class NotHexColorException : Exception
	{
		public NotHexColorException() { }
		public NotHexColorException(string message) : base(message) { }
		public NotHexColorException(string message, Exception inner) : base(message, inner) { }
		protected NotHexColorException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
