using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class GameOfLife
    {
        private int rows;
        private int columns;
        private Board board;
        private int activeBoardCells = 0;
        private int iterations = 0;
        
        public GameOfLife() { }

        public GameOfLife(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            board = new Board(rows, columns);
            board.InitializeBoard();
        }

        public GameOfLife(int rows, int columns, bool[,] board)
        {
            this.rows = rows;
            this.columns = columns;
            this.board = new Board(rows, columns, board);
        }

        public void Start()
        {
            bool isActiveGame = true;
            while (isActiveGame)
            {
                Console.Clear();
                ConsoleInOutUtils.PrintBoard(rows, columns, board.GetBoard());
                activeBoardCells = board.GetBoardActiveCell();
                ConsoleInOutUtils.PrintBoardInformation(iterations, activeBoardCells);
                isActiveGame = ConsoleInOutUtils.CheckUserAction(rows, columns, board.GetBoard());
                board.UpdateBoard();
                iterations++;
                Thread.Sleep(1000);
            }
        }
    }
}
