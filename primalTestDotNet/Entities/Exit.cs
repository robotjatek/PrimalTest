using PrimalTestDotNet.Level;

namespace PrimalTestDotNet.Entities;

class Exit(ILeaveHandler container, int x, int y) : IGameObject
{
    public char Sprite { get => 'j'; }

    public IntVector2 Position { get; } = new(x, y);

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }

    public void Interact(Hero hero)
    {
        container.OnLeave(hero);
    }
}
