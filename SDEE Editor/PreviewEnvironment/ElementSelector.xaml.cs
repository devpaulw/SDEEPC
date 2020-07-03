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

namespace SDEE_Editor.PreviewEnvironment
{
    /// <summary>
    /// Takes charge of selecting PreviewEnvironmentFrame Selected Element with a surrounding rectangle
    /// </summary>
    public partial class PreviewEnvironmentElementSelector : UserControl
    {

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(PreviewEnvironmentElementSelector));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(PreviewEnvironmentElementSelector));
        public static readonly DependencyProperty PreviewEnvironmentFrameProperty = DependencyProperty.Register("PreviewEnvironmentFrame", typeof(PreviewEnvironmentFrame), typeof(PreviewEnvironmentElementSelector));

        public Brush Color {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public double Size {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        public PreviewEnvironmentFrame PreviewEnvironmentFrame
        {
            get => (PreviewEnvironmentFrame)GetValue(PreviewEnvironmentFrameProperty);
            set => SetValue(PreviewEnvironmentFrameProperty, value);
        }

        public double Gap { get; set; }

        //public Action RemoveSelectedElement { get; set; }


        public PreviewEnvironmentElementSelector()
        {
            InitializeComponent();
        }

        // TODO When loaded, check if values DPs have been filled

        public PreviewEnvironmentElementSelector(Brush color, double size, double gap) : this()
        {
            Color = color;
            Size = size;
            Gap = gap;
        }



        public void SurroundElement(FrameworkElement element)
        {
            if (element == null) // Is going to be deselected, unsurrounded
            {
                if (surroundingRect.Visibility == Visibility.Visible)
                {
                    surroundingRect.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                surroundingRect.Visibility = Visibility.Visible;
                surroundingRect.HorizontalAlignment = element.HorizontalAlignment;
                surroundingRect.VerticalAlignment = element.VerticalAlignment;
                surroundingRect.Margin = element.Margin;
                surroundingRect.Width = element.Width + surroundingRect.StrokeThickness + Gap;
                surroundingRect.Height = element.Height + surroundingRect.StrokeThickness + Gap;

                //Focus(); // TEMP !
            }
        }

        //protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    // TODO Enable this to be always focused when PE focused too
        //    base.OnPreviewMouseLeftButtonDown(e); 

        //    Focus();// Focus for key inputs
        //}

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);

        //    if (e.Key == Key.Delete)
        //        RemoveSelectedElement?.Invoke();
        //}

        //private void ContextMenuRemoveMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (RemoveSelectedElement == null)
        //        throw new NullReferenceException("The remove element operation wasn't linked to any delegate.");
        //    else
        //    RemoveSelectedElement.Invoke();
        //}
    }
}