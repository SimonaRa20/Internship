using GameOfLife.ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameOfLife.Tests
{
    public class InputManagerTests
    {
        [Fact]
        public void ReadLine_ReturnsUserInput()
        {
            // Arrange
            string userInput = "test input";
            var inputManager = new InputManager();
            var consoleInput = new StringReader(userInput);
            Console.SetIn(consoleInput);

            // Act
            string result = inputManager.ReadLine();

            // Assert
            Assert.Equal(userInput, result);
        }

        [Fact]
        public void Write_WritesMessageToConsole()
        {
            // Arrange
            string message = "test message";
            var inputManager = new InputManager();
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            inputManager.Write(message);

            // Assert
            Assert.Equal(message, consoleOutput.ToString());
        }

        [Fact]
        public void WriteLine_WritesMessageWithNewLineToConsole()
        {
            // Arrange
            string message = "test message";
            var inputManager = new InputManager();
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            inputManager.WriteLine(message);

            // Assert
            Assert.Equal(message + Environment.NewLine, consoleOutput.ToString());
        }
    }
}
