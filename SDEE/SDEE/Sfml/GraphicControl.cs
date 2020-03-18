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
    abstract class GraphicControl : Control, Drawable
    {
        protected abstract Shape Shape { get; }

        public Vector2i Position { get; set; }
        public Vector2i Size { get; set; }

        protected GraphicControl() { }

        public IEnumerable<GraphicControl> GetGraphicControls()
            => from control in Children
               where control is GraphicControl
               select control as GraphicControl;

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Shape != null)
                target.Draw(Shape);

            foreach (var control in Children)
                if (control is GraphicControl drawableControl)
                    target.Draw(drawableControl);
        }

        protected override void OnMouseButtonPressed(MouseButtonEventArgs e)
        {
            if (e.X >= Position.X && e.X <= Position.X + Size.X
                && e.Y >= Position.Y && e.Y <= Position.Y + Size.Y)
            {
                OnClick(e);
            }

            base.OnMouseButtonPressed(e);
        }

        protected virtual void OnClick(MouseButtonEventArgs e) { }


        /// /// <summary>
        /// Init your Position, Size when Desktop Environment starts
        /// </summary>
        protected override void Init()
            => base.Init();
    }
}
