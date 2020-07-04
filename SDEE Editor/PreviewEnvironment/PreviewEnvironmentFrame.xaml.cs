﻿using System;
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
    public partial class PreviewEnvironmentFrame : UserControl /*HTBD Make it a Selector*/
    {
        // TODO Make a PreviewEnvironmentGrid instead of a Frame directly but keep Frame for background and stroke
        public PreviewEnvironmentFrame()
        {
            InitializeComponent();

            Elements = new ObservableCollection<FrameworkElement>();
            Elements.CollectionChanged += Elements_CollectionChanged;

            // TODO When this control lose the focus, deselect selected element
        }

        /// <summary>
        /// When PreviewEnvironment.SelectedElement value changed
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

        private PreviewEnvironmentElement GetContainerFromElement(FrameworkElement element)
        {
            return prevGrid.Children
                .OfType<PreviewEnvironmentElement>()
                .Where(peElem => peElem.ElementValue == element)
                .FirstOrDefault(); // OPTI
        }

        private void PrevGrid_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SurrounderRect_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedElementChanged += OnSelectedElementChanged; // We add it here in order to be sure that the surrounder rect has been initialized
        }

        private void OnSelectedElementChanged(object sender, EventArgs e)
        {
            elemSelector.SurroundElement(SelectedElement);
        }

        private void Elements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Set elements to the real grid
            prevGrid.Children.Clear();
            foreach (FrameworkElement element in Elements)
            {
                PreviewEnvironmentElement peElem = new PreviewEnvironmentElement(element)
                {
                };

                peElem.MouseDown += PeElem_MouseDown;

                prevGrid.Children.Add(peElem);
            } // HTBD Optimize this since we can know the action.

            if (e.Action != NotifyCollectionChangedAction.Move) // When we move, we don't deselect any selected element
                SelectedElement = null;
        }


        private void PeElem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PreviewEnvironmentElement selectedPeElem = sender as PreviewEnvironmentElement;
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

        private void PrevGrid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(FrameworkElement)))
            {
                if (e.Data.GetData(typeof(FrameworkElement)) is FrameworkElement elem)
                {
                    if (e.OriginalSource == prevGrid) // Make sure that we do not Drag Enter directly through an element, but rather when to reach the higher pe-grid
                    {
                        if (Elements.Contains(elem)) // Add it only when the Elements don't already contains it, otherwise there is a bug somewhere.
                            throw new PreviewEnvironmentException("The preview environment grid already contains this element.");

                        if (previewDraggingElem != null) // When an element is already supposed to be dragging, there is a bug somewhere.
                            throw new PreviewEnvironmentException("Can't drag two elements at the same time.");

                        Elements.Add(elem);
                        previewDraggingElem = elem;
                    }

                    MakeGridElementsHitTestVisible(false);
                }
            }
        }

        private void PrevGrid_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (previewDraggingElem != null)
                e.Effects = DragDropEffects.Copy;
        }

        private void PrevGrid_DragLeave(object sender, DragEventArgs e)
        {
            if (previewDraggingElem != null)
            {
                Elements.Remove(previewDraggingElem);
                previewDraggingElem = null;
                MakeGridElementsHitTestVisible(true);
            }
        }

        private void PrevGrid_Drop(object sender, DragEventArgs e)
        {
            previewDraggingElem = null;
            MakeGridElementsHitTestVisible(true);
        }

        private void MakeGridElementsHitTestVisible(bool visible)
        {
            foreach (UIElement elem in prevGrid.Children)
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
