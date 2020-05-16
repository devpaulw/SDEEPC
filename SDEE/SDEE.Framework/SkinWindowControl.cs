using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using SDEE.CLI.Win32Lib;

namespace SDEE.Framework
{
    public abstract class SkinWindowControl : ContentControl
    {
        internal SkinnedWindow SkinnedWindow { get; private set; }

        public SkinWindowControl()
        {
            Initialized += OnInitialized;
        }

        private void OnInitialized(object sender, EventArgs e)
        {

            SkinnedWindow = new SkinnedWindow
            {
                AssociatedControl = this,
                Borders = PreviewAppRect.Margin,
                Title = Title
            };
        }

        public StringBuilder Title { get; } = new StringBuilder();
        public abstract Rectangle PreviewAppRect { get; }

        public Control Instance => this;

        public void ExecuteClose()
        {
            SkinnedWindow.Close();
        }

        public void ExecuteMinimize()
        {
            SkinnedWindow.Minimize();
        }

        public void ExecuteMaximize()
        {
            SkinnedWindow.Maximize();
        }

#if DEBUG
        public void ExecuteShowInfos()
        {
            SkinnedWindow.ShowInfos();
        }
#endif // DEBUG
    }
}
