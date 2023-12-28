using Moq;
using Savanna.Core.Controller;
using Savanna.Core.Interfaces;
using Savanna.Core.Models;
using Savanna.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Savanna.Plugins;

namespace Savanna.Tests
{
    public class AnimalBirthTests
    {
        private readonly Mock<IInputManager> mockInputManager;
        private readonly IBirthLogic birthLogic;
        private readonly InOutUtils inOutUtils;

        public AnimalBirthTests()
        {
            mockInputManager = new();
            inOutUtils = new(mockInputManager.Object);
            birthLogic = new BirthLogic();
        }

        [Fact]
        public void Antelope_AddNewBorn_NearAnotherAntelope_CreatesNewAntelope()
        {
            // Arrange
            var antelope1 = new Antelope()
            {
                X = 1,
                Y = 1,
                VisionRange = 1,
                RoundsNearOtherAnimal = 2
            };

            var antelope2 = new Antelope()
            {
                X = 2,
                Y = 1,
                VisionRange = 1,
                RoundsNearOtherAnimal = 2
            };

            var animals = new List<IAnimal> { antelope1, antelope2 };

            // Act
            birthLogic.AddNewBorn(inOutUtils, animals, antelope1);

            // Assert
            Assert.Equal(3, animals.Count);
        }

        [Fact]
        public void Lion_AddNewBorn_NearAnotherLion_CreatesNewLion()
        {
            // Arrange
            var lion1 = new Lion()
            {
                X = 1,
                Y = 1,
                VisionRange = 1,
                RoundsNearOtherAnimal = 2
            };

            var lion2 = new Lion()
            {
                X = 2,
                Y = 1,
                VisionRange = 1,
                RoundsNearOtherAnimal = 2
            };

            var animals = new List<IAnimal> { lion1, lion2 };

            // Act
            birthLogic.AddNewBorn(inOutUtils, animals, lion1);

            // Assert
            Assert.Equal(3, animals.Count);
        }

    }
}
