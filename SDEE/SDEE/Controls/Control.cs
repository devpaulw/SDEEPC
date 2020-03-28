using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32;

namespace SDEE
{
    public abstract class Control : Drawable
    {
       private readonly List<Control> controls = new List<Control>();

        private bool _isZControl;
        private bool IsZControl {
            get {
                return _isZControl;
            }
            set {
                _isZControl = value;
                if (value == true)
                {
                    zrWnd = new RenderWindow(new VideoMode((uint)Size.X, (uint)Size.Y), null, Styles.None);
                    zrWnd.Position = AbsolutePosition;

                    zrWnd.Closed += (s, e) => zrWnd.Close();

                    zrWnd.KeyPressed += (s, e) => OnKeyPressed(e);
                    zrWnd.MouseButtonPressed += (s, e) => OnMouseButtonPressed(e);// new MouseButtonEventArgs(new MouseButtonEvent() { X = e.X - Position.X, Y= e.Y - Position.Y }));
                    zrWnd.MouseMoved += (s, e) => OnMouseMoved(e);// new MouseMoveEventArgs(new MouseMoveEvent() { X = e.X - Position.X, Y = e.Y - Position.Y }));
                }
            }
        }

        private RenderWindow zrWnd;

        /// <summary>
        /// Construct a control
        /// </summary>
        public Control(Control parent)
        {
            Parent = parent;

            if (DeskEnv == null)
                throw new NoDEImplementedException();

            if (Parent == null)
                return;

            Parent.MouseButtonPressed += (s, e) => OnMouseButtonPressed(new MouseButtonEventArgs(new MouseButtonEvent() { X = e.X - Position.X, Y = e.Y - Position.Y })); ;
            Parent.KeyPressed += (s, e) => OnKeyPressed(e);
            Parent.MouseMoved += (s, e) => OnMouseMoved(e);

        }

        /// <summary>
        /// Construct a control with pre-defined position and size
        /// </summary>
        public Control(Control parent, Vector2i position, Vector2i size) : this(parent)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Construct a control with pre-defined size
        /// </summary>
        public Control(Control parent, Vector2i size) : this(parent, default, size) { }

        /// <summary>
        /// Automatically add the control to its parent
        /// </summary>
        /// <param name="control">The control to be added to its parent</param>
        public static void Load(Control control)
        {
            if (control.Parent.RequiredParameters != null && control.Parent.RequiredParameters.Any()) // No parameters specified when there are
                throw new WrongControlParametersException(control.Parent.RequiredParameters);

            if (control.Parent.controls.Contains(control))
                return;

            control.Parent.controls.Add(control);
            control.Parent.OnControlAdded(new ControlAddedEventArgs(control));
        }

        /// <summary>
        /// Automatically add the control to its parent
        /// </summary>
        /// <param name="control">The control to be added to its parent</param>
        /// <param name="parameters">The parameters required by this control</param>
        public static void Load(Control control, params object[] parameters)
        {
            if (parameters.Count() != control.Parent.RequiredParameters.Count()) // Not enough parameters specified
                throw new WrongControlParametersException(control.Parent.RequiredParameters);

            var filledParameters = new Dictionary<string, object>();
            int i = 0;
            foreach (var parameter in control.Parent.RequiredParameters)
            {
                if (parameters[i].GetType() != parameter.Value) // Wrong type specified
                    throw new WrongControlParametersException(control.Parent.RequiredParameters);
                filledParameters.Add(parameter.Key, parameters[i]);
                i++;
            }

            if (control.Parent.controls.Contains(control))
                return;

            control.Parent.controls.Add(control);
            control.Parent.OnControlAdded(new ControlAddedEventArgs(control, filledParameters));
        }
        
        protected virtual void Update()
        {
            // TODO
        }

        protected virtual Dictionary<string, Type> RequiredParameters { get; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            // Here is a complex function but we don't care: It makes a much better design! The user don't care that there are other real windows, he just want to draw its controls!
            if (!IsEnabled)
            {
                if (IsZControl)
                    zrWnd.SetVisible(false);
                return;
            }

            if (IsZControl)
            {
                zrWnd.SetVisible(true);

                zrWnd.DispatchEvents();
                zrWnd.Clear(); // DOLATER And make it Win32 transparent by default
            }

            if (Shape != null)
            {
                Vector2f GetDrawPosition()
                {
                    Control parent = this;

                    Vector2i sp = new Vector2i(0, 0);
                    while (parent != null && !parent.IsZControl)
                    {
                        sp += parent.Position;
                        parent = parent.Parent;
                    }
                    return (Vector2f)sp;
                }

                Shape copyShape = Shape;
                if (copyShape is RectangleShape rs)
                    rs.Size = (Vector2f)Size;

                if (IsZControl)
                {
                    zrWnd.Draw(copyShape);
                }
                else
                {
                    copyShape.Position = GetDrawPosition();
                    target.Draw(copyShape);
                }
            }

            if (IsZControl)
            {
                foreach (var child in Controls)
                    zrWnd.Draw(child);

                zrWnd.Display();
            }
            else
                foreach (var child in Controls)
                    target.Draw(child);
        }
        /// <summary>
        /// Position of the control relative to the parent
        /// </summary>
        public virtual Dictionary<string, string> GetXmlAttributes()
        {
            return new Dictionary<string, string>();
        }

        public virtual ControlType Type => ControlType.NotSavable;

        public uint Id { get; set; }
        public Vector2i Position { get; set; }
        public Vector2i Size { get; set; }
        public bool IsEnabled { get; set; } = true;
        public IEnumerable<Control> Controls => controls;
        public Control Parent { get; }

        /// <summary>
        /// Position of the control relative to the Desktop Environment
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
                throw new NoDEImplementedException();
            }
        }

        public Control(Control parent, uint id = 0)
        {
            Controls = new ControlCollection(this);
            Parent = parent;
            Id = id;

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

        /// <summary>
        /// Configure your Shape without care about the Position and Size
        /// </summary>
        // DOLATER Make a new SDEE specific Shape without Position and Size
        private protected abstract Shape Shape { get; }

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
                    //        new WindowCustomizer(hWnd).StartCustomization(DeskEnv.window.SystemHandle); // TEMP
                    //        Console.WriteLine(DeskEnv.window.SystemHandle);
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
        /// Set the control Z-Order
        /// </summary>
        public void SetZ(ZOrder zOrder)
        {
            IsZControl = true;

            switch (zOrder)
            {
                case ZOrder.Top:
                    break;
            }
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
        protected virtual void OnControlAdded(ControlAddedEventArgs e)
        {
            if (!IsEnabled)
                return;

            ControlAdded?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
        }
        protected virtual void OnMouseMoved(MouseMoveEventArgs e)
        {
            if (!IsEnabled)
                return;

            MouseMoved?.Invoke(this, e ?? throw new ArgumentNullException(nameof(e)));
        }

        public override bool Equals(object obj)
        {
            return obj is Control control &&
                   Type == control.Type &&
                   Id == control.Id;
        }

        public override int GetHashCode()
        {
            int hashCode = -1324594315;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;
        public event EventHandler<MouseMoveEventArgs> MouseMoved;
        public event EventHandler<MouseButtonEventArgs> Click;
        public event EventHandler<ControlAddedEventArgs> ControlAdded;
    }
}