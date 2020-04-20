using SDEE.Framework;
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

namespace SDEE.Launcher
{
    /// <summary>
    /// Interaction logic for MyTaskbarXaml.xaml
    /// </summary>
    public partial class MyTaskbarXaml : UserControl
    {
        public event RoutedEventHandler ToggleStartMenu;

        public MyTaskbarXaml()
        {
            InitializeComponent();

            elementsSp.Children.Add(new ClassicExecutable(@"c:\windows\system32\cmd.exe"));
            elementsSp.Children.Add(new ClassicExecutable(@"c:\windows\notepad.exe"));
            elementsSp.Children.Add(new ClassicExecutable(@"c:\windows\system32\winver.exe"));
        }

        private void MyTaskbar_Loaded(object sender, RoutedEventArgs e)
        {
            startButton.PreviewMouseLeftButtonDown += (s, e_) => ToggleStartMenu.Invoke(s, (RoutedEventArgs)e_);
        }
    }
}
