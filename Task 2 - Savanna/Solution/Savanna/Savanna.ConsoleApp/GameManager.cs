using Savanna.Core;
using Savanna.Core.Controller;
using Savanna.Core.Interfaces;
using Savanna.Plugins;

namespace Savanna.ConsoleApp
{
    public class GameManager : IGameManager
    {
        private List<IAnimal> animals = new();
        private readonly InOutUtils inOutUtils;
        private readonly IMovementLogic movementLogic = new MovementLogic();
        private readonly IInputManager inputManager;
        private readonly Random random = new();

        public GameManager(InOutUtils inOut, IInputManager input)
        {
            inOutUtils = inOut;
            inputManager = input;
        }

        public void RunGame()
        {
            ConsoleKey? key = null;

            while (key != ConsoleKey.Escape)
            {
                key = inputManager.ReadKey();
                IAnimal animalToAdd = inOutUtils.GetAnimalFromInput(key);
                if (animalToAdd != null)
                {
                    AddRandomlyPlacedAnimal(animalToAdd);
                }

                inOutUtils.ClearBoard();
                MoveAnimalsOfType<IAnimal>();
                animals.RemoveAll(x => !x.IsAlive);
                inOutUtils.Display();
                
            }
        }

        public List<IAnimal> GetList()
        {
            return animals;
        }

        public void MoveAnimalsOfType<T>() where T : IAnimal
        {
            var animalsOfTypeT = animals.OfType<T>().ToList();
            foreach (var animal in animalsOfTypeT)
            {
                if (!animal.IsPredator)
                {
                    movementLogic.MoveNonPredator(inOutUtils, animals, animal);
                    inOutUtils.DrawAnimal(animal);
                }
                else
                {
                    movementLogic.MovePredator(inOutUtils, animals, animal);
                    inOutUtils.DrawAnimal(animal);
                }

            }
        }

        public void AddRandomlyPlacedAnimal(IAnimal animal)
        {
            for (int attempt = 0; attempt < 100; attempt++)
            {
                int x = random.Next(GameConstants.BoardWidth);
                int y = random.Next(GameConstants.BoardHeight);

                if (!AnimalExistsAt(x, y))
                {
                    animal.X = x;
                    animal.Y = y;
                    animals.Add(animal);
                    break;
                }
            }
        }

        public bool AnimalExistsAt(int x, int y)
        {
            return animals.Exists(animal => animal.X == x && animal.Y == y);
        }
    }
}
