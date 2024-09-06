using PrimalTestDotNet.Level;

namespace PrimalTestDotNet.Entities;

class Treasure(IGameObjectContainer container, int x, int y) : IGameObject
{
    public char Sprite => 'k';

    public IntVector2 Position => new(x, y);

    public void Visit(Hero hero)
    {
        hero.HasTreasure = true;
        container.RemoveGameObject(this);
    }

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }
}
