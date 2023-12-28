namespace Savanna.Plugins
{
    public interface IAnimal
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public string? Symbol { get; set; }
        public int VisionRange { get; set; }
        public double Health { get; set; }
        public int RoundsNearOtherAnimal { get; set; }
        public bool IsPredator { get; set; }
    }
}