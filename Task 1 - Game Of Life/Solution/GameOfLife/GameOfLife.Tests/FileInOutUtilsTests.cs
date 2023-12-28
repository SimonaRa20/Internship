using GameOfLife.ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameOfLife.Tests
{
    public class FileInOutUtilsTests
    {
        private readonly MockData mockData;

        public FileInOutUtilsTests()
        {
            mockData = new MockData();
        }

        [Fact]
        public void SaveGameBoard3on3_ShouldSaveToFile()
        {
            bool[,] board = mockData.BoardExample3on3();
            string fileName = "boardGame3on3.txt";

            IFileInOutUtils fileInOutUtils = new FileInOutUtils();
            fileInOutUtils.SaveGameBoard(board.GetLength(0), board.GetLength(1), board, fileName);

            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public void SaveGameBoard3on3_ShouldSaveToFileWhichDoesNotExist()
        {
            bool[,] board = mockData.BoardExample3on3();
            string fileName = "Game3on3.txt";

            IFileInOutUtils fileInOutUtils = new FileInOutUtils();
            fileInOutUtils.SaveGameBoard(board.GetLength(0), board.GetLength(1), board, fileName);

            Assert.True(File.Exists(fileName));
            File.Delete(fileName);
        }

        [Fact]
        public void SaveGameBoard5on5_ShouldSaveToFile()
        {
            bool[,] board = mockData.BoardExample5on5();
            string fileName = "boardGame5on5.txt";

            IFileInOutUtils fileInOutUtils = new FileInOutUtils();
            fileInOutUtils.SaveGameBoard(board.GetLength(0), board.GetLength(1), board, fileName);

            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public void SaveGameBoard10on10_ShouldSaveToFile()
        {
            bool[,] board = mockData.BoardExample10on10();
            string fileName = "boardGame10on10.txt";

            IFileInOutUtils fileInOutUtils = new FileInOutUtils();
            fileInOutUtils.SaveGameBoard(board.GetLength(0), board.GetLength(1), board, fileName);

            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public void ReadFromFile_ShouldBeReadFile3on3()
        {
            bool[,] expectedBoard = mockData.BoardExample3on3();
            int expectedRows = expectedBoard.GetLength(0);
            int expectedColumns = expectedBoard.GetLength(1);
            string fileName = "boardGame3on3.txt";

            IFileInOutUtils fileInOutUtils = new FileInOutUtils();
            fileInOutUtils.ReadFromFile(out int rows, out int columns, out bool[,] board, fileName);

            Assert.Equal(expectedRows, rows);
            Assert.Equal(expectedColumns, columns);
            Assert.Equal(expectedBoard, board);
        }

        [Fact]
        public void ReadFromFile_ShouldBeReadFile5on5()
        {
            bool[,] expectedBoard = mockData.BoardExample5on5();
            int expectedRows = expectedBoard.GetLength(0);
            int expectedColumns = expectedBoard.GetLength(1);
            string fileName = "boardGame5on5.txt";

            IFileInOutUtils fileInOutUtils = new FileInOutUtils();
            fileInOutUtils.ReadFromFile(out int rows, out int columns, out bool[,] board, fileName);

            Assert.Equal(expectedRows, rows);
            Assert.Equal(expectedColumns, columns);
            Assert.Equal(expectedBoard, board);
        }

        [Fact]
        public void ReadFromFile_ShouldBeReadFile10on10()
        {
            bool[,] expectedBoard = mockData.BoardExample10on10();
            int expectedRows = expectedBoard.GetLength(0);
            int expectedColumns = expectedBoard.GetLength(1);
            string fileName = "boardGame10on10.txt";

            IFileInOutUtils fileInOutUtils = new FileInOutUtils();
            fileInOutUtils.ReadFromFile(out int rows, out int columns, out bool[,] board, fileName);

            Assert.Equal(expectedRows, rows);
            Assert.Equal(expectedColumns, columns);
            Assert.Equal(expectedBoard, board);
        }
    }
}
