using Savanna.Core.Models;
using Savanna.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savanna.Core.Interfaces
{
    public interface IBirthLogic
    {
        void AddNewBorn(InOutUtils game, List<IAnimal> animals, IAnimal animal);
    }
}
