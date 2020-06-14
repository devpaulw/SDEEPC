using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static readonly DependencyProperty PreviewEnvironmentProperty = DependencyProperty.Register(nameof(PreviewEnvironment),  typeof(PreviewEnvironment), typeof(OutlineTreeview));
        public PreviewEnvironment PreviewEnvironment { get => (PreviewEnvironment)GetValue(PreviewEnvironmentProperty); set => SetValue(PreviewEnvironmentProperty, value); }

        public OutlineTreeview()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (PreviewEnvironment.GridElements == null)
                throw new ArgumentNullException(nameof(PreviewEnvironment.GridElements));

            PreviewEnvironment.GridElements.CollectionChanged += PreviewEnvironment_GridElements_CollectionChanged;
            PreviewEnvironment.SelectedElementChanged += OnElementSelected;
        }

        private void OnElementSelected(object sender, EventArgs e)
        {
            SelectPrevEnvSelectedElement();
        }

        private void PreviewEnvironment_GridElements_CollectionChanged(object sender, EventArgs e)
        {
            outlineTreeview.Items.Clear();

            foreach (FrameworkElement element in PreviewEnvironment.GridElements)
            {
                outlineTreeview.Items.Add(GetTviFromElem(element));
            }

            SelectPrevEnvSelectedElement();
        }

        private void SelectPrevEnvSelectedElement()
        {
            //var tvi = outlineTreeview.Items.SourceCollection
            //    .OfType<TreeViewItem>()
            //    .Where(ptvi => ptvi.Tag == PreviewEnvironment.SelectedElement)
            //    .FirstOrDefault();

            //if (tvi != null)
            //    tvi.IsSelected = true;
        }


        private void DownArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (outlineTreeview.SelectedItem is TreeViewItem tvi)
                if (tvi.Tag is FrameworkElement elem)
                {
                    int index = PreviewEnvironment.GridElements.IndexOf(elem);
                    try
                    {
                        PreviewEnvironment.GridElements.Move(index, index + 1);
                    }
                    catch (ArgumentOutOfRangeException) { }
                }
        }

        private void UpArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (outlineTreeview.SelectedItem is TreeViewItem tvi)
                if (tvi.Tag is FrameworkElement elem)
                {
                    int index = PreviewEnvironment.GridElements.IndexOf(elem);
                    try
                    {
                        PreviewEnvironment.GridElements.Move(index, index - 1);
                    }
                    catch (ArgumentOutOfRangeException) { new Action(() => { }).Invoke(); /*Troll*/ }
                }
        }

        private void OutlineTreeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            PreviewEnvironment.SelectedElement = e.NewValue is TreeViewItem tvi ? tvi.Tag as FrameworkElement : null;
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
