using Savanna.Core;
using Savanna.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savanna.ConsoleApp
{
    public interface IGameManager
    {
        void RunGame();
        List<IAnimal> GetList();
        void MoveAnimalsOfType<T>() where T : IAnimal;
        void AddRandomlyPlacedAnimal(IAnimal animal);
        bool AnimalExistsAt(int x, int y);
    }
}
