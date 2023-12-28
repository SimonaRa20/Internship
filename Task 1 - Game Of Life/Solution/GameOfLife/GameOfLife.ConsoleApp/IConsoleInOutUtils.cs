using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ConsoleApp
{
    public interface IConsoleInOutUtils
    {
        BoardType CheckSelectedBoardType();
        void SetGame(BoardType selectedType, out Game game);
        void CheckEnteredValues(out int input, string header);
        void PrintBoardInformation(int iterations, int activeCellsCount);
        void PrintBoard(bool[,] board);
        UserAction CheckUserAction(int rows, int columns, bool[,] board);
    }
}
