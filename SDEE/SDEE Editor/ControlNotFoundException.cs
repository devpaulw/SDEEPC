using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE_Editor
{

    [Serializable]
    public class InvalidControlException : Exception
    {
        public InvalidControlException() { }
        public InvalidControlException(string message) : base(message) { }
        public InvalidControlException(string message, Exception inner) : base(message, inner) { }
        protected InvalidControlException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
