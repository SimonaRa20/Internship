using Savanna.Core.Models;
using Savanna.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savanna.Core.Interfaces
{
    public interface IMovementLogic
    {
        void MovePredator(InOutUtils game, List<IAnimal> animals, IAnimal predator);
        void MoveNonPredator(InOutUtils game, List<IAnimal> animals, IAnimal nonPredator);
    }
}
