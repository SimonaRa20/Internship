using Savanna.Core;
using Savanna.Core.Interfaces;
using Savanna.Core.Views;
using Savanna.Plugins;

namespace Savanna.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {

            PluginLoader loader = new PluginLoader();
            loader.LoadPlugins();
            IInputManager inputManager = new InputManager();
            InOutUtils inOutUtils = new InOutUtils(inputManager);
            IGameManager gameManager = new GameManager(inOutUtils, inputManager);
            gameManager.RunGame();
        }
    }
}