using GameOfLife.ConsoleApp;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameOfLife.Tests
{
    public class ConsoleInOutUtilsTests
    {
        private readonly ConsoleInOutUtils consoleInOutUtils;
        private readonly Mock<IInputManager> inputManager;
        private readonly Mock<IFileInOutUtils> fileInOutUtils;
        private readonly MockData mockData;

        public ConsoleInOutUtilsTests()
        {
            mockData = new MockData();
            inputManager = new();
            fileInOutUtils = new();
            consoleInOutUtils = new(inputManager.Object, fileInOutUtils.Object);
        }

        [Fact]
        public void CheckUserAction_UserWritesSave_SavesBoardToFile_And_ReturnsFalse()
        {
            // Arrange
            var rows = 5;
            var columns = 5;
            bool[,] board = new bool[rows, columns];
            inputManager.Setup(x => x.ReadLine()).Returns("save");

            // Act
            var result = consoleInOutUtils.CheckUserAction(rows, columns, board);

            // Assert
            Assert.Equal(result, UserAction.Save);
            fileInOutUtils.Verify(x => x.SaveGameBoard(rows, columns, board, "game.txt"), Times.Once);
        }

        [Fact]
        public void CheckUserAction_UserWritesStop_SavesBoardToFile_And_ReturnsFalse()
        {
            // Arrange
            var rows = 5;
            var columns = 5;
            bool[,] board = new bool[rows, columns];
            inputManager.Setup(x => x.ReadLine()).Returns("stop");

            // Act
            var result = consoleInOutUtils.CheckUserAction(rows, columns, board);

            // Assert
            Assert.Equal(result, UserAction.Stop);
            fileInOutUtils.Verify(x => x.SaveGameBoard(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool[,]>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CheckUserAction_UserWrites_NotStopOrSave_ReturnsTrue()
        {
            // Arrange
            var rows = 5;
            var columns = 5;
            bool[,] board = new bool[rows, columns];
            inputManager.Setup(x => x.ReadLine()).Returns("test");

            // Act
            var result = consoleInOutUtils.CheckUserAction(rows, columns, board);

            // Assert
            Assert.Equal(result, UserAction.Continue);
            fileInOutUtils.Verify(x => x.SaveGameBoard(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool[,]>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CheckUserAction_UserWriteNew_ReturnsNewTypeOfBoard()
        {
            // Arrange
            inputManager.Setup(x => x.ReadLine()).Returns("new");

            // Act
            var result = consoleInOutUtils.CheckSelectedBoardType();

            // Assert
            Assert.Equal(result, BoardType.New);
        }

        [Fact]
        public void CheckUserAction_UserWriteOld_ReturnsOldTypeOfBoard()
        {
            // Arrange
            inputManager.Setup(x => x.ReadLine()).Returns("old");

            // Act
            var result = consoleInOutUtils.CheckSelectedBoardType();

            // Assert
            Assert.Equal(result, BoardType.Old);
        }

        [Fact]
        public void CheckUserAction_UserWriteNotOldOrNewAndNextTimeNew_ReturnMessage()
        {
            // Arrange
            inputManager.SetupSequence(x => x.ReadLine()).Returns("invalid").Returns("new");

            // Act
            var result = consoleInOutUtils.CheckSelectedBoardType();

            // Assert
            inputManager.Verify(x => x.WriteLine("Please to enter 'new' or 'old' game board"), Times.Once);
            Assert.Equal(result, BoardType.New);
        }

        [Fact]
        public void CheckEnteredValues_ValidInput_ReturnsPositiveInteger()
        {
            // Arrange
            int expectedInput = 42;
            string header = "rows";

            inputManager.SetupSequence(x => x.ReadLine())
                .Returns("42");

            // Act
            consoleInOutUtils.CheckEnteredValues(out int input, header);

            // Assert
            Assert.Equal(expectedInput, input);
        }

        [Fact]
        public void CheckEnteredValues_InvalidInputThenValidInput_ReturnsValidPositiveInteger()
        {
            // Arrange
            int expectedInput = 42;
            string header = "rows";

            inputManager.SetupSequence(x => x.ReadLine())
                .Returns("invalid")
                .Returns("42");

            // Act
            consoleInOutUtils.CheckEnteredValues(out int input, header);

            // Assert
            Assert.Equal(expectedInput, input);
        }

        [Fact]
        public void CheckEnteredValues_InvalidInputThenValidInput_ReturnsValidNegativeInteger()
        {
            // Arrange
            int expectedInput = 42;
            string header = "testHeader";

            inputManager.SetupSequence(x => x.ReadLine())
                .Returns("-1")
                .Returns("42");

            // Act
            consoleInOutUtils.CheckEnteredValues(out int input, header);

            // Assert
            Assert.Equal(expectedInput, input);
        }

        [Fact]
        public void PrintBoardInformation_CorrectlyFormatsAndWritesInformation()
        {
            // Arrange
            int iterations = 5;
            int activeCellsCount = 10;

            string expectedText = "Iterations: 5 . Active cells count: 10 .";

            // Act
            consoleInOutUtils.PrintBoardInformation(iterations, activeCellsCount);

            // Assert
            inputManager.Verify(x => x.WriteLine(expectedText), Times.Once);
        }

        [Fact]
        public void RunNewGame_CreatesNewGame()
        {
            // Arrange
            int expectedRows = 5;
            int expectedColumns = 5;
            Game expectedGame = new Game(expectedRows, expectedColumns);

            inputManager.SetupSequence(x => x.ReadLine())
                .Returns("5")
                .Returns("5");

            // Act
            consoleInOutUtils.RunNewGame(out int rows, out int columns, out Game game);

            // Assert
            Assert.Equal(expectedRows, rows);
            Assert.Equal(expectedColumns, columns);
        }

        [Fact]
        public void RunOldGame_CreatesGameFromSavedFile()
        {
            // Arrange
            int expectedRows = 0;
            int expectedColumns = 0;
            bool[,] expectedBoard = new bool[expectedRows, expectedColumns];

            fileInOutUtils.Setup(x => x.ReadFromFile(out expectedRows, out expectedColumns, out expectedBoard, "game.txt"));

            // Act
            consoleInOutUtils.RunOldGame(out int rows, out int columns, out Game game);

            // Assert
            Assert.Equal(expectedRows, rows);
            Assert.Equal(expectedColumns, columns);
        }

        [Fact]
        public void PrintBoard_CorrectlyPrintsBoard()
        {
            // Arrange
            bool[,] board = mockData.BoardExample3on3();

            string expectedOutput = " X \r\n X \r\n X \r\n";

            // Act
            consoleInOutUtils.PrintBoard(board);

            // Assert
            inputManager.Verify(x => x.Write(expectedOutput), Times.Once);
        }

        [Fact]
        public void SetGame_SelectsNewGameType_CreatesNewGame()
        {
            // Arrange
            Game game;

            // Simulate user input for rows and columns
            inputManager.SetupSequence(x => x.ReadLine())
                .Returns("5")
                .Returns("5");

            int expectedRows = 5;
            int expectedColumns = 5;

            // Act
            consoleInOutUtils.SetGame(BoardType.New, out game);

            // Assert
            Assert.Equal(expectedRows, game.GetGameBoard().GetBoard().GetLength(0));
            Assert.Equal(expectedColumns, game.GetGameBoard().GetBoard().GetLength(1));
        }

        [Fact]
        public void SetGame_SelectsOldGame_CreatesGameFromSavedFile()
        {
            // Arrange
            Game game;

            // Simulate user selecting "old" game
            inputManager.Setup(x => x.ReadLine()).Returns("old");

            // Simulate reading game data from a file
            bool[,] expectedBoard = mockData.BoardExample3on3();
            int expectedRows = expectedBoard.GetLength(0);
            int expectedColumns = expectedBoard.GetLength(1);

            fileInOutUtils.Setup(x => x.ReadFromFile(out expectedRows, out expectedColumns, out expectedBoard, "game.txt"));

            // Act
            consoleInOutUtils.SetGame(BoardType.Old, out game);

            // Assert
            Assert.Equal(expectedRows, game.GetGameBoard().GetBoard().GetLength(0));
            Assert.Equal(expectedColumns, game.GetGameBoard().GetBoard().GetLength(1));
            Assert.Equal(expectedBoard, game.GetGameBoard().GetBoard());
        }
    }
}
