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

namespace SDEE_Editor
{
    public partial class SurrounderRect : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(SurrounderRect));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(SurrounderRect));

        public Brush Color {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public double Size {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public double Gap { get; set; }

        public Action RemoveSelectedElement { get; set; }

        public SurrounderRect()
        {
            InitializeComponent();
        }

        public SurrounderRect(Brush color, double size, double gap) : this()
        {
            Color = color;
            Size = size;
            Gap = gap;
        }

        public void SurroundElement(FrameworkElement elem)
        {
            if (elem == null)
            {
                if (surroundingRect.Visibility == Visibility.Visible)
                {
                    surroundingRect.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                surroundingRect.Visibility = Visibility.Visible;
                surroundingRect.HorizontalAlignment = elem.HorizontalAlignment;
                surroundingRect.VerticalAlignment = elem.VerticalAlignment;
                surroundingRect.Margin = elem.Margin;
                surroundingRect.Width = elem.Width + surroundingRect.StrokeThickness + Gap;
                surroundingRect.Height = elem.Height + surroundingRect.StrokeThickness + Gap;
            }
        }

        //protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    // TODO Enable this to be always focused when PE focused too
        //    base.OnPreviewMouseLeftButtonDown(e); 

        //    Focus();// Focus for key inputs
        //}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Delete)
                RemoveSelectedElement?.Invoke();
        }

        private void ContextMenuRemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelectedElement?.Invoke();
        }
    }
}