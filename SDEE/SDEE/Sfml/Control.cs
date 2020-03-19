using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE.Sfml
{
    abstract class Control : Drawable
    {
        protected abstract Shape Shape { get; }

        public Vector2i Position { get; set; }
        public Vector2i Size { get; set; }

        public ControlCollection Controls { get; }
        public Control Parent { get; set; }
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

        public Control() 
        {
            Controls = new ControlCollection(this);
        }

        //public new void Draw(RenderTarget target, RenderStates states)
        //{
        //    base.Draw(target, states);

        //    foreach (var child in Children)
        //        target.Draw(child);
        //}

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
                }
                catch (Win32Exception ex) // Should appear when execution cancelled
                {
                    MessageBox(ex.Message, ex.NativeErrorCode.ToString(), MessageBoxIcon.Exclamation);
                }

                //while (!process.HasExited) // FOR LATER
                //{
                //    process.Refresh();
                //    if (process.MainWindowHandle.ToInt32() != 0)
                //    {
                //        IntPtr hWnd = process.MainWindowHandle;
                //        break;
                //    }
                //}
            }

        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Shape != null)
                target.Draw(Shape);

            foreach (var control in Controls)
                target.Draw(control);
        }

        /// <summary>
        /// Deeply initializes every controls so that they get filled events from DE
        /// </summary>
        protected virtual void Init()
        {
            if (DeskEnv == null)
                throw new NoDEImplementedException();

            #region Assign event to virtual functions
            KeyPressed += (s, e) => OnKeyPressed(e);
            MouseButtonPressed += (s, e) => OnMouseButtonPressed(e);
            Click += (s, e) => OnClick(e);
            #endregion

            #region Get DE Events
            DeskEnv.KeyPressed += KeyPressed;
            DeskEnv.MouseButtonPressed += MouseButtonPressed;
            #endregion

            #region Init Children
            foreach (Control child in Controls)
                child.Init();
            #endregion

            InitChildren();
        }


        protected virtual void InitChildren()
        {
            foreach (Control child in Controls)
                child.InitChildren();
        }

        protected virtual void OnKeyPressed(KeyEventArgs e) { }
        protected virtual void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            if (e.X >= Position.X && e.X <= Position.X + Size.X
                && e.Y >= Position.Y && e.Y <= Position.Y + Size.Y)
            {
                Click(this, e);
            }
        }

        protected virtual void OnClick(MouseButtonEventArgs e) { }

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
        public event EventHandler<MouseButtonEventArgs> Click;
    }

}
