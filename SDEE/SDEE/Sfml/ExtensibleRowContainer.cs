using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class ExtensibleRowContainer : Control
    {
        private int freeXpos;

        public int BorderLength { get; set; }

        public ExtensibleRowContainer(Control parent, int borderLength) : base(parent)
        {
            BorderLength = borderLength;

            freeXpos = BorderLength;
        }

        protected override void OnControlAdded(Control control)
        {
            control.Position = new Vector2i(freeXpos + Position.X, Position.Y);
            freeXpos += control.Size.X + BorderLength * 2;

            base.OnControlAdded(control);
        }
    }
}
