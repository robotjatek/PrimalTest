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
        // TODO: state machine
        Console.SetCursorPosition(0, level.Y);
        if (level.GameState == Level.GameState.DEAD)
        {
            Console.WriteLine("Game over");
        }
        else if (level.GameState == Level.GameState.WIN)
        {
            Console.WriteLine("You win");
        }
        else if (level.GameState == Level.GameState.FORFEIT)
        {
            Console.WriteLine("Forfeit...");
        }
        else if (level.GameState == Level.GameState.STUCK)
        {
            Console.WriteLine("Stuck. Game over.");
        }
    }
}
