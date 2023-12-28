using GameOfLife.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ConsoleApp
{
    public class ConsoleInOutUtils : IConsoleInOutUtils
    {
        private readonly IFileInOutUtils fileInOutUtils = new FileInOutUtils();
        private readonly IInputManager inputManager;

        public ConsoleInOutUtils(IInputManager inputManager, IFileInOutUtils fileInOutUtils)
        {
            this.inputManager = inputManager;
            this.fileInOutUtils = fileInOutUtils;
        }

        public BoardType CheckSelectedBoardType()
        {
            while (true)
            {
                Console.WriteLine("Enter 'new', if you want to get new randomized game board.");
                Console.WriteLine("Enter 'old', if you want to get previous saved game board.");
                string enteredValue = inputManager.ReadLine();

                if (enteredValue.ToLower() == BoardType.New.ToString().ToLower())
                {
                    return BoardType.New;
                }
                else if (enteredValue.ToLower() == BoardType.Old.ToString().ToLower())
                {
                    return BoardType.Old;
                }
                else
                {
                    inputManager.WriteLine("Please to enter 'new' or 'old' game board");
                    continue;
                }
            }
        }

        public void SetGame(BoardType selectedType, out Game game)
        {
            while (true)
            {
                int rows;
                int columns;

                if (selectedType == BoardType.New)
                {
                    RunNewGame(out rows, out columns, out game);
                    break;
                }
                else
                {
                    RunOldGame(out rows, out columns, out game);
                    break;
                }
            }
        }

        public void RunNewGame(out int rows, out int columns, out Game game)
        {
            CheckEnteredValues(out rows, "rows");
            CheckEnteredValues(out columns, "columns");
            game = new Game(rows, columns);
        }

        public void RunOldGame(out int rows, out int columns, out Game game)
        {
            bool[,] board;
            fileInOutUtils.ReadFromFile(out rows, out columns, out board, "game.txt");
            game = new Game(rows, columns, board);
        }

        public void CheckEnteredValues(out int input, string header)
        {
            while (true)
            {
                string text = "Enter the number of " + header + " :";
                inputManager.WriteLine(text);
                if (int.TryParse(inputManager.ReadLine(), out input))
                {
                    if (input > 0)
                    {
                        break;
                    }
                    text = "Please enter a valid positive integer for " + header + " :";
                    inputManager.WriteLine(text);
                }
                else
                {
                    text = "Please enter a valid positive integer for " + header + " :";
                    inputManager.WriteLine(text);
                }
            }
        }

        public void PrintBoardInformation(int iterations, int activeCellsCount)
        {
            string text = "Iterations: " + iterations + " . Active cells count: " + activeCellsCount + " .";
            inputManager.WriteLine(text);
        }

        public void PrintBoard(bool[,] board)
        {
            string text = string.Empty;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j])
                    {
                        text += "X";
                    }
                    else
                    {
                        text += " ";
                    }
                }

                text += "\r\n";
            }
            inputManager.Write(text);
        }

        public UserAction CheckUserAction(int rows, int columns, bool[,] board)
        {
            Console.WriteLine("Write 'save' if you want to save game. Write 'stop' if you want to stop game.");
            Console.WriteLine("Press enter, if you want to get another iteration.");
            string enteredValue = inputManager.ReadLine();
            if (enteredValue.ToLower() == UserAction.Save.ToString().ToLower())
            {
                fileInOutUtils.SaveGameBoard(rows, columns, board, "game.txt");
                return UserAction.Save;
            }
            if (enteredValue.ToLower() == UserAction.Stop.ToString().ToLower())
            {
                return UserAction.Stop;
            }

            return UserAction.Continue;
        }
    }
}
