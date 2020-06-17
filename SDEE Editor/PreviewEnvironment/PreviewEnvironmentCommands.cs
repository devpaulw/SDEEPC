using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace SDEE_Editor.PreviewEnvironment
{
    class PreviewEnvironmentCommands
    {
        public static readonly RoutedUICommand RemoveSelectedElement // TODO Try with {get;}
            = new RoutedUICommand("Remove", "RemoveSelectedElement", typeof(PreviewEnvironmentFrame),
                        new InputGestureCollection { new KeyGesture(Key.Delete, ModifierKeys.None) });
    }
}
