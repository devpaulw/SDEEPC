using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE
{
    class Window : Control
    {
        private Win32Window w32wnd;
        private MouseMoveEvent oldMouseMoveEvent;

        private protected override Shape Shape
            => new RectangleShape()
            {
                FillColor = Color.Red
            };

        public Window(DesktopEnvironment parent, IntPtr hWnd) : base(parent)
        {
            w32wnd = new Win32Window(hWnd);
            w32wnd.RemoveOverlap();
            Position = w32wnd.GetPosition() - new Vector2i(20, 20);
            Size = w32wnd.GetSize() + new Vector2i(40, 40); // tmpf

            SetZ(ZOrder.Top);

            oldMouseMoveEvent = new MouseMoveEvent() { X = Mouse.GetPosition().X, Y = Mouse.GetPosition().Y }; // WARNING This could contain bugs if Loaded a long time before env dispatch events
        }

        protected override void OnMouseMoved(MouseMoveEventArgs e)
        {
            User32.GetCursorPos(out POINT p);
            MouseMoveEvent we = new MouseMoveEvent() { X = p.x, Y = p.y };

            int deltaX = we.X - oldMouseMoveEvent.X,
                deltaY = we.Y - oldMouseMoveEvent.Y;

            if (Mouse.IsButtonPressed(Mouse.Button.Left)
                && we.X >= Position.X && we.X <= Position.X + Size.X // DOLATER Create another way to go faster
                && we.Y >= Position.Y && we.Y <= Position.Y + Size.Y)
            {
                Vector2i newPos = Position + new Vector2i(deltaX, deltaY);

                Position = newPos;
                w32wnd.SetPosition(newPos);
            }

            oldMouseMoveEvent = new MouseMoveEvent() { X = we.X, Y = we.Y };

            base.OnMouseMoved(e);
        }

        protected override void OnClick(MouseButtonEventArgs e)
        {
            SetZ(ZOrder.Top);

            base.OnClick(e);
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
