using SFML.Graphics;
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
    abstract class Control
    {
        public ControlCollection Children { get; }
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
            Children = new ControlCollection(this);
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

        protected virtual void OnKeyPressed(KeyEventArgs e) { }
        protected virtual void OnMouseButtonPressed(MouseButtonEventArgs e) { }

        /// <summary>
        /// Deeply initializes every controls so that they get filled events from DE
        /// </summary>
        protected virtual void Init()
        {
            if (DeskEnv == null)
                throw new NoDEImplementedException();

            DeskEnv.KeyPressed += (s, e) => OnKeyPressed(e);
            DeskEnv.MouseButtonPressed += (s, e) => OnMouseButtonPressed(e);

            foreach (Control child in Children)
                child.Init();
        }
    }

}
