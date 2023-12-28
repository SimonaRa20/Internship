using Savanna.Core.Models;
using Savanna.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savanna.Core.Interfaces
{
    public interface IDeathLogic
    {
        bool CheckOrDead(IAnimal animal);
        void PredatorEatsNOnPredator(IAnimal animal);
        void AnimalMoves(IAnimal animal);
    }
}
