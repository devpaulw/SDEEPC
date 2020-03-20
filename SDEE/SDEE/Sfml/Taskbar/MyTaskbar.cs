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
    class MyTaskbar : Control
    {
        /// <summary>
        /// The height of the taskbar in percentage between 0.0 and 1.0
        /// </summary>
        public float Height { get; }
        public Color Color { get; set; }
        public virtual int BorderLength => 10;

        protected override Shape Shape { // TODO A function to get this with an easier way (preconfigured shape with graphicControl values)
            get => new RectangleShape(this.GetBasicShape())
            {
                FillColor = Color
            };
        }

        public MyTaskbar(DesktopEnvironment parent, float height, Color color) : base(parent)
        {
            Height = height;
            Color = color;

            Position = new Vector2i(0, (int)(parent.Size.Y * (1 - Height)));
            Size = new Vector2i(parent.Size.X, (int)(parent.Size.Y * Height));
        }

        protected override void Init()
        {
            // TODO Create container
            #region Sort taskbar elements
            for (int i = 0, tbX = BorderLength / 2; i < Controls.Count; i++)
            {
                Controls[i].Position = new Vector2i(tbX, Position.Y);
                tbX += Controls[i].Size.X + BorderLength;
            }
            #endregion

            base.Init();
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
