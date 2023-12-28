namespace GameOfLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Game of Life");
            GameOfLife game = new GameOfLife();
            ConsoleInOutUtils.CheckNewOrExistGame(out game);
            game.Start();
        }
    }
}