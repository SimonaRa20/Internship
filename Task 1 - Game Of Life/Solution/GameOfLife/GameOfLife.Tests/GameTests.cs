using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameOfLife.Tests
{
    public class GameTests
    {
        [Fact]
        public void DefaultConstructor_ShouldCreateInstance()
        {
            // Arrange and Act
            var runGame = new ConsoleApp.Game();

            // Assert
            Assert.NotNull(runGame);
        }

        [Fact]
        public void ConstructorWithRowsAndColumns_ShouldCreateInstanceWithBoardInitialized()
        {
            // Arrange
            int rows = 3;
            int columns = 3;

            // Act
            var runGame = new ConsoleApp.Game(rows, columns);

            // Assert
            Assert.NotNull(runGame);
        }
    }
}
