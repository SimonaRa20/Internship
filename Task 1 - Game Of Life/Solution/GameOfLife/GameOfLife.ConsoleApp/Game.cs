using GameOfLife.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ConsoleApp
{
    public class Game
    {
        private int rows;
        private int columns;
        private Board board;
        private readonly IConsoleInOutUtils consoleInOutUtils = new ConsoleInOutUtils(new InputManager(), new FileInOutUtils());

        public Game() { }

        public Game(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            board = new Board(rows, columns);
            board.InitializeBoard();
        }

        public Game(int rows, int columns, bool[,] board)
        {
            this.rows = rows;
            this.columns = columns;
            this.board = new Board(rows, columns, board);
        }

        public Board GetGameBoard()
        {
            return board;
        }
    }
}
