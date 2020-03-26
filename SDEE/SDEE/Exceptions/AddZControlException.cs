using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{

    [Serializable]
    public class AddZControlException : Exception
    {
        public AddZControlException()  : this("Can't add Z Control to a LControl other than the DE")
        { }
        public AddZControlException(string message) : base(message) { }
        public AddZControlException(string message, Exception inner) : base(message, inner) { }
        protected AddZControlException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
