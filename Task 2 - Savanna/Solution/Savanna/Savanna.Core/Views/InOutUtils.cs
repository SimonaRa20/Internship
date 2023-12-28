using Savanna.Core.Interfaces;
using Savanna.Core.Models;
using Savanna.Plugins;

namespace Savanna.Core
{
    public class InOutUtils
    {
        private readonly IInputManager inputManager;
        public string[,] board = new string[GameConstants.BoardWidth, GameConstants.BoardHeight];

        public InOutUtils(IInputManager inputManager)
        {
            this.inputManager = inputManager;
        }

        public void ClearBoard()
        {
            for (int x = 0; x < GameConstants.BoardWidth; x++)
            {
                for (int y = 0; y < GameConstants.BoardHeight; y++)
                {
                    board[x, y] = ".";
                }
            }
        }

        public void DrawAnimal(IAnimal animal)
        {
            int x = animal.X;
            int y = animal.Y;

            if (x >= 0 && x < GameConstants.BoardWidth && y >= 0 && y < GameConstants.BoardHeight)
            {
                board[x, y] = animal.Symbol;
            }
        }

        public void Display()
        {
            List<Type> animalTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => (typeof(IAnimal)).IsAssignableFrom(type) && !type.IsInterface).ToList();

            inputManager.Clear();
            inputManager.WriteLine("Welcome to the Savanna Game!");
            inputManager.WriteLine("");

            for (int y = 0; y < GameConstants.BoardHeight; y++)
            {
                for (int x = 0; x < GameConstants.BoardWidth; x++)
                {
                    inputManager.Write(board[x, y]);
                }
                inputManager.WriteLine("");
            }

            inputManager.WriteLine("");
            foreach (Type type in animalTypes)
            {
                string name = type.Name;
                string value = "Press " + name[0].ToString() + " to add " + name + " to the field.";
                inputManager.WriteLine(value);
            }
            inputManager.WriteLine("Press Esc to quit the game.");
        }

        public IAnimal GetAnimalFromInput(ConsoleKey? key)
        {
            if (key.HasValue)
            {
                string animalType = key.ToString();
                if (animalType.Length == 1)
                {
                    animalType = animalType.ToUpper();
                    Type type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(t => typeof(IAnimal).IsAssignableFrom(t) && !t.IsAbstract && t.Name.StartsWith(animalType))
                        .FirstOrDefault();


                    if (type != null)
                    {
                        return (IAnimal)Activator.CreateInstance(type);
                    }
                }
            }
            return null;
        }
    }
}
