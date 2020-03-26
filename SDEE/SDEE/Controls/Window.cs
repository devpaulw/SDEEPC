//using SFML.Graphics;
//using SFML.System;
//using SFML.Window;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Win32;

//namespace SDEE
//{
//    class Window : Control
//    {
//        private Win32Window w32wnd;
//        private MouseMoveEvent oldMouseMoveEvent;

//        public override Shape Shape
//            => new RectangleShape(this.GetBasicShape())
//            {
//                FillColor = Color.Red
//            };

//        public Window(DesktopEnvironment parent, IntPtr hWnd) : base(parent)
//        {
//            w32wnd = new Win32Window(hWnd);
//            w32wnd.RemoveOverlap();
//            Position = w32wnd.GetPosition();
//            Size = w32wnd.GetSize() + new Vector2i(20, 20); // tmpf
//        }

//        public override void Load()
//        {
//            oldMouseMoveEvent = new MouseMoveEvent() { X = Mouse.GetPosition().X, Y = Mouse.GetPosition().Y }; // WARNING This could contain bugs if Loaded a long time before env dispatch events

//            base.Load();
//        }

//        protected override void OnMouseMoved(MouseMoveEventArgs e)
//        {
//            int deltaX = e.X - oldMouseMoveEvent.X,
//                deltaY = e.Y - oldMouseMoveEvent.Y;

//            if (Mouse.IsButtonPressed(Mouse.Button.Left)
//                && e.X >= Position.X && e.X <= Position.X + Size.X // DOLATER Create another way to go faster
//                && e.Y >= Position.Y && e.Y <= Position.Y + Size.Y)
//            {
//                Vector2i newPos = Position + new Vector2i(deltaX, deltaY);
//                w32wnd.SetPosition(newPos);

//                Position = newPos;
//            }

//            oldMouseMoveEvent = new MouseMoveEvent() { X = e.X, Y = e.Y };

//            base.OnMouseMoved(e);
//        }

//        class Win32Window
//        {
//            public IntPtr Handle { get; }
//            public Win32Window(IntPtr hWnd)
//            {
//                Handle = hWnd;
//            }

//            public void RemoveOverlap()
//            {
//                User.SetWindowLong(Handle, User.GWL_STYLE,
//                    User.GetWindowLong(Handle, User.GWL_STYLE) & ~User.WS_OVERLAPPEDWINDOW);
//            }

//            public Vector2i GetSize()
//            {
//                RECT rect = new RECT();
//                User.GetWindowRect(Handle, ref rect);
//                return new Vector2i(rect.Right - rect.Left, rect.Bottom - rect.Top);
//            }

//            public Vector2i GetPosition()
//            {
//                RECT rect = new RECT();
//                User.GetWindowRect(Handle, ref rect);
//                return new Vector2i(rect.Left, rect.Top);
//            }

//            public void SetPosition(Vector2i position)
//            {
//                User.SetWindowPos(Handle, new User().HWND_TOP, position.X, position.Y, 0, 0, User.SWP_NOSIZE); // WARNING this make always the windows on top, i should request a Z-order index
//            }
//        }
//    }
//}
