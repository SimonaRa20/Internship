using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ConsoleApp
{
    public interface IFileInOutUtils
    {
        void SaveGameBoard(int rows, int columns, bool[,] board, string fileName);
        void ReadFromFile(out int rows, out int columns, out bool[,] board, string fileName);
    }
}
