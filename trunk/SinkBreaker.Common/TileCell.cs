using System;
using System.Collections.Generic;

namespace SinkBreaker.Common
{
    class TileCell
    {
        // NEIGHBOURS
        public TileCell Top { get; set; }
        public TileCell Bottom { get; set; }
        public TileCell Left { get; set; }
        public TileCell Right { get; set; }
        // DIAGONALS
        public TileCell TopRight { get; set; }
        public TileCell TopLeft { get; set; }
        public TileCell BottomRight { get; set; }
        public TileCell BottomLeft { get; set; }

        public string Name { get; protected set; } // CONSISTS OF THE ROW COL COORDINATES SEPERATED BY SPACE
        public int RowOriginal { get; protected set; }
        public int ColOriginal { get; protected set; }

        public bool Taken { get; set; }
        public bool Visited { get; set; }
        
        public TileCell(string name)
        {
            Name = name;
            Taken = false;
            Visited = false;

            string[] positionStr = name.Split();
            RowOriginal = int.Parse(positionStr[0]);
            ColOriginal = int.Parse(positionStr[1]);
        }
    }
}
