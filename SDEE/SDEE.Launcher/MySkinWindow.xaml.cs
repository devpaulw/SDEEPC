using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using SDEE.Framework;

namespace SDEE.Launcher
{
    /// <summary>
    /// Interaction logic for MySkinWindow.xaml
    /// </summary>
    public partial class MySkinWindow : SkinWindowControl
    {
        public MySkinWindow()
        {
            InitializeComponent();
            tTitleLabel.Content = Title;
        }

        public override Rectangle PreviewAppRect => previewApplicationRect;

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) // TODO better names of these
        {
            ExecuteMinimize();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteClose();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteMaximize();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteShowInfos();
        }
    }
}
