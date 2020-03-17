using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;
using static Win32.User;

namespace SDEE
{
    class SfmlDesktopEnvironment : DesktopEnvironment<Drawable>
    {
        List<IntPtr> hWnds = new List<IntPtr>();

        public Color Wallpaper { get; set; }

        public SfmlDesktopEnvironment(Color wallpaper) : base()
        {
            Wallpaper = wallpaper;
        }

        public override void Start()
        {
            using (RenderWindow window = new RenderWindow(VideoMode.DesktopMode, null, Styles.Default))
            {
                #region Win32 conf
                const int WS_EX_TOOLWINDOW = 0x80,
                    WS_EX_APPWINDOW = 0x40000;
                User.SetWindowLong(window.SystemHandle, User.GWL_EXSTYLE,
                    User.GetWindowLong(window.SystemHandle, User.GWL_EXSTYLE) & ~WS_EX_APPWINDOW | WS_EX_TOOLWINDOW); // Remove window from Alt Tab

                //User.SetWindowLong(hWnd, User.GWL_EXSTYLE, User.GetWindowLong(hWnd, User.GWL_EXSTYLE) & ~User.WS_EX_TOPMOST);
                //User.SetWindowLong(hWnd, User.GWL_STYLE, User.GetWindowLong(hWnd, User.GWL_STYLE) & ~User.WS_OVERLAPPEDWINDOW);
                //User.SetWindowPos(hWnd, new IntPtr(1), 0, 0, 0, 0, User.SWP_NOSIZE | User.SWP_NOMOVE | User.SWP_NOACTIVATE);


                //User.SetForegroundWindow(testHWnd);

                //var testHWnd = new IntPtr(User.FindWindow(null, "Outil Capture"));
                //User.SetWindowLong(testHWnd, User.GWL_EXSTYLE, 
                //    User.WS_EX_TOPMOST);
                //Console.WriteLine(testHWnd);

                //User.SetForegroundWindow(testHWnd);

                #endregion

                window.Closed += (s, e) => window.Close();
                window.KeyPressed += KeyPressed;

                MainLoop();

                void MainLoop()
                {
                    MSG msg = new MSG();

                    while (window.IsOpen)
                    {
                        window.DispatchEvents();

                        if (PeekMessage(ref msg, window.SystemHandle, 0, 0, PM_REMOVE) != 0) 
                        {
                            Console.WriteLine(msg.message);
                        }

                        //User.SetWindowPos(window.SystemHandle, new User().HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE |SWP_NOZORDER);
                        //BringWindowsOnTop();
                        //SetWindowPos(window.SystemHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER);

                        window.Clear(Wallpaper);

                        DrawGuiElements();

                        window.Display();
                    }

                    void DrawGuiElements()
                    {
                        foreach (var guiElement in GuiElements)
                        {
                            window.Draw(guiElement);
                        }
                    }

                    void BringWindowsOnTop()
                    {
                        foreach (IntPtr hWnd in hWnds)
                        {
                            BringWindowToTop(hWnd);
                            //SetWindowPos(hWnd, window.SystemHandle, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER);
                            //Console.WriteLine(hWnd);
                            //User.SetWindowLong(testHWnd, User.GWL_EXSTYLE, User.WS_EX_TOPMOST);
                        }
                    }
                }

                void KeyPressed(object sender, KeyEventArgs e)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (e.Code == Keyboard.Key.K)
                        {
                            using (Process process = new Process())
                            {
                                process.StartInfo.FileName = @"C:\Windows\notepad.exe";
                                process.Start();

                                //User.SetWindowLong(process.MainWindowHandle, User.GWL_EXSTYLE, User.WS_EX_TOPMOST);
                                
                                while (!process.HasExited)
                                {
                                    process.Refresh();
                                    if (process.MainWindowHandle.ToInt32() != 0)
                                    {
                                        hWnds.Add(process.MainWindowHandle);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
