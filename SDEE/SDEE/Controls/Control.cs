using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE
{
    public abstract partial class Control : Drawable
    {
        protected virtual Shape Shape { get => null; }
        public virtual ControlType Type { get => ControlType.NotSavable; }
        public virtual Dictionary<string, string> XmlAttributes { get => new Dictionary<string, string>(); }

        public Vector2i Position { get; set; }
        public Vector2i Size { get; set; }
        public bool IsEnabled { get; set; } = true;

        public ControlCollection Controls { get; }
        public Control Parent { get; }
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

        public Control(Control parent)
        {
            Controls = new ControlCollection(this);
            Parent = parent;

            if (DeskEnv == null)
                throw new NoDEImplementedException();
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
            if (!IsEnabled)
                return;

            if (Shape != null)
                target.Draw(Shape);

            foreach (var control in Controls)
                target.Draw(control);
        }

        /// <summary>
        /// Once DE started and every controls linked, there might be other things to do.
        /// Deeply initializes every controls so that they get filled events from DE
        /// </summary>
        protected virtual void Load() // TODO Is this method really useful or do I let it only in DE?
        {
            #region Init Children
            foreach (Control child in Controls)
                child.Load();
            #endregion

            //InitChildren();
        }


        //protected virtual void InitChildren()
        //{
        //    foreach (Control child in Controls)
        //        child.InitChildren();
        //}

        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
            if (!IsEnabled)
                return;

            KeyPressed?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
            foreach (var child in Controls) child.OnKeyPressed(e);
        }
        protected virtual void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (e.X >= Position.X && e.X <= Position.X + Size.X
                && e.Y >= Position.Y && e.Y <= Position.Y + Size.Y
                && e.Button == Mouse.Button.Left)
            {
                OnClick(e);
            }

            MouseButtonPressed?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
            foreach (var child in Controls) child.OnMouseButtonPressed(e);
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

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
        public event EventHandler<MouseButtonEventArgs> Click;
        public event EventHandler<Control> ControlAdded;



        //public event EventHandler<KeyEventArgs> KeyPressed {
        //    add => DeskEnv.KeyPressed += value;
        //    remove => DeskEnv.KeyPressed -= value;
        //}
        //public event EventHandler<MouseButtonEventArgs> MouseButtonPressed {
        //    add => DeskEnv.MouseButtonPressed += value;
        //    remove => DeskEnv.MouseButtonPressed -= value;
        //}
    }
}
