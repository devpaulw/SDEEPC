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
            const string spongeTaskbarName = "SpongeTaskbar";

            _tree.Items.Add(new TreeViewItem
            {
                Header = spongeTaskbarName, // TODO This could be attached to LibraryElement.ElementName
                Style = Resources["toolStyle"] as Style, // TODO This should be in the ItemTemplate of ItemContainerStyle
                Tag = new LibraryElement(spongeTaskbarName, () => new Rectangle
                {
                    Fill = Brushes.DeepSkyBlue,
                    Height = 50,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                })
            });

            _tree.Items.Add(new TreeViewItem
            {
                Header = "SpongeDesktop",
                Style = Resources["toolStyle"] as Style,
                Tag = new LibraryElement("SpongeDesktop", () => new Rectangle
                {
                    Fill = Brushes.Green,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                })
            });


            _tree.Items.Add(new TreeViewItem
            {
                Header = "SpongeButton",
                Style = Resources["toolStyle"] as Style,
                Tag = new LibraryElement("SpongeButton", () => new Button
                {
                    Content = "Do you know the way?\n=> Sponge bob.",
                    Height = 50,
                    Width = 150,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                })
            });

            _tree.Items.Add(new TreeViewItem
            {
                Header = "SpongeLargeButton",
                Style = Resources["toolStyle"] as Style,
                Tag = new LibraryElement("SpongeLargeButton", () => new Button
                {
                    Content = "Hello Mr. Gates",
                    Width = 100,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Left
                })
            });
        }

        public event EventHandler<FrameworkElement> ElementClicked;

        private void Tool_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                if (sender is TreeViewItem tvi)
                    if (tvi.Tag is LibraryElement elem)
                    {
                        DataObject dObj = new DataObject(typeof(FrameworkElement), (elem ?? throw new NullReferenceException()).GetElement() /* Extracts the element from the LibraryElement */);
                        DragDrop.DoDragDrop(this, dObj, DragDropEffects.Copy);
                    }
        }

        private void Tool_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO Replace this by a command
            if (e.ChangedButton == MouseButton.Left)
                if (sender is TreeViewItem tvi)
                    if (tvi.Tag is LibraryElement elem)
                    {
                        ElementClicked?.Invoke(this, (elem ?? throw new NullReferenceException()).GetElement());
                    }
        }
    }
}
