using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Win32;
using SDEE.CLI.Win32Lib;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Data;

namespace SDEE.Framework
{
    // DOLATER: Supply several screens
    public class DesktopEnvironment
    {
        readonly Application app = new Application();
        DEWin32Impl w32Impl = new DEWin32Impl();
        WindowsSkinManager windowsSkinManager;
        List<Window> floatingElemWindows = new List<Window>();
        bool isShutdown;

        public ContentControl Desktop { get; set; } = new ContentControl();
        public Collection<UIElement> FloatingElements { get; } = new Collection<UIElement>(); // ISSUE Can't be filled after DE.Run() executed
        public Func<SkinWindowControl> GetNewSkinWindowControl { get; set; }

        public DesktopEnvironment()
        {
            Closed += OnClosed;

            // Unhandled exception handling
            AppDomain.CurrentDomain.UnhandledException += (s, e) 
                => OnUnhandledException(e.ExceptionObject as Exception);
            app.DispatcherUnhandledException += (s, e) 
                => OnUnhandledException(e.Exception);
        }

        private void OnUnhandledException(Exception ex)
        {
            MessageBox.Show(
                string.Format("An exception as occurred {0}:\n\"{1}\"", ex.GetType(), ex.Message),
                "SDEE FATAL ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //Application.Current.Shutdown();
            Shutdown();
            Environment.Exit(0);
        }

        public event CancelEventHandler Closing;
        public event EventHandler Closed;

        public void Run()
        {
            //int w = User32.FindWindow(null, "7-Zip");//"Brainfucking machine [by Kacper 'KKKas' Kwapisz, 2006]");
            //if (w == 0)
            //    throw new Exception();
            //MessageBox.Show(User32.IsWindowUnicode((IntPtr)w).ToString());

            //int w = User32.FindWindow(null, "About Windows");//"Brainfucking machine [by Kacper 'KKKas' Kwapisz, 2006]");
            //if (w == 0)
            //    throw new Exception();

            //User32.SetWindowRgn((IntPtr)w, IntPtr.Zero, 1);

            desktopWindow = new Window
            {
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,

                Content = Desktop.Content ?? throw new NullReferenceException()
            };

            windowsSkinManager = new WindowsSkinManager(() => GetNewSkinWindowControl()
                .SkinnedWindow);

            app.Startup += App_Startup;
            app.Exit += (s, e) => Shutdown();
            desktopWindow.SourceInitialized += DesktopWindow_SourceInitialized;
            desktopWindow.ContentRendered += DesktopWindow_ContentRendered;
            desktopWindow.Closing += Closing;
            desktopWindow.Closed += Closed;

            isShutdown = false;

            app.Run();
        }


        private void App_Startup(object sender, StartupEventArgs e)
        {
            desktopWindow.Show();
        }

        private void DesktopWindow_ContentRendered(object sender, EventArgs e)
        {
            // Set Floating Elements as windows
            foreach (var fElem in FloatingElements)
            {
                Window wnd = DESC.ConvertElementToWindow(fElem, desktopWindow); // TODO Convert Element to Window do in W32 impl
                floatingElemWindows.Add(wnd);
                wnd.Closing += Closing;
                wnd.Closed += Closed;
                wnd.Show();
            }

            //DESC.InitWin32(desktopWindow);
            var wih = new WindowInteropHelper(desktopWindow);
            HwndSource source = HwndSource.FromHwnd(wih.Handle);

#if DEBUG
            FloatingElements.Add(new Label() { Content = source.Handle.ToString(), Foreground = Brushes.Black, HorizontalAlignment = HorizontalAlignment.Right });
#endif

            w32Impl.HookDesktop(source);
            windowsSkinManager.StartEngine(source);
        }

        private void DesktopWindow_SourceInitialized(object sender, EventArgs e)
        {
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Shutdown();
        }

        public void Shutdown()
        {
            if (isShutdown)
                return;
            isShutdown = true;

            w32Impl.UnhookDesktop();
            windowsSkinManager.StopEngine();

            desktopWindow.Close();
            foreach (var feWnd in floatingElemWindows)
            {
                feWnd.Close();
            }

            app.Shutdown();

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
