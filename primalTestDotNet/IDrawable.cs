namespace PrimalTestDotNet;

public interface IDrawable
{
    IntVector2 Position { get; }
    char Sprite { get; }
    void Draw(EntityRenderer renderer);
}
