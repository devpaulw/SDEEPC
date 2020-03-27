using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    class ExtensibleRowContainer : Control
    {
        private int freeXpos;

        public int BorderLength { get; set; }

        protected override Shape Shape => null;
        public override bool NoSize => false;

        public override bool NoMove => false;

        public ExtensibleRowContainer(Control parent, int borderLength) : base(parent)
        {
            BorderLength = borderLength;

            freeXpos = BorderLength;
        }

        protected override void OnControlAdded(Control control)
        {
            control.Position = new Vector2i(freeXpos, 0);
            freeXpos += control.Size.X + BorderLength * 2;

            base.OnControlAdded(control);
        }
    }
}
