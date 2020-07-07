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
using SDEE_Editor.InteractiveEnvironment;
using System.ComponentModel.Design;
using System.Diagnostics;
// TODO Gather IE-Box and IE-Frame
namespace SDEE_Editor.InteractiveEnvironment
{
    /// <summary>
    /// Interaction logic for InteractiveEnvironmentControl.xaml
    /// </summary>
    public partial class InteractiveEnvironmentBox : UserControl /*HTBD Perhaps make it a Selector*/
    {
        public InteractiveEnvironmentBox()
        {
            InitializeComponent();

            Elements = new ObservableCollection<FrameworkElement>();
            Elements.CollectionChanged += Elements_CollectionChanged;
            SelectedElementChanged += OnSelectedElementChanged;
        }

        /// <summary>
        /// When InteractiveEnvironment.SelectedElement value changed
        /// </summary>
        public event EventHandler SelectedElementChanged;

        private FrameworkElement selectedElement;

        /// <summary>
        /// Current selected element on your preview environment, a null value indicates that no element is selected.
        /// </summary>
        public FrameworkElement SelectedElement
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

        public ObservableCollection<FrameworkElement> Elements { get; }

        private void OnSelectedElementChanged(object sender, EventArgs e)
        {
            elemSelector.SurroundElement(TryGetContainerFromElement(SelectedElement));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            elemSelector.SurroundElement(TryGetContainerFromElement(SelectedElement));
        }

        private InteractiveEnvironmentElement TryGetContainerFromElement(FrameworkElement element)
        {
            return grid.Children
                .OfType<InteractiveEnvironmentElement>()
                .Where(peElem => peElem.ElementValue == element)
                .FirstOrDefault(); // OPTI
        }

        private void Elements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Set elements to the real grid
            grid.Children.Clear();
            foreach (FrameworkElement element in Elements)
            {
                InteractiveEnvironmentElement peElem = new InteractiveEnvironmentElement(element)
                {
                };
                peElem.MouseDown += PeElem_MouseDown;

                grid.Children.Add(peElem);
            } // OPTI Optimize this since we can know the action.

            if (e.Action != NotifyCollectionChangedAction.Move) // When we move, we don't deselect any selected element
                SelectedElement = null;
        }


        private void PeElem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InteractiveEnvironmentElement selectedPeElem = sender as InteractiveEnvironmentElement;
            SelectedElement = selectedPeElem.ElementValue;
            e.Handled = true; // Any parent (supposed to be this PEFrame) that try to receive a MouseDown will be cancelled so that we focus on this.
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            //FocusManager.SetIsFocusScope(this, true);
            Focus();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e) // Preview (Tunneling) means: First on higher level elements while Buble (without 'Preview') means the contrary ; Anyway, it's a matter of order.
        {
            base.OnPreviewMouseDown(e);

            SelectedElement = null; // When select nothing
        }

        private FrameworkElement previewDraggingElem;

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FrameworkElement)))
            {
                if (e.Data.GetData(typeof(FrameworkElement)) is FrameworkElement elem)
                {
                    if (e.OriginalSource == sender && sender == grid) // Make sure that we do not Drag Enter directly through an element, but rather when to reach the higher pe-grid
                    {
                        if (Elements.Contains(elem)) // Add it only when the Elements don't already contains it, otherwise there is a bug somewhere.
                            throw new InteractiveEnvironmentException("The interactive environment grid already contains this element.");

                        if (previewDraggingElem != null) // When an element is already supposed to be dragging, there is a bug somewhere.
                            throw new InteractiveEnvironmentException("Can't drag two elements at the same time.");

                        Elements.Add(elem);
                        previewDraggingElem = elem;
                    }

                    MakeGridElementsHitTestVisible(false);
                }
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (previewDraggingElem != null)
                e.Effects = DragDropEffects.Copy;
        }

        private void Grid_DragLeave(object sender, DragEventArgs e)
        {

            if (previewDraggingElem != null)
            {
                Elements.Remove(previewDraggingElem);
                previewDraggingElem = null;
                MakeGridElementsHitTestVisible(true);
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            previewDraggingElem = null;
            MakeGridElementsHitTestVisible(true);
        }

        private void MakeGridElementsHitTestVisible(bool visible)
        {
            foreach (UIElement elem in grid.Children)
                elem.IsHitTestVisible = visible;
        }

        #region Command Bindings execution management
        private void RemoveSelectedElement_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedElement != null;
        }
        private void RemoveSelectedElement_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Elements.Remove(SelectedElement);
        }

        private void DeselectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedElement != null; // Only when there is one element selected
        }

        private void DeselectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedElement = null;
        }
        #endregion

    }
}
