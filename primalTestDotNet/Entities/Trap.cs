namespace PrimalTestDotNet.Entities;

class Trap(Level.Level level, int x, int y) : IGameObject
{
    public char Sprite => 'c';

    public IntVector2 Position => new(x, y);

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }

    public void Visit(Hero hero)
    {
        level.RemoveGameObject(this);
        level.SetWall(Position.Y, Position.X);
        hero.JumpOverTrap();
    }
}
