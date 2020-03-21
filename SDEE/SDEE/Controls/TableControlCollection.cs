using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    public class TableControlCollection : Control.ControlCollection
    {


        public TableContainer Container => Owner as TableContainer;

        public TableControlCollection(TableContainer container) : base(container)
        {
            
        }

        public virtual void Add(Control control, Vector2u position)
        {
            control.Position = new Vector2i(
                (int)position.X * Container.ColumnHeight + 
                (int)position.X * Container.BorderSize * 2 + Container.BorderSize,
                (int)position.Y * Container.RowWidth + 
                (int)position.Y * Container.BorderSize * 2 + Container.BorderSize);// DOLATER Don't allow if outside table, too big for the box and not autosize, 

            Owner.Controls.Add(control);
        }
    }
}
