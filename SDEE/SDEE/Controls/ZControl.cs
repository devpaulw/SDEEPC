using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    /// <summary>
    /// Control with Z-Order support
    /// </summary>
    public abstract class ZControl : Control, IDisposable
    {
        // TODO MAybe make as if Zcontrol hadn't parent
        RenderWindow rWnd;

        public ZControl(DesktopEnvironment deParent) : base(deParent)
        {
            // DOLATER hide ZControls from Alt Tab
        }

        public override void Load()
        {
            rWnd = new RenderWindow(new VideoMode((uint)Size.X, (uint)Size.Y), null, Styles.None);
            rWnd.Position = AbsolutePosition;

            rWnd.Closed += (s, e) => rWnd.Close();

            rWnd.KeyPressed += (s, e) => OnKeyPressed(e);
            rWnd.MouseButtonPressed += (s, e) => OnMouseButtonPressed(e);// new MouseButtonEventArgs(new MouseButtonEvent() { X = e.X - Position.X, Y= e.Y - Position.Y }));
            rWnd.MouseMoved += (s, e) => OnMouseMoved(e);// new MouseMoveEventArgs(new MouseMoveEvent() { X = e.X - Position.X, Y = e.Y - Position.Y }));

            base.Load();
        }

        public void Display()
        {
            rWnd.DispatchEvents();
            rWnd.Clear(); // DOLATER And make it Win32 transparent by default
            rWnd.Draw(Drawing);
            rWnd.Display();
        }

        protected override void OnControlAdded(Control control)
        {
            //control.Position -= Position;// Put it back in the window, because positions are staggered

            base.OnControlAdded(control);
        }

        //private static Vector2i GetDEPosition(Vector2i zCtrlPosition)
        //{
        //    return new Vector2i(zCtrlPosition.X - )
        //}

        //private static Vector2i GetZCtrlPosition(Vector2i dePosition)
        //{

        //} 

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    rWnd.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
