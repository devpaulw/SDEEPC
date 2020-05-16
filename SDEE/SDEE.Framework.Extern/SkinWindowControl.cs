using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SDEE.Framework.Extern
{
    public abstract class SkinWindowControl : UIElement
    {
        //public static readonly DependencyProperty TitleProperty
        //    = DependencyProperty.Register("Title", typeof(string), typeof(SkinWindowControl), new UIPropertyMetadata(string.Empty));

        public string Title { get; set; } = "Unknown";
        public abstract Rectangle PreviewAppRect { get; }

        // TODO Put DesktopEnvironment
        public void ExecuteClose()
        {

        }

        public void ExecuteMinimize()
        {

        }

        public void ExecuteMaximize()
        {
        }

        //internal SDEE.CLI.Win32Lib.SkinWindowControlImpl ConvertToImpl()
        //{
        //    return new SDEE.CLI.Win32Lib.SkinWindowControlImpl
        //    {
        //        Title = Title,
        //        AssociatedControl = this,
        //        Borders = PreviewAppRect.Margin
        //    };
        //}
    }
}
