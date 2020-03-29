using System;
using System.Runtime.Serialization;

namespace SDEE_Editor
{
	[Serializable]
	internal class InvalidAttributeException : Exception
	{
		public InvalidAttributeException()
		{
		}

		public InvalidAttributeException(string message) : base(message)
		{
		}

		public InvalidAttributeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}