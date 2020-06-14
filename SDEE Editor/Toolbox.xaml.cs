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
            _tree.Items.Add(new TreeViewItem
            {
                Header = "SpongeTaskbar",
                Style = Resources["toolStyle"] as Style,
                Tag = new Rectangle
                {
                    Name = "SpongeTaskbar",
                    Fill = Brushes.Blue,
                    Height = 50,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                }
            });

            _tree.Items.Add(new TreeViewItem
            {
                Header = "SpongeDesktop",
                Style = Resources["toolStyle"] as Style,
                Tag = new Rectangle
                {
                    Name = "SpongeDesktop",
                    Fill = Brushes.Green,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                }
            });

            _tree.Items.Add(new TreeViewItem
            {
                Header = "SpongeButton",
                Style = Resources["toolStyle"] as Style,
                Tag = new Button
                {
                    Name = "SpongeButton",
                    Content = "Do you know the way?\n=> Sponge bob.",
                    Height = 50,
                    Width = 150,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            });
        }

        public event EventHandler<FrameworkElement> ElementClicked;

        private void Tool_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                if (sender is TreeViewItem tvi)
                    if (tvi.Tag is FrameworkElement elem)
                    {
                        DataObject dObj = new DataObject(typeof(FrameworkElement), elem);
                        DragDrop.DoDragDrop(this, dObj, DragDropEffects.Copy);
                    }
        }

        private void Tool_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (sender is TreeViewItem tvi)
                    if (tvi.Tag is FrameworkElement elem)
                    {
                        ElementClicked?.Invoke(this, elem);
                    }
        }
    }
}
