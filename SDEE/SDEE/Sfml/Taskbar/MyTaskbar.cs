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
    class MyTaskbar : Taskbar
    {
        /// <summary>
        /// The height of the taskbar in percentage between 0.0 and 1.0
        /// </summary>
        public float Height { get; }
        public Color Color { get; set; }

        public override Shape Shape
            => new RectangleShape()
            {
                Position = (Vector2f)Position,
                Size = (Vector2f)Size,
                FillColor = Color
            };

        public MyTaskbar(float height, Color color)
        {
            Height = height;
            Color = color;
        }

        protected override void Init()
        {
            Position = new Vector2i(0, (int)(DeskEnv.Size.Y * (1 - Height)));
            Size = new Vector2i(DeskEnv.Size.X, (int)(DeskEnv.Size.Y * Height));

            base.Init();
        }

        //public override void Draw(RenderTarget target, RenderStates states)
        //{
        //    RectangleShape rs = new RectangleShape()
        //    {
        //        Position = (Vector2f)Position,
        //        Size = (Vector2f)Size,
        //        FillColor = Color
        //    };

        //    target.Draw(rs);

        //    base.Draw(target, states);
        //}

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
