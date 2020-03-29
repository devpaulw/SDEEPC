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

        protected override Dictionary<string, Type> RequiredParameters => new Dictionary<string, Type>()
        {
            { "position", typeof(Vector2u) }
        };

        private protected override Shape Shape => null;

        public TableContainer(Control parent, int rowCount, int columnCount, int rowWidth, int columnHeight, int borderSize) : base(parent: parent)
		{
            RowCount = rowCount;
            ColumnCount = columnCount;
            RowWidth = rowWidth;
            ColumnHeight = columnHeight;
            BorderSize = borderSize;
            //Controls = new TableControlCollection(this);
        }

        protected override void OnControlAdded(ControlAddedEventArgs e)
        {
            var position = (Vector2u)e.Parameters["position"];

            e.Control.Position = new Vector2i(
                (int)position.X * ColumnHeight +
                (int)position.X * BorderSize * 2 + BorderSize,
                (int)position.Y * RowWidth +
                (int)position.Y * BorderSize * 2 + BorderSize);// DOLATER Don't allow if outside table, too big for the box and not autosize, 

            base.OnControlAdded(e);
        }
    }
}
