using Savanna.Plugins;

namespace Savanna.AnimalPluginImplementation
{
    // this class for testing other animals plugin
    public class Tiger : IAnimal
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public string? Symbol { get; set; }
        public int VisionRange { get; set; }
        public double Health { get; set; }
        public int RoundsNearOtherAnimal { get; set; }
        public bool IsPredator { get; set; }
        public Tiger()
        {
            Symbol = "T";
            IsPredator = true;
            IsAlive = true;
            VisionRange = 1;
            Health = 15;
            RoundsNearOtherAnimal = 0;
        }
    }
}