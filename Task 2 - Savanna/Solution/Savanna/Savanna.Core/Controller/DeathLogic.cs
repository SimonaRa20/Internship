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
    public class DeathLogic : IDeathLogic
    {
        public bool CheckOrDead(IAnimal animal)
        {
            if (animal.Health > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void PredatorEatsNOnPredator(IAnimal animal)
        {
            animal.Health += 2; 
        }

        public void AnimalMoves(IAnimal animal)
        {
            animal.Health -= 0.5;
        }
    }
}
