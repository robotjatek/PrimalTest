using PrimalTestDotNet.Level;

namespace PrimalTestDotNet.Entities;

class Potion(IGameObjectContainer container, int x, int y) : IGameObject
{
    public char Sprite => 'i';

    public IntVector2 Position => new(x, y);

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }

    public void Interact(Hero hero)
    {
        hero.Health++;
        container.RemoveGameObject(this);
    }
}
