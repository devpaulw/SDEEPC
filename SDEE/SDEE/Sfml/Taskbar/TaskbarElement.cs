using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class TaskbarElement : GraphicControl
    {
        public Taskbar Taskbar => Parent as Taskbar;

        public TaskbarElement()
        {

        } 

        public override void Draw(RenderTarget target, RenderStates states)
        {
            RectangleShape exeIcon = new RectangleShape()
            {
                Size = new Vector2f(20, 20),
                Position = new Vector2f(50, target.Size.Y * Taskbar.Height + 4),
                FillColor = Color.Black
            };

            target.Draw(exeIcon);

            base.Draw(target, states);
        }

        protected override void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            if (e.X >= 50 && e.X <= 70 
                && e.Y >= RenderTarget.Size.Y * Taskbar.Height + 4 && e.Y <= RenderTarget.Size.Y * Taskbar.Height + 4 + 20)
            {
                StartExe(@"C:\Windows\system32\cmd.exe");
            }

            base.OnMouseButtonPressed(e);
        }
    }
}
