using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    public class TableContainer : Control
    {
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int RowWidth { get; set; }
        public int ColumnHeight { get; set; }
        public int BorderSize { get; set; }

        public new TableControlCollection Controls { get; }

        public override ControlDrawing Drawing => null;

        public TableContainer(Control parent, int rowCount, int columnCount, int rowWidth, int columnHeight, int borderSize) : base(parent)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            RowWidth = rowWidth;
            ColumnHeight = columnHeight;
            BorderSize = borderSize;
            Controls = new TableControlCollection(this);
        }
    }
}
