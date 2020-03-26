using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE
{
    public abstract class Control
    {
        public Control(Control parent)
        {
            Controls = new ControlCollection(this);

            Parent = parent;

            if (DeskEnv == null)
                throw new NoDEImplementedException();
        }

        /// <summary>
        /// Position of the control relative to the parent
        /// </summary>
        public Vector2i Position { get; set; }
        public Vector2i Size { get; set; }
        public bool IsEnabled { get; set; } = true;

        public ControlCollection Controls { get; }
        public Control Parent { get; }

        /// <summary>
        /// Position of the control relative to the Desktop Environment or ZControl
        /// </summary>
        public Vector2i AbsolutePosition {
            get {
                Control parent = this;
                Vector2i sp = default;
                while (parent != null)
                {
                    sp += parent.Position;
                    parent = parent.Parent;
                }
                return sp;
            }
        }

        public DesktopEnvironment DeskEnv {
            get {
                Control parent = this;
                while (parent != null)
                    if (parent is DesktopEnvironment de)
                        return de;
                    else
                        parent = parent.Parent;
                return null;
            }
        }
        public abstract ControlDrawing Drawing { get; }

        public void MessageBox(string text, string caption, MessageBoxIcon icon) =>
            User.MessageBox(IntPtr.Zero, text, caption, (int)icon);

        public void StartExe(string executablePath)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = executablePath;

                try
                {
                    process.Start();

                    //while (!process.HasExited) // FOR LATER
                    //{
                    //    process.Refresh();
                    //    if (process.MainWindowHandle.ToInt32() != 0)
                    //    {
                    //        IntPtr hWnd = process.MainWindowHandle;
                    //        //DeskEnv.Controls.Add(new Window(DeskEnv, hWnd));
                    //        new WindowCustomizer(hWnd).StartCustomization();
                    //        break;
                    //    }
                    //}
                }
                catch (Win32Exception) // Should appear when execution cancelled
                {
                    // DO NOTHING (not important)
                    //MessageBox(ex.Message, ex.NativeErrorCode.ToString(), MessageBoxIcon.Exclamation);
                }
            }

        }

        /// <summary>
        /// TOFILL
        /// </summary>
        public virtual void Load()
        {
            if (Parent == null || this is ZControl)
                return;

            Parent.MouseButtonPressed += (s, e) => OnMouseButtonPressed(new MouseButtonEventArgs(new MouseButtonEvent() { X = e.X - Position.X, Y = e.Y - Position.Y })); ;
            Parent.KeyPressed += (s, e) => OnKeyPressed(e);
            Parent.MouseMoved += (s, e) => OnMouseMoved(e);
        }

        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
            if (!IsEnabled)
                return;

            KeyPressed?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
        }
        protected virtual void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (e.X >= 0 && e.X <= Size.X
                && e.Y >= 0 && e.Y <= Size.Y
                && e.Button == Mouse.Button.Left)
            {
                OnClick(e);
            }

            MouseButtonPressed?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
        }
        protected virtual void OnClick(MouseButtonEventArgs e)
        {
            if (!IsEnabled)
                return;

            Click?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
        }
        protected virtual void OnControlAdded(Control control)
        {
            if (!IsEnabled)
                return;

            ControlAdded?.Invoke(this, control ?? throw new ArgumentNullException(nameof(control)));
        }
        protected virtual void OnMouseMoved(MouseMoveEventArgs e)
        {
            if (!IsEnabled)
                return;

            MouseMoved?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
        }

        internal void RaiseControlAdded(Control controlEventArgs) => OnControlAdded(controlEventArgs);

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
        public event EventHandler<MouseMoveEventArgs> MouseMoved;
        public event EventHandler<MouseButtonEventArgs> Click;
        public event EventHandler<Control> ControlAdded;
    }
}
