using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    public abstract class DesktopEnvironment : Control, IDisposable
    {
        private KeyboardShortcutComponent keyboardShortcuts;
        internal SfW32DEWindow window; // temp public

        public DesktopEnvironment() : base(null)
        {
            keyboardShortcuts = new KeyboardShortcutComponent();
            window = new SfW32DEWindow();

            Position = window.Position;
            Size = (Vector2i)window.Size;

            window.Closed += (s, e) => window.Close();

            window.KeyPressed += (s, e) => OnKeyPressed(e);
            window.MouseButtonPressed += (s, e) => OnMouseButtonPressed(e);
            window.MouseMoved += (s, e) => OnMouseMoved(e);
        }

        //public override void Load()
        //{
        //    // ... TO ADD Needed EventHandlers

        //    base.Load();
        //}

        public void Start()
        {
            while (window.IsOpen) // MAIN LOOP
            {
                window.DispatchSystemMessage();
                window.DispatchEvents();
                window.Clear();
                window.Draw(this);
                window.Display();
            }
        }

        protected override void OnKeyPressed(KeyEventArgs e)
        {
            DesktopEnvironmentCommand command = keyboardShortcuts.GetCommand(KeyCombinationFactory.FromKeyEventArgs(e));
            if (command is null)
                return;
            if (command is ExecuteProgramCommand concreteCommand)
                StartExe(concreteCommand.ExecutablePath);
            base.OnKeyPressed(e);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    window.Dispose();
                }

                // xTODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // xTODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
