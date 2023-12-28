using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Core
{
    public interface ILogicValidator
    {
        int CheckAroundLiveCells(int x, int y, int rows, int columns, bool[,] board);
        bool UpdateCellValue(bool value, int liveNeighbors);
    }
}
