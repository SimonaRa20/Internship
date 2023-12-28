using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class LogicValidator
    {
        public static int CheckAroundLiveCells(int x, int y, int rows, int columns, bool[,] board)
        {
            int liveCells = 0;

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newX = x + dx[i];
                int newY = y + dy[i];

                if (IsValidCell(newX, newY, rows, columns) && board[newX, newY])
                {
                    liveCells++;
                }
            }

            return liveCells;
        }

        private static bool IsValidCell(int x, int y, int rows, int columns)
        {
            if (x >= 0 && x < rows && y >= 0 && y < columns)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UpdateCellValue(bool value, int liveNeighbors)
        {
            if(value)
            {
                if(liveNeighbors < 2 || liveNeighbors > 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if(liveNeighbors == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
