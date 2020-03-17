using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class MyTaskbar : GraphicControl
    {
        /// <summary>
        /// The height of the taskbar in percentage between 0.0 and 1.0
        /// </summary>
        public float Height { get; set; }
        public Color Color { get; set; }
        //public Collection<TaskbarElement> Elements {
        //    get {
        //        return (Collection<TaskbarElement>)
        //            from control in Controls
        //            where control is TaskbarElement
        //            select control as TaskbarElement;
        //    }
        //}

        public MyTaskbar(DesktopEnvironment de, float height, Color color) : base(de)
        {
            Height = height;
            Color = color;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            RectangleShape rs = new RectangleShape()
            {
                Size = new Vector2f(target.Size.X, target.Size.Y * Height),
                Position = new Vector2f(0, target.Size.Y * (1 - Height)),
                FillColor = Color
            };

            target.Draw(rs);

            base.Draw(target, states);
        }

        protected override void OnKeyPressed(KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.B) // TEMP
                StartExe(@"C:\Windows\notepad.exe");

            base.OnKeyPressed(e);
        }

        protected override void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            if (e.Button != Mouse.Button.Left)
                MessageBox("Tu viens de cliquer sur le bouton " + e.Button + " de ta souris.\nFrom " + GetType(),
                    "SDEE test", MessageBoxIcon.Information); // TEMP

            base.OnMouseButtonPressed(e);
        }
    }
}
