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
    public class BirthLogic : IBirthLogic
    {
        public void AddNewBorn(InOutUtils game, List<IAnimal> animals, IAnimal animal)
        {
            List<IAnimal> animalsInRange = animals.OfType<IAnimal>()
                    .Where(a => a != animal && a.Symbol == animal.Symbol && Math.Abs(a.X - animal.X) <= animal.VisionRange && Math.Abs(a.Y - animal.Y) <= animal.VisionRange)
                    .ToList();

            if (animalsInRange.Count > 0)
            {
                animal.RoundsNearOtherAnimal++;
                if (animal.RoundsNearOtherAnimal == 3)
                {
                    var newAnimal = CreateAnimal(animal);
                    newAnimal.X = animal.X;
                    newAnimal.Y = animal.Y;
                    animals.Add(newAnimal);
                    animal.RoundsNearOtherAnimal = 0;
                }
            }
            else
            {
                animal.RoundsNearOtherAnimal = 0;
            }
        }

        private IAnimal CreateAnimal(IAnimal parentAnimal)
        {
            return (IAnimal)Activator.CreateInstance(parentAnimal.GetType());
        }
    }
}
