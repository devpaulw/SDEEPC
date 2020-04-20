using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Win32;

namespace SDEE.Framework
{
    // DOLATER: Supply several screens
    public class DesktopEnvironment
    {
        readonly Application app = new Application();
        List<Window> floatingElemWindows = new List<Window>();

        public ContentControl Desktop { get; set; } = new ContentControl();
        public Collection<UIElement> FloatingElements { get; } = new Collection<UIElement>(); // ISSUE Can't be filled after DE.Run() executed
        public UIElement WindowsStyle { get; set; }

        public DesktopEnvironment()
        {
            Closed += OnClosed;
        }

        public event CancelEventHandler Closing;
        public event EventHandler Closed;

        public void Run()
        {
            desktopWindow = new Window
            {
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                Content = Desktop.Content ?? throw new NullReferenceException()
            };

            app.Startup += (s, e) =>
            {
                desktopWindow.Show();
            };

            desktopWindow.SourceInitialized += (s, e) =>
            {
                DESC.InitWin32(desktopWindow);
            };

            desktopWindow.ContentRendered += (s, e) =>
            {
                // Set Floating Elements as windows
                foreach (var fElem in FloatingElements)
                {
                    Window wnd = DESC.ConvertElementToWindow(fElem, desktopWindow);
                    floatingElemWindows.Add(wnd);
                    wnd.Closing += Closing;
                    wnd.Closed += Closed;
                    wnd.Show();
                }
            };

            desktopWindow.Closing += Closing;
            desktopWindow.Closed += Closed;

            app.Run();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            desktopWindow.Close();
            foreach (var feWnd in floatingElemWindows)
            {
                feWnd.Close();
            }
        }

        Window desktopWindow;

        private static class DESC // Desktop Environment SubClass
        {

            /// <summary>
            /// Convert a WPF element to a window with identical content, fully supports any controls.
            /// </summary>
            /// <param name="element">The WPF element</param>
            /// <param name="parentWindow">The parent window which should be the desktop window</param>
            /// <returns>A window that looks like a WPF element, with a linked close operation</returns>
            public static Window ConvertElementToWindow(UIElement element, Window parentWindow)
            {
                // TODO the window AltF4 should be linked

                Window wnd = new Window
                {
                    // Parameters
                    ShowInTaskbar = false,
                    AllowsTransparency = true,
                    Background = Brushes.Transparent, // TEST new SolidColorBrush(Color.FromArgb(40, 255, 255, 255)),
                    WindowStyle = WindowStyle.None, // Remove borders

                    // Size, position
                    Width = parentWindow.Width,
                    Height = parentWindow.Height,
                    Left = parentWindow.WindowState == WindowState.Maximized ? 0 : parentWindow.Left,
                    Top = parentWindow.WindowState == WindowState.Maximized ? 0 : parentWindow.Top,


                    // Opacity
                    Opacity = element.Opacity,

                    // Set Content
                    Content = element
                };

                // More config on window

                wnd.SourceInitialized += (s, e) =>
                {
                    User32.SetWindowLong(new WindowInteropHelper(wnd).Handle, User32.GWL_EXSTYLE, User32.WS_EX_TOOLWINDOW); // Set as Tool Window, window no longer in Alt-Tab
                };

                // UI Elem correction
                element.Opacity = 1d;
                return wnd;
            }

            //void SetWndToBackground()
            //{
            //    User.SetWindowPos(mainWindowWIH.Handle, HWND_NOTOPMOST, 0, 0, 0, 0,
            //        User.SWP_NOSIZE | User.SWP_NOMOVE | User.SWP_NOZORDER);// | User.SWP_NOOWNERZORDER);
            //}

            public static void InitWin32(Window window)
            {
                var wih = new WindowInteropHelper(window);
                IntPtr hWnd = wih.Handle;
                HwndSource source = HwndSource.FromHwnd(hWnd);
                var hsh = new HwndSourceHook(DesktopWndProc);
                source.AddHook(hsh);

                User32.SetWindowLong(hWnd, User32.GWL_EXSTYLE, User32.WS_EX_TOOLWINDOW); // Window no longer in Alt-Tab
            }

            private static IntPtr DesktopWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                switch (msg)
                {
                    case User32.WM_WINDOWPOSCHANGING:
                        unsafe
                        {
                            ((WINDOWPOS*)lParam)->flags |= User32.SWP_NOZORDER;
                        }
                        break;
                }
                return IntPtr.Zero;
            }
        }
    }
}
