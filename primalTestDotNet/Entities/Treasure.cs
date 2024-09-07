using PrimalTestDotNet.Level;

namespace PrimalTestDotNet.Entities;

class Treasure(IGameObjectContainer container, int x, int y) : IGameObject, IBackpackItem
{
    public char Sprite => 'k';

    public IntVector2 Position => new(x, y);

    public void Interact(Hero hero)
    {
        hero.AddItemToBackpack(this);
        container.RemoveGameObject(this);
    }

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }
}
