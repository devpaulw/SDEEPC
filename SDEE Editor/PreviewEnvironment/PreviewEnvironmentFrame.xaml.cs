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
using System.ComponentModel.Design;
using System.Diagnostics;

namespace SDEE_Editor.PreviewEnvironment
{
    /// <summary>
    /// Interaction logic for PreviewEnvironmentControl.xaml
    /// </summary>
    public partial class PreviewEnvironmentFrame : UserControl
    {
        /// <summary>
        /// When PreviewEnvironment.SelectedElement value changed
        /// </summary>
        public event EventHandler SelectedElementChanged;

        private EditorElement selectedElement;

        /// <summary>
        /// Current selected element on your preview environment, a null value indicates that no element is selected.
        /// </summary>
        public EditorElement SelectedElement
        {
            get => selectedElement;
            set
            {
                if (selectedElement != value) // When this element is not already selected/deselected (so that in particular we don't invoke again the event)
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

        public ObservableCollection<EditorElement> Elements { get; }

        public PreviewEnvironmentFrame()
        {
            InitializeComponent();

            Elements = new ObservableCollection<EditorElement>();
            Elements.CollectionChanged += Elements_CollectionChanged;

            // TODO When this control lose the focus, deselect selected element
        }

        private void PrevGrid_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SurrounderRect_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedElementChanged += OnSelectedElementChanged; // We add it here in order to be sure that the surrounder rect has been initialized

            //surrounderRect.RemoveSelectedElement = () => Elements.Remove(SelectedElement);
        }

        private void OnSelectedElementChanged(object sender, EventArgs e)
        {
            elemSelector.SelectElement(SelectedElement);
        }

        private void Elements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            prevGrid.Children.Clear();
            foreach (EditorElement element in Elements)
            {
                prevGrid.Children.Add(element);
            } // HTBD Optimize this since we can know the action.

            if (e.Action != NotifyCollectionChangedAction.Move) // When we move, we don't deselect any selected element
                SelectedElement = null;
        }


        private EditorElement previewDraggingElem;

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            if (e.Data.GetDataPresent(typeof(EditorElement)))
            {
                if (e.Data.GetData(typeof(EditorElement)) is EditorElement elem)
                {
                    ////////EditorElement instancedElement = (elem ?? throw new NullReferenceException()).Invoke();
                    ////////// OPTI Don't always instance it, keep a copy during the drag

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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            // UNDONE TODO IMPORTANT Change this system

            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                // Find clicked element
                Point mPos = e.GetPosition(this);
                EditorElement elementClicked = null;

                foreach (EditorElement ctrl in Elements)
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

        private void RemoveSelectedElement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Elements.Remove(SelectedElement);
           
        }

        private void RemoveSelectedElement_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedElement != null;
        }
    }
}
