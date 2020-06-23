using SDEE_Editor.PreviewEnvironment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
    public partial class Outline : UserControl
    {
        public Outline()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PreviewEnvironmentProperty
            = DependencyProperty.Register(nameof(PreviewEnvironmentFrame),
            typeof(PreviewEnvironmentFrame),
            typeof(Outline));

        public PreviewEnvironmentFrame PreviewEnvironmentFrame
        {
            get => (PreviewEnvironmentFrame)GetValue(PreviewEnvironmentProperty);
            set => SetValue(PreviewEnvironmentProperty, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (PreviewEnvironmentFrame.Elements == null)
                throw new ArgumentNullException(nameof(PreviewEnvironmentFrame.Elements));

            PreviewEnvironmentFrame.SelectedElementChanged += PrevEnv_SelectedElementChanged;
        }

        private void PrevEnv_SelectedElementChanged(object sender, EventArgs e)
        {
            // Select in the listBox, the current selected element of the Preview Environment
            listBox.SelectedItem = PreviewEnvironmentFrame.SelectedElement;
        }

        private void DownArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is EditorElement elem)
            {
                int index = PreviewEnvironmentFrame.Elements.IndexOf(elem);

                try
                {
                    PreviewEnvironmentFrame.Elements.Move(index, index - 1);
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        private void UpArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is EditorElement elem)
            {
                int index = PreviewEnvironmentFrame.Elements.IndexOf(elem);
                
                try
                {
                    PreviewEnvironmentFrame.Elements.Move(index, index + 1);
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditorElement selectedObject = listBox.SelectedItem as EditorElement; // HTBD, TEMP: Currently we can handle one item, so it's normal if we take the last one.

            if (selectedObject != null)
            {
                PreviewEnvironmentFrame.SelectedElement = selectedObject;
            }
            //else if (!isReseting) // A null value has been selected whereas this is not reseting.
            //    throw new NullReferenceException("An incorrect item has been selected.");
        }

        // TODO Make it more "WPF"
        //private TreeViewItem GetTviFromElem(FrameworkElement element)
        //{
        //    return new TreeViewItem()
        //    {
        //        Header = element.Name,
        //        Tag = element
        //    };
        //}
    }
}
