using SDEE.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace SDEE.Launcher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("SDEE Project CodeName -");
            Console.WriteLine("Test cmd window");
            Console.WriteLine("Do not close this window directly, it's not safe because the Skin Engine would not be stopped properly.");

            var dep = new DesktopEnvironment();

            MyTaskbarXaml taskbar = new MyTaskbarXaml();
            MyStartMenu startMenu = new MyStartMenu()
            {
                Visibility = Visibility.Hidden,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, taskbar.Height)
            };
            taskbar.ToggleStartMenu += (s, e) => startMenu.Toggle();

            dep.Desktop.Content = new MyDesktop();

            Grid explorerGrid = new Grid();

            explorerGrid.Children.Add(taskbar);
            explorerGrid.Children.Add(startMenu);

            dep.FloatingElements.Add(explorerGrid);

            dep.GetNewSkinWindowControl = () => new MySkinWindow();

            //dep.FloatingElements.Add(new Button() { Width=double.NaN, Height = 50, Opacity = 0.7,
            //    VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(10, 10, 0, 0) });
            //dep.FloatingElements.Add(new CheckBox() { Width = 500, Height = 400, Opacity=0.2 });

            dep.Run();
            dep.Shutdown();
        }
    }
}
