using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace SDEE_Editor.InteractiveEnvironment
{
    class InteractiveEnvironmentCommands
    {
        public static RoutedUICommand RemoveSelectedElement { get; }
            = new RoutedUICommand("Remove", "RemoveSelectedElement", typeof(InteractiveEnvironmentBox),
                        new InputGestureCollection { new KeyGesture(Key.Delete, ModifierKeys.None) });
            

        public static RoutedUICommand DeselectAll { get; }
            = new RoutedUICommand("Deselect all", "DeselectAll", typeof(InteractiveEnvironmentBox),
                        new InputGestureCollection { new KeyGesture(Key.Escape, ModifierKeys.None) });
    }
}
