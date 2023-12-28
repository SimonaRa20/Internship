using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class ConsoleInOutUtils
    {
        public static void CheckNewOrExistGame(out GameOfLife game)
        {
            while (true)
            {
                int rows;
                int columns;
                Console.WriteLine("Enter 'new', if you want to get new randomized game board.");
                Console.WriteLine("Enter 'old', if you want to get previous saved game board.");
                string enteredValue = Console.ReadLine();

                if (enteredValue.ToLower() == "new")
                {
                    CheckEnteredValues(out rows, "rows");
                    CheckEnteredValues(out columns, "columns");
                    game = new GameOfLife(rows, columns);
                    break;
                }
                if (enteredValue.ToLower() == "old")
                {
                    bool[,] board;
                    FileInOutUtils.ReadFromFile(out rows, out columns, out board, "game.txt");
                    game = new GameOfLife(rows, columns, board);
                    break;
                }
                else
                {
                    Console.WriteLine("Please to enter 'new' or 'old' game board");
                }
            }
        }

        public static void CheckEnteredValues(out int input, string header)
        {
            while (true)
            {
                Console.WriteLine("Enter the number of {0}: ", header);
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    if (input > 0)
                    {
                        break;
                    }
                    Console.WriteLine("Please enter a valid positive integer for {0}: ", header);
                }
                else
                {
                    Console.WriteLine("Please enter a valid positive integer for {0}: ", header);
                }
            }
        }

        public static void PrintBoardInformation (int iterations, int activeCellsCount)
        {
            Console.WriteLine("Iterations: {0}. Active cells count: {1}", iterations, activeCellsCount);
        }

        public static void PrintBoard(int rows, int columns, bool[,] board)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(board[i, j] ? "X" : " ");
                }

                Console.WriteLine();
            }
        }

        public static bool CheckUserAction(int rows, int columns, bool[,] board)
        {
            Console.WriteLine("Write 'save' if you want to save game. Write 'stop' if you want to stop game.");
            Console.WriteLine("Press enter, if you want to get another iteration.");
            string enteredValue = Console.ReadLine();
            if (enteredValue.ToLower() == "save")
            {
                FileInOutUtils.SaveGameBoard(rows, columns, board);
                return false;
            }
            if (enteredValue.ToLower() == "stop")
            {
                return false;
            }

            return true;
        }
    }
}
