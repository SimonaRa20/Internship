using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Board
    {
        private int rows;
        private int columns;
        private bool [,] board;

        public Board(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            board = new bool[rows, columns];
        }

        public Board(int rows, int columns, bool[,] board)
        {
            this.rows = rows;
            this.columns = columns;
            this.board = board;
        }

        public bool[,] GetBoard()
        {
            return board;
        }

        public void InitializeBoard()
        {
            Random random = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    board[i, j] = false;
                    if (random.Next(2) == 0)
                    {
                        board[i, j] = true;
                    }
                }
            }
        }

        public void UpdateBoard()
        {
            bool[,] newBoard = new bool[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int liveNeighbors = LogicValidator.CheckAroundLiveCells(i, j, rows, columns, board);
                    newBoard[i,j] = LogicValidator.UpdateCellValue(board[i,j], liveNeighbors);
                }
            }

            board = newBoard;
        }

        public int GetBoardActiveCell()
        {
            int activeCellCount = 0;
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    if (board[i, j])
                    {
                        activeCellCount++;
                    }
                }
            }

            return activeCellCount;
        }
    }
}
