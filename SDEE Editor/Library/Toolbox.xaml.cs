using System;
using System.Collections.Generic;
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
    public partial class Toolbox : UserControl
    {
        public Toolbox()
        {
            InitializeComponent();
            //Initialize();
            InitializeTest();
        }

        public void Initialize()
        {
            string sdeePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SDEE", "frameworks");
            string[] directories = Directory.GetDirectories(sdeePath);

            foreach (var directory in directories)
            {
                string[] files = Directory.GetFiles(System.IO.Path.Combine(sdeePath, directory));
                var group = new TreeViewItem
                {
                    Header = System.IO.Path.GetFileName(directory)
                };

                foreach (var file in files)
                {
                    var item = new TreeViewItem()
                    {
                        Header = System.IO.Path.GetFileNameWithoutExtension(file)
                    };
                    group.Items.Add(item);
                }
                _tree.Items.Add(group);
            }
        }

        public void InitializeTest()
        {
            _tree.Items.Add(new LibraryElement("SpongeTaskbar", () => new Rectangle
            {
                Fill = Brushes.DeepSkyBlue,
                Height = 50,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Stretch
            }));            
            
            _tree.Items.Add(new LibraryElement("SpongeGatesButton", () => new Button
            {
                Content = "Hello Mr. Gates",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(20, 35, 50, 35)
            }));

            _tree.Items.Add(new LibraryElement("SpongeButton", () => new Button
            {
                Content = "Do you know the way?\n=> Sponge bob.",
                Height = 50,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            }));

            _tree.Items.Add(new LibraryElement("SpongeDesktop", () => new Rectangle
            {
                Fill = Brushes.Green,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            }));

            _tree.Items.Add(new LibraryElement("SpongeCalendar", () => new Calendar
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 5, 5, 5)
            }));
        }

        public event EventHandler<FrameworkElement> ElementClicked;

        // Drag & Drop
        private void Tool_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                if (sender is TreeViewItem tvi)
                    if (tvi.Header is LibraryElement elem)
                    {
                        DataObject dObj = new DataObject(typeof(FrameworkElement), elem.GetElement() /* Extracts the element from the LibraryElement */);
                        DragDrop.DoDragDrop(this, dObj, DragDropEffects.Copy);
                    }
        }

        // Double-click
        private void Tool_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (sender is TreeViewItem tvi)
                    if (tvi.Header is LibraryElement elem)
                    {
                        ElementClicked?.Invoke(this, elem.GetElement());
                    }
        }
    }
}
