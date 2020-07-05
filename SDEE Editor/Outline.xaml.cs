using SDEE_Editor.Miscellaneous;
using SDEE_Editor.PreviewEnvironment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
            //CommandBindings.Add(new CommandBinding(PreviewEnvironmentCommands.RemoveSelectedElement));
        }

        public static readonly DependencyProperty PreviewEnvironmentGridProperty
            = DependencyProperty.Register(nameof(PreviewEnvironmentGrid),
            typeof(PreviewEnvironmentGrid),
            typeof(Outline));

        public PreviewEnvironmentGrid PreviewEnvironmentGrid
        {
            get => (PreviewEnvironmentGrid)GetValue(PreviewEnvironmentGridProperty);
            set => SetValue(PreviewEnvironmentGridProperty, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PreviewEnvironmentGrid.SelectedElementChanged += PrevEnv_SelectedElementChanged;
        }

        private void PrevEnv_SelectedElementChanged(object sender, EventArgs e)
        {
            // Select in the listBox, the current selected element of the Preview Environment
            listBox.SelectedItem = PreviewEnvironmentGrid.SelectedElement;
        }

        private void DownArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is FrameworkElement elem)
            {
                PreviewEnvironmentGrid.Elements.TryMoveElementBy(elem, -1);
            }
        }

        private void UpArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is FrameworkElement elem)
            {
                PreviewEnvironmentGrid.Elements.TryMoveElementBy(elem, 1);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem is FrameworkElement selectedObject)
            {
                PreviewEnvironmentGrid.SelectedElement = selectedObject;
            }
        }
    }
}
