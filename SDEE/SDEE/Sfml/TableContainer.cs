using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{
    class TableContainer : Control
    {
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int BorderSize { get; set; }

        public TableContainer(Control parent) : base(parent)
        {

        }
    }
}
