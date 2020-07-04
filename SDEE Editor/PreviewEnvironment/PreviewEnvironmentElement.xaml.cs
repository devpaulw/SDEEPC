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
// TODO Unselect command
namespace SDEE_Editor.PreviewEnvironment
{
    /// </summary>
    /// Interaction logic for EditorElement.xaml <br/>
    /// Represents an element that is a FrameworkElement that is not HitTestVisible but still supports Mouse and Keyboard input.
    /// </summary>
    public partial class PreviewEnvironmentElement : ContentControl
    {
        public PreviewEnvironmentElement(FrameworkElement elementValue)
        {
            ElementValue = elementValue ?? throw new ArgumentNullException(nameof(elementValue));

            InitializeComponent();
        }

        public FrameworkElement ElementValue { get; }
    }
}
