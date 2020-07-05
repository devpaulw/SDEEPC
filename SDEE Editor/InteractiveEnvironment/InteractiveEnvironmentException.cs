using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE_Editor.InteractiveEnvironment
{

    [Serializable]
    public class InteractiveEnvironmentException : Exception
    {
        public InteractiveEnvironmentException() { }
        public InteractiveEnvironmentException(string message) : base(message) { }
        public InteractiveEnvironmentException(string message, Exception inner) : base(message, inner) { }
        protected InteractiveEnvironmentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
