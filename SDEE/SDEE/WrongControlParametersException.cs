using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{

    [Serializable]
    public class WrongControlParametersException : Exception
    {
        public WrongControlParametersException(Dictionary<string, Type> requiredParameters) 
            : this("There are control wrong parameters, these are the required parameters:\n" 
                  + string.Join(",\n", requiredParameters.Select(parameter => string.Join(" ", "(" + parameter.Value + "):", parameter.Key)))
                  + "\nUse the Control.Load(Control, object[]) instead.")
        { 
        }

        [Obsolete("Parameters would better be specified")] public WrongControlParametersException() : base() { }
        public WrongControlParametersException(string message) : base(message) { }
        public WrongControlParametersException(string message, Exception inner) : base(message, inner) { }
        protected WrongControlParametersException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
