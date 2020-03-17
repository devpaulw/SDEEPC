using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
            using (var window = new SfW32DEWindow())
            {
                window.Closed += (s, e) => window.Close();
                window.KeyPressed += KeyPressed;

                MainLoop();

                void MainLoop()
                {
                    while (window.IsOpen)
                    {
                        window.DispatchSystemMessage();
                        window.DispatchEvents();
                        //Console.WriteLine("GM " + DateTime.Now.Ticks);
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
                }

                void KeyPressed(object sender, KeyEventArgs e)
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
