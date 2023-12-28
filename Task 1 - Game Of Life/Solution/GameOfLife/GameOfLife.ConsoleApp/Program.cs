namespace GameOfLife.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            IConsoleInOutUtils consoleInOut = new ConsoleInOutUtils(new InputManager(), new FileInOutUtils());
            BoardType boardType = consoleInOut.CheckSelectedBoardType();
            Console.Clear();
            consoleInOut.SetGame(boardType, out game);

            while (true)
            {
                Console.Clear();
                consoleInOut.PrintBoard(game.GetGameBoard().GetBoard());
                var activeBoardCells = game.GetGameBoard().GetBoardActiveCell();
                consoleInOut.PrintBoardInformation(game.GetGameBoard().iteration, activeBoardCells);

                UserAction userAction = consoleInOut.CheckUserAction(game.GetGameBoard().GetBoard().GetLength(0), game.GetGameBoard().GetBoard().GetLength(1), game.GetGameBoard().GetBoard());
                if (UserAction.Continue == userAction)
                {
                    game.GetGameBoard().UpdateBoard();
                    continue;
                }
                else if (UserAction.Stop == userAction)
                {
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }
}