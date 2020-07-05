using SDEE_Editor.Miscellaneous;
using SDEE_Editor.InteractiveEnvironment;
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
            //CommandBindings.Add(new CommandBinding(InteractiveEnvironmentCommands.RemoveSelectedElement));
        }

        public static readonly DependencyProperty InteractiveEnvironmentGridProperty
            = DependencyProperty.Register(nameof(InteractiveEnvironmentGrid),
            typeof(InteractiveEnvironmentGrid),
            typeof(Outline));

        public InteractiveEnvironmentGrid InteractiveEnvironmentGrid
        {
            get => (InteractiveEnvironmentGrid)GetValue(InteractiveEnvironmentGridProperty);
            set => SetValue(InteractiveEnvironmentGridProperty, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InteractiveEnvironmentGrid.SelectedElementChanged += PrevEnv_SelectedElementChanged;
        }

        private void PrevEnv_SelectedElementChanged(object sender, EventArgs e)
        {
            // Select in the listBox, the current selected element of the Preview Environment
            listBox.SelectedItem = InteractiveEnvironmentGrid.SelectedElement;
        }

        private void DownArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is FrameworkElement elem)
            {
                InteractiveEnvironmentGrid.Elements.TryMoveElementBy(elem, -1);
            }
        }

        private void UpArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is FrameworkElement elem)
            {
                InteractiveEnvironmentGrid.Elements.TryMoveElementBy(elem, 1);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem is FrameworkElement selectedObject)
            {
                InteractiveEnvironmentGrid.SelectedElement = selectedObject;
            }
        }
    }
}
