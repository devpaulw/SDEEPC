using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE
{
    class DesktopEnvironment
    {
        public Collection<Drawable> GuiElements { get; } = new Collection<Drawable>();
        public Color Wallpaper { get; set; }

        public DesktopEnvironment(Color wallpaper)
        {
            Wallpaper = wallpaper;
        }

        public void Start()
        {
            using (RenderWindow window = new RenderWindow(VideoMode.DesktopMode, null, Styles.Default))
            {
                #region Win32 conf
                IntPtr hWnd = window.SystemHandle;
                //User.SetWindowLong(hWnd, User.GWL_EXSTYLE, User.GetWindowLong(hWnd, User.GWL_EXSTYLE) & ~User.WS_EX_TOPMOST);
                //User.SetWindowLong(hWnd,
                  //  User.GWL_STYLE, User.GetWindowLong(hWnd, User.GWL_STYLE) & ~User.WS_OVERLAPPEDWINDOW);
                //User.SetWindowPos(hWnd, new IntPtr(1), 0, 0, 0, 0, User.SWP_NOSIZE | User.SWP_NOMOVE | User.SWP_NOACTIVATE);

                #endregion

                window.Closed += (s, e) => window.Close();

                MainLoop();

                void MainLoop()
                {
                    while (window.IsOpen)
                    {
                        window.DispatchEvents();
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
            }
        }
    }
}
