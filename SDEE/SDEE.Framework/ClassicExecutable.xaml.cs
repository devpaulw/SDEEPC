using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SDEE.Framework
{
    /// <summary>
    /// Interaction logic for TaskbarExecutable.xaml
    /// </summary>
    public partial class ClassicExecutable : UserControl
    {
        public string ExecutablePath { get; set; }

        public ClassicExecutable() {
            InitializeComponent();
        }

        public ClassicExecutable(string executablePath) : this()
        {
            ExecutablePath = executablePath;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = ExecutablePath;

                try
                {
                    process.Start();
                }
                catch (System.ComponentModel.Win32Exception) // Should appear when execution cancelled
                {
                    // DO NOTHING (not important)
                }
            }
        }

        private static ImageSource ExtractAssociatedIcon(string executablePath)
        {
            var sysicon = System.Drawing.Icon.ExtractAssociatedIcon(executablePath);
            var bmpSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                sysicon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            sysicon.Dispose();

            return bmpSrc;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            var image = new FrameworkElementFactory(typeof(Image));
            template.VisualTree = image;
            image.SetValue(Image.SourceProperty, ExtractAssociatedIcon(ExecutablePath));
            button.Template = template;
        }
    }
}
