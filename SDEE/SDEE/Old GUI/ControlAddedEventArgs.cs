using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    public class ControlAddedEventArgs : EventArgs
    {
        public Control Control { get; }
        public Dictionary<string, object> Parameters { get; }

        public ControlAddedEventArgs(Control control, Dictionary<string, object> parameters = null)
        {
            Control = control;
            Parameters = parameters;
        }
    }

    
}
