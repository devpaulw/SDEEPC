using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{

    [Serializable]
    public class NoDEImplementedException : Exception
    {
        public NoDEImplementedException() : base("No Desktop Environment is implemented as a top level parent.")
        { }
        //public NoDEImplementedException(string message) : base(message) { }
        //public NoDEImplementedException(string message, Exception inner) : base(message, inner) { }
        protected NoDEImplementedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
