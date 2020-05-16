using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SDEE.Framework
{
    public class SkinWindowElementControl : ContentControl
    {
        protected static readonly DependencyProperty AncestorWindowProperty
            = DependencyProperty.Register("AncestorWindow", typeof(Window), typeof(SkinWindowElementControl));

        protected Window AncestorWindow {
            get => (Window)GetValue(AncestorWindowProperty);
            set => SetValue(AncestorWindowProperty, value);
        } // TODO Try with protected, or just set, private set...

        public SkinWindowElementControl()
        {
            Loaded += SkinWindowElement_Loaded;
        }

        private void SkinWindowElement_Loaded(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding();
            RelativeSource source = new RelativeSource(RelativeSourceMode.FindAncestor);
            binding.RelativeSource = source;
            source.AncestorType = typeof(Window);
            SetBinding(AncestorWindowProperty, binding);
        }
    }
}
