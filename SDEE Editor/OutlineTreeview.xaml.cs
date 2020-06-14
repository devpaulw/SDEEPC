using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SDEE_Editor
{

    /// <summary>
    /// Interaction logic for Toolbox.xaml
    /// </summary>
    public partial class OutlineTreeview : UserControl
    {
        public OutlineTreeview()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PreviewEnvironmentProperty
            = DependencyProperty.Register(nameof(PreviewEnvironment),
            typeof(PreviewEnvironmentFrame),
            typeof(OutlineTreeview));

        public PreviewEnvironmentFrame PreviewEnvironment
        {
            get => (PreviewEnvironmentFrame)GetValue(PreviewEnvironmentProperty);
            set => SetValue(PreviewEnvironmentProperty, value);
        }

        private bool isReseting;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (PreviewEnvironment.Elements == null)
                throw new ArgumentNullException(nameof(PreviewEnvironment.Elements));

            PreviewEnvironment.Elements.CollectionChanged += PreviewEnvironment_GridElements_CollectionChanged;
            PreviewEnvironment.SelectedElementChanged += PrevEnv_SelectedElementChanged;
        }

        private void PrevEnv_SelectedElementChanged(object sender, EventArgs e)
        {
            SelectSelectedElementFromPrevEnv();
        }
        /// <summary>
        /// Select in this treeview, the current selected element of the Preview Environment
        /// </summary>
        private void SelectSelectedElementFromPrevEnv()
        {
            var tvi = outlineTreeview.Items.SourceCollection
                .OfType<TreeViewItem>()
                .Where(ptvi => ptvi.Tag == PreviewEnvironment.SelectedElement)
                .FirstOrDefault();

            if (tvi != null)
                tvi.IsSelected = true;
        }


        private void PreviewEnvironment_GridElements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            isReseting = true;
            outlineTreeview.Items.Clear();

            foreach (FrameworkElement element in PreviewEnvironment.Elements.Reverse())
            {
                TreeViewItem tvi = GetTviFromElem(element);
                outlineTreeview.Items.Add(tvi);

                if (element == PreviewEnvironment.SelectedElement) // Select the right element just as it appears
                    tvi.IsSelected = true;
            }
            isReseting = false;
        }


        private void DownArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (outlineTreeview.SelectedItem is TreeViewItem tvi)
                if (tvi.Tag is FrameworkElement elem)
                {
                    int index = PreviewEnvironment.Elements.IndexOf(elem);
                    PreviewEnvironment.Elements.TryMove(index, index - 1);
                }
        }

        private void UpArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (outlineTreeview.SelectedItem is TreeViewItem tvi)
                if (tvi.Tag is FrameworkElement elem)
                {
                    int index = PreviewEnvironment.Elements.IndexOf(elem);
                    PreviewEnvironment.Elements.TryMove(index, index + 1);
                }
        }

        private void OutlineTreeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                if (e.NewValue is TreeViewItem tvi)
                    PreviewEnvironment.SelectedElement = tvi.Tag as FrameworkElement;
            }
            else if (!isReseting) // A null value has been selected whereas this is not reseting.
                throw new NullReferenceException("An incorrect item has been selected.");
        }

        // TODO Make it more "WPF"
        private TreeViewItem GetTviFromElem(FrameworkElement element)
        {
            return new TreeViewItem()
            {
                Header = element.Name,
                Tag = element
            };
        }
    }


}
