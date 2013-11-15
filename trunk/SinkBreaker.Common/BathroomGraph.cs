using System;
using System.Text;
using System.Collections.Generic;

namespace SinkBreaker.Common
{
    // UNDIRECTED GRAPH THAT IS STRUCTURED AS A TWO DIMENSIONAL MATRIX, WHERE CELLS ARE NEIGHBOURS ONLY IF THEY SHARE A SIDE
    public class BathroomGraph
    {
        Dictionary<string, TileCell> floor;
        StringBuilder output;
        List<TileCell> visited;
        Queue<TileCell> searchQueue;

        public uint Rows { get; protected set; }
        public uint Cols { get; protected set; }

        public BathroomGraph(uint rows, uint cols)
        {
            output = new StringBuilder();
            floor = new Dictionary<string, TileCell>();
            searchQueue = new Queue<TileCell>();
            visited = new List<TileCell>();

            Rows = rows;
            Cols = cols;

            CreateCells();
            ConnectCells();
        }

        #region INIT GRAPH
        private void CreateCells()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    string name = string.Format("{0} {1}", row, col);
                    floor.Add(name, new TileCell(name));
                }
            }
        }

        private void ConnectCells()
        {
            unsafe
            {
                foreach (var cell in floor)
                {
                    TileCell currentCell = cell.Value;
                    bool up = false;
                    bool down = false;
                    bool left = false;
                    bool right = false;

                    // CONNECTING NEIGHBOURS
                    if (currentCell.RowOriginal > 0)
                    {
                        up = true;
                        currentCell.Top = floor[string.Format("{0} {1}", currentCell.RowOriginal - 1, currentCell.ColOriginal)];
                    }
                    if (currentCell.ColOriginal > 0)
                    {
                        left = true;
                        currentCell.Left = floor[string.Format("{0} {1}", currentCell.RowOriginal, currentCell.ColOriginal - 1)];
                    }
                    if (currentCell.RowOriginal < (Rows - 1))
                    {
                        down = true;
                        currentCell.Bottom = floor[string.Format("{0} {1}", currentCell.RowOriginal + 1, currentCell.ColOriginal)];
                    }
                    if (currentCell.ColOriginal < (Cols - 1))
                    {
                        right = true;
                        currentCell.Right = floor[string.Format("{0} {1}", currentCell.RowOriginal, currentCell.ColOriginal + 1)];
                    }

                    // CONNECTING DIAGONALS
                    if (up)
                    {
                        if (right)
                        {
                            currentCell.TopRight = floor[string.Format("{0} {1}", currentCell.RowOriginal - 1, currentCell.ColOriginal + 1)];
                        }
                        if (left)
                        {
                            currentCell.TopLeft = floor[string.Format("{0} {1}", currentCell.RowOriginal - 1, currentCell.ColOriginal - 1)];
                        }
                    }
                    if (down)
                    {
                        if (right)
                        {
                            currentCell.BottomRight = floor[string.Format("{0} {1}", currentCell.RowOriginal + 1, currentCell.ColOriginal + 1)];
                        }
                        if (left)
                        {
                            currentCell.BottomLeft = floor[string.Format("{0} {1}", currentCell.RowOriginal + 1, currentCell.ColOriginal - 1)];
                        }
                    }
                }
            }

        }
        #endregion

        public void PlaceFigure(string type, string row, string col)
        {
            TileCell root = floor[string.Format("{0} {1}", row, col)];
            switch (type)
            {

                case "ninetile":
                    FitFigure(Figures.ninetile, root);
                    break;
                case "plus":
                    FitFigure(Figures.plus, root);
                    break;
                case "hline":
                    FitFigure(Figures.hline, root);
                    break;
                case "vline":
                    FitFigure(Figures.vline, root);
                    break;
                case "angle-ur":
                    FitFigure(Figures.angle_ur, root);
                    break;
                case "angle-dr":
                    FitFigure(Figures.angle_dr, root);
                    break;
                case "angle-dl":
                    FitFigure(Figures.angle_dl, root);
                    break;
                case "angle-ul":
                    FitFigure(Figures.angle_ul, root);
                    break;
                default:
                    throw new ArgumentException(string.Format("Non existing figure passed as argument. Please review type of figure: {0}", type));
            }
        }

        // PERFORMS BFS STARTING FROM A GIVEN CELL OF THE MATRIX AND SEARCHES FOR THE FIRST CELL THAT FITS A GIVEN FIGURE
        private void FitFigure(Figures figureType, TileCell root)
        {
            // RESETS
            searchQueue.Clear();
            int visitedCount = visited.Count;
            for (int i = 0; i < visitedCount; i++)
            {
                visited[i].Visited = false;
            }
            visited.Clear();

            // START SEARCHING FOR A FREE SPOT WHERE THE FIGURE OF TILES CAN FIT
            searchQueue.Enqueue(root);
            string name = string.Empty;

            while (searchQueue.Count > 0)
            {
                TileCell current = searchQueue.Dequeue();
                current.Visited = true;
                visited.Add(current);

                if (figureType == Figures.ninetile)
                {
                    if (FitNinetale(current))
                    {
                        name = current.Name;
                        break;
                    }
                }
                else if (figureType == Figures.angle_dl)
                {
                    if (FitAngleDL(current))
                    {
                        name = current.Name;
                        break;
                    }
                }
                else if (figureType == Figures.angle_dr)
                {
                    if (FitAngleDR(current))
                    {
                        name = current.Name;
                        break;
                    }
                }
                else if (figureType == Figures.angle_ul)
                {
                    if (FitAngleUL(current))
                    {
                        name = current.Name;
                        break;
                    }
                }
                else if (figureType == Figures.angle_ur)
                {
                    if (FitAngleUR(current))
                    {
                        name = current.Name;
                        break;
                    }
                }
                else if (figureType == Figures.hline)
                {
                    if (FitHLine(current))
                    {
                        name = current.Name;
                        break;
                    }
                }
                else if (figureType == Figures.plus)
                {
                    if (FitPlus(current))
                    {
                        name = current.Name;
                        break;
                    }
                }
                else
                {
                    if (FitVLine(current))
                    {
                        name = current.Name;
                        break;
                    }
                }

                // IF CURRENT CELL DOES NOT FIT THE FIGURE - TRY TO ADD NEIGHBOURS TO THE SEARCH QUEUE
                EnqueueForSearch(current.Top);
                EnqueueForSearch(current.Right);
                EnqueueForSearch(current.Bottom);
                EnqueueForSearch(current.Left);
            }

            // ADDING THE NAME OF THE FIRST CELL THAT FITS THE FIGURE
            output.AppendLine(name);
        }

        private void EnqueueForSearch(TileCell cell)
        {
            if (cell != null && !cell.Visited)
            {
                searchQueue.Enqueue(cell);
                cell.Visited = true;
                visited.Add(cell);
            }
        }


        // OUTPUT RETURN METHODS

        public string ReturnOutput()
        {
            return output.ToString();
        }


        // CELL CHECK METHODS
        private bool FitVLine(TileCell current)
        {
            if (current.Top != null && current.Bottom != null &&
                !current.Taken && !current.Top.Taken && !current.Bottom.Taken)
            {
                current.Top.Taken = true;
                current.Bottom.Taken = true;

                current.Taken = true;
                return true;
            }
            return false;
        }

        private bool FitPlus(TileCell current)
        {
            if (current.Top != null && current.Bottom != null && current.Left != null &&
                current.Right != null && !current.Taken && !current.Top.Taken &&
                !current.Bottom.Taken && !current.Left.Taken && !current.Right.Taken)
            {
                current.Top.Taken = true;
                current.Bottom.Taken = true;
                current.Left.Taken = true;
                current.Right.Taken = true;

                current.Taken = true;
                return true;
            }
            return false;
        }

        private bool FitHLine(TileCell current)
        {
            if (current.Left != null && current.Right != null &&
                !current.Taken && !current.Left.Taken && !current.Right.Taken)
            {
                current.Left.Taken = true;
                current.Right.Taken = true;

                current.Taken = true;
                return true;
            }
            return false;
        }

        private bool FitAngleUR(TileCell current)
        {
            if (current.Top != null && current.Right != null &&
                !current.Taken && !current.Top.Taken && !current.Right.Taken)
            {
                current.Top.Taken = true;
                current.Right.Taken = true;

                current.Taken = true;
                return true;
            }
            return false;
        }

        private bool FitAngleUL(TileCell current)
        {
            if (current.Top != null && current.Left != null &&
                !current.Taken && !current.Top.Taken && !current.Left.Taken)
            {
                current.Top.Taken = true;
                current.Left.Taken = true;

                current.Taken = true;
                return true;
            }
            return false;
        }

        private bool FitAngleDR(TileCell current)
        {
            if (current.Bottom != null && current.Right != null &&
                !current.Taken && !current.Bottom.Taken && !current.Right.Taken)
            {
                current.Bottom.Taken = true;
                current.Right.Taken = true;

                current.Taken = true;
                return true;
            }
            return false;
        }

        private bool FitAngleDL(TileCell current)
        {
            if (current.Bottom != null && current.Left != null &&
                !current.Taken && !current.Bottom.Taken && !current.Left.Taken)
            {
                current.Bottom.Taken = true;
                current.Left.Taken = true;

                current.Taken = true;
                return true;
            }
            return false;
        }

        private bool FitNinetale(TileCell current)
        {
            if (current.Top != null && current.Bottom != null && current.Left != null && current.Right != null &&
                        current.TopLeft != null && current.TopRight != null && current.BottomLeft != null && current.BottomRight != null &&
                        !current.Taken && !current.Top.Taken && !current.Bottom.Taken && !current.Left.Taken && !current.Right.Taken &&
                        !current.TopLeft.Taken && !current.TopRight.Taken && !current.BottomLeft.Taken && !current.BottomRight.Taken)
            {
                current.Top.Taken = true;
                current.Bottom.Taken = true;
                current.Left.Taken = true;
                current.Right.Taken = true;
                current.TopLeft.Taken = true;
                current.TopRight.Taken = true;
                current.BottomLeft.Taken = true;
                current.BottomRight.Taken = true;
                current.Taken = true;
                return true;
            }
            return false;
        }
    }
}
