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

        public Action<FrameworkElement> RemoveElement { get; set; }

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
        private FrameworkElement surroundedElement;
        public void SurroundElement(FrameworkElement elem)
        {
            // Make selection outline

            if (elem == null)
                throw new NullReferenceException("An invalid element tried to be surrounded.");

            surroundingRect.Visibility = Visibility.Visible;
            surroundingRect.HorizontalAlignment = elem.HorizontalAlignment;
            surroundingRect.VerticalAlignment = elem.VerticalAlignment;
            surroundingRect.Margin = elem.Margin;
            surroundingRect.Width = elem.Width + surroundingRect.StrokeThickness + Gap;
            surroundingRect.Height = elem.Height + surroundingRect.StrokeThickness + Gap;

            this.Focus(); // Focus for key inputs

            surroundedElement = elem;
        }

        /// <summary>
        /// Unsurround the surrounded element (if any selected)
        /// </summary>
        public void UnsurroundSurroundedElement()
        {
            if (surroundingRect.Visibility == Visibility.Visible)
            {
                surroundingRect.Visibility = Visibility.Collapsed;
                surroundedElement = null;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Delete)
                RemoveElement(surroundedElement);
        }

        private void ContextMenuRemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveElement(surroundedElement);
        }
    }
}
