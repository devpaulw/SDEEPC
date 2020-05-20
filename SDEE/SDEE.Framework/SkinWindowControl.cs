using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using SDEE.CLI.Win32Lib;

namespace SDEE.Framework
{
    /// <summary>
    /// The control that will be overriden by Skinned Windows
    /// </summary>
    public abstract class SkinWindowControl : ContentControl
    {
        internal SkinnedWindow SkinnedWindow { get; private set; }

        public SkinWindowControl()
        {
            Initialized += OnInitialized;
        }


        protected override void OnInitialized(EventArgs e)
        {

            SkinnedWindow = new SkinnedWindow
            {
                AssociatedControl = this,
                Borders = PreviewAppRect.Margin,
            };
            SkinnedWindow.Initialized += SkinnedWindow_Initialized;
            base.OnInitialized(e);
        }
        private void OnInitialized(object sender, EventArgs e)
        {
        }

        private void SkinnedWindow_Initialized(object sender, EventArgs e)
        {
            Title = SkinnedWindow.Title;
            CanMaximize = SkinnedWindow.CanMaximize;
            CanMinimize = SkinnedWindow.CanMinimize;
            CanClose = SkinnedWindow.CanClose;
        }

        public abstract Rectangle PreviewAppRect { get; }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SkinWindowControl));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public bool IsMaximized => SkinnedWindow.IsMaximized;
        public bool CanMaximize { get; private set; }
        public bool CanMinimize { get; private set; }
        public bool CanClose { get; private set; }

        public void ExecuteClose()
        {
            if (CanClose)
                SkinnedWindow.Close();
        }

        public void ExecuteMinimize()
        {
            if (CanMinimize)
                SkinnedWindow.Minimize();
        }

        public void ExecuteMaximize()
        {
            if (CanMaximize)
                SkinnedWindow.Maximize();
        }

        public void ExecuteRestore()
        {
            if (CanMaximize)
                SkinnedWindow.Restore();
        }

        public void ToggleMaximize()
        {
            if (SkinnedWindow.IsMaximized)
                ExecuteRestore();
            else
                ExecuteMaximize();
        }

#if DEBUG
        public void ExecuteShowInfos()
        {
            SkinnedWindow.ShowInfos();
        }
#endif // DEBUG
    }
}
