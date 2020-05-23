using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace SDEE_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                if (sender is TreeViewItem tvi)
                    if (tvi.Tag is UIElement elem)
                    {
                        DataObject dObj = new DataObject(typeof(UIElement), elem);
                        DragDrop.DoDragDrop(tvi, dObj, DragDropEffects.Copy);
                    }
        }

        private void Toolbox_ElementClicked(object sender, FrameworkElement e)
        {
            prevEnv.AddElement(e);
        }

        //UIElement previewDraggingElem;

        //private void previewGrid_DragEnter(object sender, DragEventArgs e)
        //{
        //    //if (sender is Grid previewGrid)
        //    //{
        //    //    if (e.Data.GetDataPresent(typeof(UIElement)))
        //    //    {
        //    //        UIElement elem = e.Data.GetData(typeof(UIElement)) as UIElement;
        //    //        if (elem != null)
        //    //        {
        //    //            previewGrid.Children.Add(elem);
        //    //            previewDraggingElem = elem;
        //    //        }
        //    //    }
        //    //}
        //}

        //private void previewGrid_DragOver(object sender, DragEventArgs e)
        //{
        //    e.Effects = DragDropEffects.Copy;
        //    //e.Effects = previewDraggingElem == null ? DragDropEffects.None : DragDropEffects.Copy;
        //}

        //private void previewGrid_DragLeave(object sender, DragEventArgs e)
        //{
        //    //if (previewDraggingElem != null)
        //    //{
        //    //    Grid previewGrid = sender as Grid;
        //    //    previewGrid.Children.Remove(previewDraggingElem);
        //    //    previewDraggingElem = null;
        //    //}
        //}

        //private void PreviewGrid_Drop(object sender, DragEventArgs e)
        //{
        //    //previewDraggingElem = null;
        //}

        //private void previewGrid_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        //{

        //}
    }
}
