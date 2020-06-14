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

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            MouseDown += OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                // Find clicked element
                Point mPos = e.GetPosition(this);
                FrameworkElement mouseOverElem = null;

                foreach (FrameworkElement ctrl in Children)
                {
                    Rect ctrlRect = ctrl.TransformToVisual(this).TransformBounds(new Rect(ctrl.RenderSize));
                    if (mPos.X >= ctrlRect.Left && mPos.X <= ctrlRect.Right && mPos.Y >= ctrlRect.Top && mPos.Y <= ctrlRect.Bottom) // if Mouse Position is inside control bounds
                        mouseOverElem = ctrl;
                }

                ElementClicked?.Invoke(this, mouseOverElem);
            }
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (visualAdded is UIElement aElem) // Visual added
            {
                // Disable run-time features on the control added
                aElem.IsHitTestVisible = false;
            }
            if (visualRemoved is UIElement rElem)
            {
            }
        }
    }
}
