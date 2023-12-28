using Savanna.Core.Interfaces;
using Savanna.Core.Models;
using Savanna.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savanna.Core.Controller
{
    public class MovementLogic : IMovementLogic
    {
        private readonly IDeathLogic deathLogic = new DeathLogic();
        private readonly IBirthLogic birthLogic = new BirthLogic();
        private T FindNearestAnimal<T>(List<T> animals, bool predator) where T : IAnimal
        {
            T nearestAnimal = default(T);
            double minDistance = double.MaxValue;

            if(predator)
            {
                foreach (T animal in animals.Where(a => a.IsPredator))
                {
                    double distance = CalculateDistance(animal.X, animal.Y, nearestAnimal);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestAnimal = animal;
                    }
                }
            }
            else
            {
                foreach (T animal in animals.Where(a => !a.IsPredator))
                {
                    double distance = CalculateDistance(animal.X, animal.Y, nearestAnimal);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestAnimal = animal;
                    }
                }
            }
            

            return nearestAnimal;
        }
        private double CalculateDistance(int x, int y, IAnimal animal)
        {
            if (animal == null)
            {
                return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            }
            else
            {
                return Math.Sqrt(Math.Pow(x - animal.X, 2) + Math.Pow(y - animal.Y, 2));
            }
        }

        public void MoveNonPredator(InOutUtils game, List<IAnimal> animals, IAnimal nonPredator)
        {
            if (!deathLogic.CheckOrDead(nonPredator))
            {
                Random random = new();
                int newX = nonPredator.X;
                int newY = nonPredator.Y;
                deathLogic.AnimalMoves(nonPredator);

                List<IAnimal> predatorsInRange = animals.OfType<IAnimal>()
                    .Where(predator => predator.IsPredator && Math.Abs(predator.X - nonPredator.X) <= nonPredator.VisionRange && Math.Abs(predator.Y - nonPredator.Y) <= nonPredator.VisionRange)
                    .ToList();

                birthLogic.AddNewBorn(game, animals, nonPredator);

                if (predatorsInRange.Count > 0)
                {
                    IAnimal nearestPredator = FindNearestAnimal(predatorsInRange, true);
                    if(nearestPredator != null)
                    {
                        newX = nonPredator.X + (nonPredator.X - nearestPredator.X);
                        newY = nonPredator.Y + (nonPredator.Y - nearestPredator.Y);
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int directionX = random.Next(-1, 2);
                        int directionY = random.Next(-1, 2);

                        newX = nonPredator.X + directionX;
                        newY = nonPredator.Y + directionY;

                        if (newX >= 0 && newX < GameConstants.BoardWidth && newY >= 0 && newY < GameConstants.BoardHeight)
                        {
                            break;
                        }
                    }
                }

                nonPredator.X = newX;
                nonPredator.Y = newY;
            }
            else
            {
                nonPredator.IsAlive = false;
            }
        }

        public void MovePredator(InOutUtils game, List<IAnimal> animals, IAnimal predator)
        {
            if (!deathLogic.CheckOrDead(predator))
            {
                Random random = new();
                int newX = predator.X;
                int newY = predator.Y;
                deathLogic.AnimalMoves(predator);

                List<IAnimal> nonPredatorsInRange = animals.OfType<IAnimal>()
                    .Where(nonPredator => !nonPredator.IsPredator && Math.Abs(nonPredator.X - predator.X) <= predator.VisionRange && Math.Abs(nonPredator.Y - predator.Y) <= predator.VisionRange)
                    .ToList();

                birthLogic.AddNewBorn(game, animals, predator);

                if (nonPredatorsInRange.Count > 0)
                {
                    IAnimal nearestNonPredator = FindNearestAnimal(nonPredatorsInRange, false);
                    if(nearestNonPredator != null)
                    {
                        newX = nearestNonPredator.X;
                        newY = nearestNonPredator.Y;
                        if (AreCoordinatesNextTo(predator.X, predator.Y, newX, newY))
                        {
                            nearestNonPredator.IsAlive = false;
                            deathLogic.PredatorEatsNOnPredator(predator);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int directionX = random.Next(-1, 2);
                        int directionY = random.Next(-1, 2);

                        newX = predator.X + directionX;
                        newY = predator.Y + directionY;

                        if (newX >= 0 && newX < GameConstants.BoardWidth && newY >= 0 && newY < GameConstants.BoardHeight)
                        {
                            break;
                        }
                    }
                }

                predator.X = newX;
                predator.Y = newY;
            }
            else
            {
                predator.IsAlive = false;
            }

        }
        private bool AreCoordinatesNextTo(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);

            return dx <= 1 && dy <= 1;
        }
    }
}
