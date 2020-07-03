using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE_Editor.PreviewEnvironment
{

    [Serializable]
    public class PreviewEnvironmentException : Exception
    {
        public PreviewEnvironmentException() { }
        public PreviewEnvironmentException(string message) : base(message) { }
        public PreviewEnvironmentException(string message, Exception inner) : base(message, inner) { }
        protected PreviewEnvironmentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
