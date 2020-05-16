using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SDEE.Framework
{
    public partial class SizeGrip : SkinWindowElementControl
    {
        public static readonly DependencyProperty SizeableAreaProperty
            = DependencyProperty.Register("SizeableArea", typeof(Thickness), typeof(SizeGrip));

        public Thickness SizeableArea
        {
            get => (Thickness)GetValue(SizeableAreaProperty);
            set => SetValue(SizeableAreaProperty, value);
        }

        public SizeGrip()
        {
            InitializeComponent();
        }

        public SizeGrip(Thickness size) 
            : this() 
            => SizeableArea = size;

        bool resizeInProgress = false;
        Thickness gap;
        Point lastRightBottom;
        private void ResizeInit(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = (Rectangle)sender ?? throw new NullReferenceException();

            resizeInProgress = true;
            senderRect.CaptureMouse();

            // Get gap
            Point mPos = e.GetPosition(AncestorWindow);
            if (senderRect.HorizontalAlignment == HorizontalAlignment.Left)
                gap.Left = mPos.X;
            if (senderRect.VerticalAlignment == VerticalAlignment.Top)
                gap.Top = mPos.Y;
            if (senderRect.HorizontalAlignment == HorizontalAlignment.Right)
                gap.Right = AncestorWindow.Width - mPos.X;
            if (senderRect.VerticalAlignment == VerticalAlignment.Bottom)
                gap.Bottom = AncestorWindow.Height - mPos.Y;

            // Set minimum pos
            lastRightBottom = new Point(AncestorWindow.Left + AncestorWindow.Width, 
                lastRightBottom.Y = AncestorWindow.Top + AncestorWindow.Height);
        }

        private void ResizeEnd(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = (Rectangle)sender ?? throw new NullReferenceException();

            resizeInProgress = false;
            gap = default;
            senderRect.ReleaseMouseCapture();
        }

        private void Resizing(object sender, MouseEventArgs e)
        {
            if (resizeInProgress)
            {
                Rectangle senderRect = sender as Rectangle ?? throw new NullReferenceException();

                Point newMpos = e.GetPosition(AncestorWindow);

                double wndPWidth = AncestorWindow.Width,
                    wndPHeight = AncestorWindow.Height;

                if (senderRect.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    wndPWidth -= newMpos.X - gap.Left;

                    if (wndPWidth <= AncestorWindow.MinWidth)
                    {
                        AncestorWindow.Width = AncestorWindow.MinWidth;
                        AncestorWindow.Left = lastRightBottom.X - AncestorWindow.MinWidth;
                    }
                    else if (wndPWidth >= AncestorWindow.MaxWidth)
                    {
                        AncestorWindow.Width = AncestorWindow.MaxWidth;
                        AncestorWindow.Left = lastRightBottom.X - AncestorWindow.MaxWidth;
                    }
                    else
                    {
                        AncestorWindow.Width = wndPWidth;
                        AncestorWindow.Left += newMpos.X - gap.Left;
                    }
                }
                if (senderRect.VerticalAlignment == VerticalAlignment.Top)
                {
                    wndPHeight -= newMpos.Y - gap.Top;

                    if (wndPHeight <= AncestorWindow.MinHeight)
                    {
                        AncestorWindow.Height = AncestorWindow.MinHeight; // TODO Try with Win32 native direct resize, smoother
                        AncestorWindow.Top = lastRightBottom.Y - AncestorWindow.MinHeight;
                    }
                    else if (wndPHeight >= AncestorWindow.MaxHeight)
                    {
                        AncestorWindow.Height = AncestorWindow.MaxHeight;
                        AncestorWindow.Top = lastRightBottom.Y - AncestorWindow.MaxHeight;
                    }
                    else
                    {
                        AncestorWindow.Height = wndPHeight;
                        AncestorWindow.Top += newMpos.Y - gap.Top;
                    }
                }

                if (senderRect.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    wndPWidth = newMpos.X + gap.Right;

                    if (wndPWidth <= AncestorWindow.MinWidth)
                        AncestorWindow.Width = AncestorWindow.MinWidth;
                    else if (wndPWidth >= AncestorWindow.MaxWidth)
                        AncestorWindow.Width = AncestorWindow.MaxWidth;
                    else
                    {
                        AncestorWindow.Width = wndPWidth;
                        lastRightBottom.X = AncestorWindow.Left + AncestorWindow.Width; // right
                    }
                }
                if (senderRect.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    wndPHeight = newMpos.Y + gap.Bottom;

                    if (wndPHeight <= AncestorWindow.MinHeight)
                        AncestorWindow.Height = AncestorWindow.MinHeight;
                    else if (wndPHeight >= AncestorWindow.MaxHeight)
                        AncestorWindow.Height = AncestorWindow.MaxHeight;
                    else
                    {
                        AncestorWindow.Height = wndPHeight;
                        lastRightBottom.Y = AncestorWindow.Top + AncestorWindow.Height; // right
                    }
                }
            }
        }

        // HTBD POTENTIAL BUG: Bug with high resolution or multiple screen.

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool GetCursorPos(ref Win32Point pt);

        //[StructLayout(LayoutKind.Sequential)]
        //private struct Win32Point
        //{
        //    public int X;
        //    public int Y;
        //};
        //private static Point GetNativeMousePosition()
        //{
        //    Win32Point w32Mouse = new Win32Point();
        //    GetCursorPos(ref w32Mouse);
        //    return new Point(w32Mouse.X, w32Mouse.Y);
        //}


    }
}
