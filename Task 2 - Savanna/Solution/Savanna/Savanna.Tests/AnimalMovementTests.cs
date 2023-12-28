using Moq;
using Savanna.Core;
using Savanna.Core.Controller;
using Savanna.Core.Interfaces;
using Savanna.Core.Models;
using Savanna.Plugins;
using Xunit;

namespace Savanna.Tests
{
    public class AnimalMovementTests
    {
        private readonly Mock<IInputManager> mockInputManager;
        private readonly IMovementLogic movementLogic;
        private readonly InOutUtils inOutUtils;

        public AnimalMovementTests()
        {
            mockInputManager = new();
            inOutUtils = new(mockInputManager.Object);
            movementLogic = new MovementLogic();
        }

        [Fact]
        public void Lion_MoveTowardsNearestAntelope_UpdatesPosition()
        {
            // Arrange
            var lion = new Lion()
            {
                X = 0,
                Y = 0
            };
            var antelope = new Antelope()
            {
                X = 2,
                Y = 2
            };
            var animals = new List<IAnimal> { lion, antelope };

            // Act
            movementLogic.MovePredator(inOutUtils, animals, lion);
            movementLogic.MovePredator(inOutUtils, animals, lion);

            // Assert
            bool move = false;
            if (lion.X != 0 || lion.Y != 0)
            {
                move = true;
            }

            Assert.True(move);
        }

        [Fact]
        public void Antelope_MoveAwayFromNearestLion_UpdatesPosition()
        {
            // Arrange
            var antelope = new Antelope()
            {
                X = 3,
                Y = 3
            };
            var lion = new Lion()
            {
                X = 2,
                Y = 2
            };
            var animals = new List<IAnimal> { antelope, lion };

            // Act
            movementLogic.MoveNonPredator(inOutUtils, animals, antelope);

            // Assert
            Assert.NotEqual(3, antelope.X);
            Assert.NotEqual(3, antelope.Y);
        }

        [Fact]
        public void Antelope_MoveRandomlyWhenNoLionsAround_UpdatesPositionRandomly()
        {
            // Arrange
            var antelope = new Antelope()
            {
                X = 3,
                Y = 3,
                VisionRange = 1
            };
            var animals = new List<IAnimal> { antelope };

            int initialX = antelope.X;
            int initialY = antelope.Y;

            // Act
            movementLogic.MoveNonPredator(inOutUtils, animals, antelope);

            bool move = false;
            if (initialX != antelope.X || initialY != antelope.Y)
            {
                move = true;
            }

            // Assert
            Assert.True(move);
        }

        [Fact]
        public void Lion_AttackAdjacentAntelope_AntelopeIsKilled()
        {
            // Arrange
            var lion = new Lion()
            {
                X = 1,
                Y = 1,
                VisionRange = 1
            };

            var antelope = new Antelope()
            {
                X = 1,
                Y = 2,
                IsAlive = true
            };
            var animals = new List<IAnimal> { lion, antelope };

            // Act
            movementLogic.MovePredator(inOutUtils, animals, lion);

            // Assert
            Assert.False(antelope.IsAlive);
        }

        [Fact]
        public void Lion_DieForMoves()
        {
            // Arrange
            var lion = new Lion()
            {
                X = 1,
                Y = 1,
                VisionRange = 1,
                Health = 1
            };
            var animals = new List<IAnimal> { lion };

            // Act
            movementLogic.MovePredator(inOutUtils, animals, lion);
            movementLogic.MovePredator(inOutUtils, animals, lion);
            movementLogic.MovePredator(inOutUtils, animals, lion);

            // Assert
            Assert.False(lion.IsAlive);
        }

        [Fact]
        public void Antelope_DieForMoves()
        {
            // Arrange
            var antelope = new Antelope()
            {
                X = 1,
                Y = 1,
                VisionRange = 1,
                Health = 0.5
            };
            var animals = new List<IAnimal> { antelope };

            // Act
            movementLogic.MoveNonPredator(inOutUtils, animals, antelope);
            movementLogic.MoveNonPredator(inOutUtils, animals, antelope);

            // Assert
            Assert.False(antelope.IsAlive);
        }
    }
}
