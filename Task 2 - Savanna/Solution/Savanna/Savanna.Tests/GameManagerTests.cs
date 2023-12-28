using Moq;
using Savanna.Core.Interfaces;
using Savanna.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savanna.Core.Models;
using Xunit;
using Savanna.ConsoleApp;
using Savanna.Plugins;

namespace Savanna.Tests
{
    public class GameManagerTests
    {
        private readonly InOutUtils inOutUtils;
        private readonly Mock<IInputManager> mockInputManager;
        public readonly IGameManager gameManager;

        public GameManagerTests()
        {
            mockInputManager = new Mock<IInputManager>();
            inOutUtils = new InOutUtils(mockInputManager.Object);
            gameManager = new GameManager(inOutUtils, mockInputManager.Object);
        }

        [Fact]
        public void AddRandomlyPlacedAnimal_AddsAnimalToAnimalsList()
        {
            // Arrange
            var animal = new Antelope();

            // Act
            gameManager.AddRandomlyPlacedAnimal(animal);

            // Assert
            Assert.Contains(animal, gameManager.GetList());
        }

        [Fact]
        public void AnimalExistsAt_WhenAnimalExists_ReturnsTrue()
        {
            // Arrange
            var gameManager = new GameManager(inOutUtils, mockInputManager.Object);
            var animal = new Antelope { X = 1, Y = 2 };
            gameManager.GetList().Add(animal);

            // Act
            bool exists = gameManager.AnimalExistsAt(1, 2);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void RunGame_ExitKeyStopsGame()
        {
            // Arrange
            var gameManager = new GameManager(inOutUtils, mockInputManager.Object);
            mockInputManager.Setup(m => m.ReadKey()).Returns(ConsoleKey.Escape);

            // Act
            gameManager.RunGame();

            // Assert
            Assert.Empty(gameManager.GetList());
        }

        [Fact]
        public void MoveAnimalsOfType_MoveAnimals()
        {
            // Arrange
            var gameManager = new GameManager(inOutUtils, mockInputManager.Object);
            var lion = new Lion()
            {
                X = 0,
                Y = 0
            };
            gameManager.AddRandomlyPlacedAnimal(lion);

            // Act
            gameManager.MoveAnimalsOfType<IAnimal>();

            bool move = false;
            if (lion.X != 0 || lion.Y != 0)
            {
                move = true;
            }

            // Assert
            Assert.True(move);
        }
    }
}
