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
        public static RoutedUICommand RemoveSelectedElement { get; }
            = new RoutedUICommand("Remove", "RemoveSelectedElement", typeof(PreviewEnvironmentGrid),
                        new InputGestureCollection { new KeyGesture(Key.Delete, ModifierKeys.None) });
            

        public static RoutedUICommand DeselectAll { get; }
            = new RoutedUICommand("Deselect all", "DeselectAll", typeof(PreviewEnvironmentGrid),
                        new InputGestureCollection { new KeyGesture(Key.Escape, ModifierKeys.None) });
    }
}
