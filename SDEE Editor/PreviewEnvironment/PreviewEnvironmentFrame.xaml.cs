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

namespace SDEE_Editor.PreviewEnvironment
{
    /// <summary>
    /// Supports the background style and selection style of <seealso cref="PreviewEnvironmentGrid"/><br/>
    /// Interaction logic for PreviewEnvironmentFrame.xaml
    /// </summary>
    public partial class PreviewEnvironmentFrame : UserControl
    {
        public PreviewEnvironmentFrame()
        {
            InitializeComponent();

            
        }

        public PreviewEnvironmentGrid Grid { get => prevGrid; }

        private void Grid_SelectedElementChanged(object sender, EventArgs e)
        {
            elemSelector.SurroundElement(Grid.SelectedElement);
        }
    }
}

// HTBD Organize the project with MVVM pattern