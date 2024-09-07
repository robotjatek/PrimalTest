using PrimalTestDotNet.Level;

namespace PrimalTestDotNet.Entities;

class Sword(IGameObjectContainer container, int x, int y) : IGameObject, IBackpackItem
{
    public char Sprite => 'a';

    public IntVector2 Position => new(x, y);

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }

    public void Interact(Hero hero)
    {
        hero.AddItemToBackpack(this);
        container.RemoveGameObject(this);
    }
}
