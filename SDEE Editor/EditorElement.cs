using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SDEE_Editor
{
    /// <summary>
    /// Represents an element that is a FrameworkElement that is not HitTestVisible but still supports Mouse and Keyboard input.
    /// </summary>
    public class EditorElement : ContentControl
    {
        // HTBD Do a click event

        public FrameworkElement ElementValue { get;}

        public EditorElement(string name, FrameworkElement elementValue)
        {
            ElementValue = elementValue ?? throw new ArgumentNullException(nameof(elementValue));
            Name = name.Length > 0 ? name : throw new ArgumentOutOfRangeException(nameof(name), "The name is too small.");

            // HERE Be careful, this might not work well:
            HorizontalAlignment = ElementValue.HorizontalAlignment;
            VerticalAlignment = ElementValue.VerticalAlignment;
            Margin = ElementValue.Margin;
            Width = ElementValue.Width;
            Height = ElementValue.Height;
            Visibility = ElementValue.Visibility;

            Grid contentGrid = new Grid();

            Rectangle inputDetectionRect = new Rectangle
            {
                Fill = Brushes.Transparent// Invisible, is just for virtual hit detection
            };

            // Disable run-time features on the control added
            ElementValue.IsHitTestVisible = false;

            contentGrid.Children.Add(ElementValue);
            //contentGrid.Children.Add(inputDetectionRect); // TEMP please restore

            Content = contentGrid;
        }
    }
}
