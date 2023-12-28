using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Tests
{
    public class MockData
    {
        public bool[,] BoardExample3on3()
        {
            bool[,] board = new bool[3, 3]
            {
                { false, true, false },
                { false, true, false },
                { false, true, false }
            };

            return board;
        }

        public bool[,] UpdatedBoardExample3on3()
        {
            bool[,] board = new bool[3, 3]
            {
                { false, false, false },
                { true, true, true },
                { false, false, false }
            };

            return board;
        }

        public bool[,] BoardExample5on5()
        {
            bool[,] board = new bool[5, 5]
            {
                { false, false, true, false, false },
                { true, false, true, false, false },
                { false, true, true, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            return board;
        }

        public bool[,] UpdatedBoardExample5on5()
        {
            bool[,] board = new bool[5, 5]
            {
                { false, true, false, false, false },
                { false, false, true, true, false },
                { false, true, true, false, false },
                { false, false, false, false, false },
                { false, false, false, false, false }
            };

            return board;
        }

        public bool[,] BoardExample10on10()
        {
            bool[,] board = new bool[10, 10]
            {
                { true, true, true, true, false, true, true, false, false, false },
                { false, false, true, true, false, true, true, false, true, false },
                { false, false, true, false, true, true, true, false, false, true },
                { true, true, false, false, true, false, true, false, true, false },
                { true, false, true, true, true, true, true, false, false, true },
                { true, false, true, false, false, true, false, true, false, false },
                { true, false, false, false, false, true, true, true, false, false },
                { true, true, true, true, true, true, false, true, false, false },
                { true, false, true, true, true, false, false, false, false, false },
                { true, true, false, false, true, false, false, false, true, false }
            };

            return board;
        }

        public bool[,] UpdatedBoardExample10on10()
        {
            bool[,] board = new bool[10, 10]
            {
                { false, true, false, true, false, true, true, true, false, false },
                { false, false, false, false, false, false, false, false, false, false },
                { false, false, true, false, false, false, false, false, true, true },
                { true, false, false, false, false, false, false, false, true, true },
                { true, false, true, false, false, false, false, false, true, false },
                { true, false, true, false, false, false, false, true, true, false },
                { true, false, false, false, false, false, false, true, true, false },
                { true, false, false, false, false, false, false, true, false, false },
                { false, false, false, false, false, false, false, false, false, false },
                { true, true, true, false, true, false, false, false, false, false }
            };

            return board;
        }
    }
}
