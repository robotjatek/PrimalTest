namespace PrimalTestDotNet;

public class EntityRenderer
{
    public void Draw(IDrawable drawable)
    {
        Console.SetCursorPosition(drawable.Position.X, drawable.Position.Y);
        Console.Write(drawable.Sprite);
    }
}
