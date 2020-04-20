using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win32;

namespace SDEE
{
    class WindowCustomizer
    {
        private Win32Window w32wnd;

        public WindowCustomizer(IntPtr hWnd)
        {
            w32wnd = new Win32Window(hWnd);
        }

        public void StartCustomization(IntPtr ch)
        {
            //w32wnd.RemoveOverlap();

            Vector2i customSize = w32wnd.GetSize() + new Vector2i(50, -50);
            Vector2i customPos = w32wnd.GetPosition();

            //RectangleShape shape = new RectangleShape()
            //{
            //    Size = (Vector2f)(wndSize + new Vector2i(20, 20)),
            //    Position = (Vector2f)wndPos,
            //    FillColor = Color.Green
            //};

            MouseMoveEvent oldMouseMoveEvent = new MouseMoveEvent();
            new Thread(() =>
            {
                using (RenderWindow window = new RenderWindow(
                    new VideoMode((uint)customSize.X, (uint)customSize.Y),
                    null,
                    Styles.None))
                {
                    //User.SetWindowLong(w32wnd.Handle, User.GWL_HWNDPARENT, (int)window.SystemHandle);
                    User32.SetWindowLong(w32wnd.Handle, User32.GWL_STYLE, User32.GetWindowLong(w32wnd.Handle,User32.GWL_STYLE) | User32.WS_CLIPSIBLINGS | User32.WS_CHILD);
                    User32.SetParent(w32wnd.Handle, ch);
                    User32.ShowWindow(w32wnd.Handle, User32.SW_SHOWNORMAL);

                    window.Position = customPos;

                    window.MouseMoved += OnMouseMoved;

                    //while (window.IsOpen)
                    //{
                    //    window.DispatchEvents();
                    //    window.Clear(Color.Green);
                    //    //window.Draw(shape);
                    //    window.Display();
                    //}



                    void OnMouseMoved(object sender, MouseMoveEventArgs _)
                    {
                        User32.GetCursorPos(out POINT p);
                        MouseMoveEvent e = new MouseMoveEvent() { X = p.x, Y = p.y };

                        int deltaX = e.X - oldMouseMoveEvent.X,
                        deltaY = e.Y - oldMouseMoveEvent.Y;

                        if (Mouse.IsButtonPressed(Mouse.Button.Left)
                            && e.X >= window.Position.X && e.X <= window.Position.X + window.Size.X
                            && e.Y >= window.Position.Y && e.Y <= window.Position.Y + window.Size.Y)
                        {
                            Vector2i newPos = window.Position + new Vector2i(deltaX, deltaY);
                            //w32wnd.SetPosition(newPos);
                            window.Position = newPos;
                        }

                        oldMouseMoveEvent = new MouseMoveEvent() { X = e.X, Y = e.Y };
                    }
                }
            }).Start();
        }

        class Win32Window
        {
            public IntPtr Handle { get; }
            public Win32Window(IntPtr hWnd)
            {
                Handle = hWnd;
            }

            public void RemoveOverlap()
            {
                User32.SetWindowLong(Handle, User32.GWL_STYLE,
                    User32.GetWindowLong(Handle, User32.GWL_STYLE) & ~User32.WS_OVERLAPPEDWINDOW);
            }

            public Vector2i GetSize()
            {
                RECT rect = new RECT();
                User32.GetWindowRect(Handle, ref rect);
                return new Vector2i(rect.Right - rect.Left, rect.Bottom - rect.Top);
            }

            public Vector2i GetPosition()
            {
                RECT rect = new RECT();
                User32.GetWindowRect(Handle, ref rect);
                return new Vector2i(rect.Left, rect.Top);
            }

            public void SetPosition(Vector2i position)
            {
                User32.SetWindowPos(Handle, new User32().HWND_TOPMOST, position.X, position.Y, 0, 0, User32.SWP_NOSIZE); // WARNING this make always the windows on top, i should request a Z-order index
            }
        }
    }

}
