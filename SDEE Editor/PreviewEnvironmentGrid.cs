using System.Windows;
using System.Windows.Controls;

namespace SDEE_Editor
{
    /// <summary>
    /// Preview Environment Grid can't receive mouse and keyboard input
    /// </summary>
    class PreviewEnvironmentGrid : Grid
    {
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            
            if (visualAdded is UIElement elem) // Visual added
            {
                // Disable run-time features on the control added
                elem.IsHitTestVisible = false;
            }
        }
    }
}
