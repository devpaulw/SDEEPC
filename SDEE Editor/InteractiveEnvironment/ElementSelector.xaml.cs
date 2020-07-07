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

namespace SDEE_Editor.InteractiveEnvironment
{
    /// <summary>
    /// Takes charge of selecting InteractiveEnvironmentGrid Selected Element with a surrounding rectangle
    /// </summary>
    public partial class ElementSelector : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(ElementSelector));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(ElementSelector));

        public Brush Color {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public double Size {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public double Gap { get; set; }

        //public Action RemoveSelectedElement { get; set; }


        public ElementSelector()
        {
            InitializeComponent();
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
                Rect elementBoundsRect = element
                    .TransformToVisual(canvas)
                    .TransformBounds(new Rect(element.RenderSize));

                double gap = surroundingRect.StrokeThickness + Gap;

                Canvas.SetLeft(surroundingRect, elementBoundsRect.Left - gap);
                Canvas.SetTop(surroundingRect, elementBoundsRect.Top - gap);

                surroundingRect.Visibility = Visibility.Visible;
                surroundingRect.Width = element.ActualWidth + gap * 2;
                surroundingRect.Height = element.ActualHeight + gap * 2;
            }
        }
    }
}