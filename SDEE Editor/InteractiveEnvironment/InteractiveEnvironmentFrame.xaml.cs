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

namespace SDEE_Editor.InteractiveEnvironment
{
    /// <summary>
    /// Supports the background style and selection style of <seealso cref="InteractiveEnvironmentGrid"/><br/>
    /// Interaction logic for InteractiveEnvironmentFrame.xaml
    /// </summary>
    public partial class InteractiveEnvironmentFrame : UserControl
    {
        public InteractiveEnvironmentFrame()
        {
            InitializeComponent();

            
        }

        public InteractiveEnvironmentGrid Grid { get => prevGrid; }

        private void Grid_SelectedElementChanged(object sender, EventArgs e)
        {
            elemSelector.SurroundElement(Grid.GetContainerFromElement(Grid.SelectedElement));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            elemSelector.SurroundElement(Grid.GetContainerFromElement(Grid.SelectedElement));
        }
    }
}

// HTBD Organize the project with MVVM pattern