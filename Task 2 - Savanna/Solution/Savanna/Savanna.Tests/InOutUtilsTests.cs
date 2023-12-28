using Moq;
using Savanna.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savanna.Core.Interfaces;
using Xunit;
using Savanna.Core.Models;

namespace Savanna.Tests
{
    public class InOutUtilsTests
    {
        private readonly Mock<IInputManager> mockInputManager;
        private readonly InOutUtils inOutUtils;

        public InOutUtilsTests()
        {
            mockInputManager = new();
            inOutUtils = new(mockInputManager.Object);
        }

        [Fact]
        public void ClearBoard_ClearsBoardArray()
        {
            // Arrange

            // Act
            inOutUtils.ClearBoard();

            // Assert
            for (int x = 0; x < GameConstants.BoardWidth; x++)
            {
                for (int y = 0; y < GameConstants.BoardHeight; y++)
                {
                    Assert.Equal(".", inOutUtils.board[x, y]);
                }
            }
        }

        [Fact]
        public void DrawAnimal_DrawsAnimalSymbol()
        {
            // Arrange
            var animal = new Antelope { X = 4, Y = 4, Symbol = "A" };

            // Act
            inOutUtils.DrawAnimal(animal);

            // Assert
            Assert.Equal("A", inOutUtils.board[4, 4]);
        }
    }
}
