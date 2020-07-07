using SDEE_Editor.Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace SDEE_Editor.InteractiveEnvironment
{
    /// </summary>
    /// Interaction logic for EditorElement.xaml <br/>
    /// Represents an element that is a FrameworkElement that is not HitTestVisible but still supports Mouse and Keyboard input.
    /// </summary>
    public partial class InteractiveEnvironmentElement : ContentControl
    {
        public InteractiveEnvironmentElement(FrameworkElement elementValue)
        {
            ElementValue = elementValue ?? throw new ArgumentNullException(nameof(elementValue));
            
            InitializeComponent();

            UpdateContentControl();
        }

        private void UpdateContentControl()
        {
            FrameworkElement adaptedElem = ElementValue.Clone();

            adaptedElem.HorizontalAlignment = HorizontalAlignment.Stretch;
            adaptedElem.VerticalAlignment = VerticalAlignment.Stretch;
            adaptedElem.Margin = new Thickness(0);
            adaptedElem.Width = double.NaN;
            adaptedElem.Height = double.NaN;

            contentCtrl.Content = adaptedElem;
        }

        public FrameworkElement ElementValue { get; }
    }
}
