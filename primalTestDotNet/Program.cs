namespace PrimalTestDotNet;

internal class Program
{
    static void Main()
    {
        var levelData = File.ReadAllText("level1.txt");
        var level = new Level.Level(levelData);
        while (level.GameState == Level.GameState.RUNNING)
        {
            level.Draw();
            level.Update();
        }

        level.Draw();
        WriteEnd(level);
        Console.ReadKey();
    }

    private static void WriteEnd(Level.Level level)
    {
        Console.SetCursorPosition(0, level.Y);
        switch (level.GameState)
        {
            case Level.GameState.DEAD:
                Console.WriteLine("Game over");
                break;
            case Level.GameState.WIN:
                Console.WriteLine("You win");
                break;
            case Level.GameState.FORFEIT:
                Console.WriteLine("Forfeit...");
                break;
            default:
                throw new InvalidOperationException("Invalid state at the end");
        }
    }
}
