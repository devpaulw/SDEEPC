using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SDEE_Editor
{
    /// <summary>
    /// Interaction logic for PreviewEnvironmentControl.xaml
    /// </summary>
    public partial class PreviewEnvironment : UserControl
    {
        public FrameworkElement SelectedElement { get; private set; }

        public PreviewEnvironment()
        {
            InitializeComponent();

            // TODO When this control lose the focus, deselect selected element
        }

        public void AddElement(FrameworkElement element)
        {
            if (element == null)
                throw new NullReferenceException("Couldn't add an element.");

            prevGrid.Children.Add(element);
            DeselectSelectedElement(); // Don't stay selected on an element, honor the new one
        }

        public void RemoveElement(FrameworkElement element)
        {
            if (element == null)
                throw new NullReferenceException("Couldn't remove an element.");

            prevGrid.Children.Remove(element);
            DeselectSelectedElement();
        }

        event EventHandler<FrameworkElement> ElementSelected;
        event EventHandler ElementDeselected;
        FrameworkElement previewDraggingElem;

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            if (e.Data.GetDataPresent(typeof(FrameworkElement)))
            {
                if (e.Data.GetData(typeof(FrameworkElement)) is FrameworkElement elem)
                {
                    AddElement(elem);
                    previewDraggingElem = elem;
                }
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            e.Effects = DragDropEffects.None;
            if (previewDraggingElem != null)
                e.Effects = DragDropEffects.Copy;
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);

            if (previewDraggingElem != null)
            {
                RemoveElement(previewDraggingElem);
                previewDraggingElem = null;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            previewDraggingElem = null;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                // Find clicked element
                Point mPos = e.GetPosition(this);
                FrameworkElement mouseOverElem = null;

                foreach (FrameworkElement ctrl in prevGrid.Children)
                {
                    Rect ctrlRect = ctrl.TransformToVisual(this).TransformBounds(new Rect(ctrl.RenderSize));
                    if (mPos.X >= ctrlRect.Left && mPos.X <= ctrlRect.Right && mPos.Y >= ctrlRect.Top && mPos.Y <= ctrlRect.Bottom) // if Mouse Position is inside control bounds
                        mouseOverElem = ctrl;
                }

                if (mouseOverElem != null)
                    SelectElement(mouseOverElem);
                else // When we actually select nothing
                    DeselectSelectedElement();
            }
        }

        /// <summary>
        /// Select an element, surround it and call an event
        /// </summary>
        /// <param name="element">The element to be selected</param>
        private void SelectElement(FrameworkElement element) // It's like an event raiser, more reliable than directly add an eventhandler
        {
            if (SelectedElement != element) // When this element is not already selected (so that in particular we don't invoke again the event)
            {
                surrounderRect.SurroundElement(element);
                SelectedElement = element;
                ElementSelected?.Invoke(this, element);
            }
        }

        /// <summary>
        /// Deselect any selected element
        /// </summary>
        private void DeselectSelectedElement() // It's like an event raiser
        {
            if (SelectedElement != null) // When there is no element selected (so that in particular we don't invoke again the event)
            {
                surrounderRect.UnsurroundSurroundedElement();
                ElementDeselected?.Invoke(this, EventArgs.Empty);
                SelectedElement = null;
            }
        }
    }
}
