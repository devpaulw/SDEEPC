using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SDEE_Editor
{
    /// <summary>
    /// Preview Environment Grid can't receive mouse and keyboard input
    /// </summary>
    public class PreviewEnvironmentGrid : Grid
    {
        public event EventHandler<FrameworkElement> ElementClicked;
    }
}
