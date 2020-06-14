using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Linq;
using System.Collections;
using System.Windows.Media.Animation;
using SDEE_Editor.PreviewEnvironment;

namespace SDEE_Editor.PreviewEnvironment
{
    /// <summary>
    /// Interaction logic for PreviewEnvironmentControl.xaml
    /// </summary>
    public partial class PreviewEnvironmentFrame : UserControl
    {
        /// <summary>
        /// Current selected element on your preview environment, a null value indicates that no element is selected.
        /// </summary>
        public FrameworkElement SelectedElement
        { 
            get => selectedElement; 
            set
            {
                if (SelectedElement != value) // When this element is not already selected/deselected (so that in particular we don't invoke again the event)
                {
                    selectedElement = value;
                    SelectedElementChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        //public void SelectElement(FrameworkElement element, bool focus)
        //{
        //    SelectedElement = element;
        //    if (focus && element != null)
        //        surrounderRect.Focus();
       //}

        public PreviewEnvironmentGridCollection Elements { get; private set; }

        public PreviewEnvironmentFrame()
        {
            InitializeComponent();
            // TODO When this control lose the focus, deselect selected element
        }

        private void PrevGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Elements = new PreviewEnvironmentGridCollection(prevGrid);
            Elements.CollectionChanged += Grid_CollectionChanged;
        }

        private void SurrounderRect_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedElementChanged += OnSelectedElementChanged; // We add it here in order to be sure that the surrounder rect has been initialized
            surrounderRect.RemoveSelectedElement = () => Elements.Remove(SelectedElement);
        }

        private void OnSelectedElementChanged(object sender, EventArgs e)
        {
            surrounderRect.SurroundElement(SelectedElement);
        }

        private void Grid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Move) // When we move, we don't unsurround any selected element
                SelectedElement = null;
        }

        /// <summary>
        /// When PreviewEnvironment.SelectedItem value changed
        /// </summary>
        public event EventHandler SelectedElementChanged;

        FrameworkElement previewDraggingElem;
        private FrameworkElement selectedElement;

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            if (e.Data.GetDataPresent(typeof(FrameworkElement)))
            {
                if (e.Data.GetData(typeof(FrameworkElement)) is FrameworkElement elem)
                {
                    Elements.Add(elem);
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
                Elements.Remove(previewDraggingElem);
                previewDraggingElem = null;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            previewDraggingElem = null;
        }

        private void PrevGrid_ElementClicked(object sender, FrameworkElement elementClicked)
        {
            
            //SelectElement(elementClicked, true);
        }



        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                // Find clicked element
                Point mPos = e.GetPosition(this);
                FrameworkElement elementClicked = null;

                foreach (FrameworkElement ctrl in Elements)
                {
                    Rect ctrlRect = ctrl.TransformToVisual(this).TransformBounds(new Rect(ctrl.RenderSize));
                    if (mPos.X >= ctrlRect.Left && mPos.X <= ctrlRect.Right && mPos.Y >= ctrlRect.Top && mPos.Y <= ctrlRect.Bottom) // if Mouse Position is inside control bounds
                        elementClicked = ctrl;

                }

                // Element clicked
                SelectedElement = elementClicked;
            }

            base.OnMouseDown(e);
        }

        // UNDONE finish this , SelectedElement = ? ; When I use OnSelectedElement event? How can I implement well OutlineTreeview itemTemplate ?
        // Fix bugs like we can't select items above, can we select nothing without bad focus and is focus properly installed.
        // And then:
        // TODO good move in outline treeview, keep it.
    }
}
