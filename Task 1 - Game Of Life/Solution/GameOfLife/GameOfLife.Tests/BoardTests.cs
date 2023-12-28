using GameOfLife.ConsoleApp;
using GameOfLife.Core;
using Moq;
using Xunit;

namespace GameOfLife.Tests
{
    public class BoardTests
    {
        private readonly MockData mockData;
        private readonly Mock<IInputManager> inputManager;

        public BoardTests()
        {
            mockData = new MockData();
            inputManager = new();
        }

        [Fact]
        public void UpdateBoard_UpdatesBoardCorrectly3on3()
        {
            bool[,] updatedBoard = mockData.UpdatedBoardExample3on3();
            bool[,] board = mockData.BoardExample3on3();
            int rows = board.GetLength(0);
            int columns = board.GetLength(1);
            Board gameBoard = new Board(rows, columns, board);

            gameBoard.UpdateBoard();
            bool[,] updatedBoardState = gameBoard.GetBoard();

            Assert.Equal(updatedBoard, updatedBoardState);
        }

        [Fact]
        public void UpdateBoard_UpdatesBoardCorrectly5on5()
        {
            bool[,] updatedBoard = mockData.UpdatedBoardExample5on5();
            bool[,] board = mockData.BoardExample5on5();
            int rows = board.GetLength(0);
            int columns = board.GetLength(1);
            Board gameBoard = new Board(rows, columns, board);

            gameBoard.UpdateBoard();
            bool[,] updatedBoardState = gameBoard.GetBoard();

            Assert.Equal(updatedBoard, updatedBoardState);
        }

        [Fact]
        public void UpdateBoard_UpdatesBoardCorrectly10on10()
        {
            bool[,] updatedBoard = mockData.UpdatedBoardExample10on10();
            bool[,] board = mockData.BoardExample10on10();
            int rows = board.GetLength(0);
            int columns = board.GetLength(1);
            Board gameBoard = new Board(rows, columns, board);

            gameBoard.UpdateBoard();
            bool[,] updatedBoardState = gameBoard.GetBoard();

            Assert.Equal(updatedBoard, updatedBoardState);
        }

        [Fact]
        public void GetActiveCell_Board3on3()
        {
            int expectedResult = 3;
            bool[,] board = mockData.BoardExample3on3();
            Board gameBoard = new Board(board.GetLength(0), board.GetLength(1), board);

            int result = gameBoard.GetBoardActiveCell();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetActiveCell_UpdatedBoard3on3()
        {
            int expectedResult = 3;
            bool[,] board = mockData.UpdatedBoardExample3on3();
            Board gameBoard = new Board(board.GetLength(0), board.GetLength(1), board);

            int result = gameBoard.GetBoardActiveCell();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetActiveCell_Board5on5()
        {
            int expectedResult = 5;
            bool[,] board = mockData.BoardExample5on5();
            Board gameBoard = new Board(board.GetLength(0), board.GetLength(1), board);

            int result = gameBoard.GetBoardActiveCell();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetActiveCell_UpdatedBoard5on5()
        {
            int expectedResult = 5;
            bool[,] board = mockData.UpdatedBoardExample5on5();
            Board gameBoard = new Board(board.GetLength(0), board.GetLength(1), board);

            int result = gameBoard.GetBoardActiveCell();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetActiveCell_Board10on10()
        {
            int expectedResult = 51;
            bool[,] board = mockData.BoardExample10on10();
            Board gameBoard = new Board(board.GetLength(0), board.GetLength(1), board);

            int result = gameBoard.GetBoardActiveCell();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetActiveCell_UpdatedBoard10on10()
        {
            int expectedResult = 27;
            bool[,] board = mockData.UpdatedBoardExample10on10();
            Board gameBoard = new Board(board.GetLength(0), board.GetLength(1), board);

            int result = gameBoard.GetBoardActiveCell();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void InitializeBoard_ShouldInitializeBoardWithRandomValues()
        {
            // Arrange
            int rows = 3;
            int columns = 3;
            var board = new Board(rows, columns);

            // Act
            board.InitializeBoard();
            bool[,] resultBoard = board.GetBoard();

            // Assert
            bool hasTrueValue = false;
            bool hasFalseValue = false;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (resultBoard[i, j] == true)
                    {
                        hasTrueValue = true;
                    }
                    else
                    {
                        hasFalseValue = true;
                    }
                }
            }

            Assert.True(hasTrueValue && hasFalseValue, "Board contains both true and false values");
        }
    }
}