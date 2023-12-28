using GameOfLife.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameOfLife.Tests
{
    public class LogicValidatorTests
    {
        private readonly MockData mockData;
        private readonly ILogicValidator logicValidator;

        public LogicValidatorTests()
        {
            mockData = new MockData();
            logicValidator = new LogicValidator();
        }

        [Fact]
        public void CheckAroundLiveCells_ReturnsCorrectLiveCellsCountBoard3on3()
        {
            bool[,] board = mockData.BoardExample3on3();
            int x = 1;
            int y = 1;
            int result = 2;

            int expectedResult = logicValidator.CheckAroundLiveCells(x, y, board.GetLength(0), board.GetLength(1), board);

            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void CheckAroundLiveCells_ReturnsCorrectLiveCellsCountUpdatedBoard3on3()
        {
            bool[,] board = mockData.UpdatedBoardExample3on3();
            int x = 1;
            int y = 1;
            int result = 2;

            int expectedResult = logicValidator.CheckAroundLiveCells(x, y, board.GetLength(0), board.GetLength(1), board);

            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void CheckAroundLiveCells_ReturnsCorrectLiveCellsCountBoard5on5()
        {
            bool[,] board = mockData.BoardExample5on5();
            int x = 1;
            int y = 1;
            int result = 5;

            int expectedResult = logicValidator.CheckAroundLiveCells(x, y, board.GetLength(0), board.GetLength(1), board);

            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void CheckAroundLiveCells_ReturnsCorrectLiveCellsCountUpdatedBoard5on5()
        {
            bool[,] board = mockData.UpdatedBoardExample5on5();
            int x = 1;
            int y = 1;
            int result = 4;

            int expectedResult = logicValidator.CheckAroundLiveCells(x, y, board.GetLength(0), board.GetLength(1), board);

            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void CheckAroundLiveCells_ReturnsCorrectLiveCellsCountBoard10on10()
        {
            bool[,] board = mockData.BoardExample10on10();
            int x = 1;
            int y = 1;
            int result = 5;

            int expectedResult = logicValidator.CheckAroundLiveCells(x, y, board.GetLength(0), board.GetLength(1), board);

            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void CheckAroundLiveCells_ReturnsCorrectLiveCellsCountUpdatedBoard10on10()
        {
            bool[,] board = mockData.UpdatedBoardExample10on10();
            int x = 1;
            int y = 1;
            int result = 2;

            int expectedResult = logicValidator.CheckAroundLiveCells(x, y, board.GetLength(0), board.GetLength(1), board);

            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData(true, 5, false)]
        [InlineData(true, 1, false)]
        [InlineData(true, 2, true)]
        [InlineData(true, 3, true)]
        [InlineData(false, 3, true)]
        [InlineData(false, 2, false)]
        public void UpdateCellValue_ReturnsExpectedResult(bool value, int liveNeighbors, bool expectedResult)
        {
            bool result = logicValidator.UpdateCellValue(value, liveNeighbors);

            Assert.Equal(expectedResult, result);
        }
    }
}
