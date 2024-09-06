using PrimalTestDotNet.Level;

namespace PrimalTestDotNet.Entities;

class Sword(IGameObjectContainer container, int x, int y) : IGameObject
{
    public char Sprite => 'a';

    public IntVector2 Position => new(x, y);

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }

    public void Visit(Hero hero)
    {
        hero.HasSword = true;
        container.RemoveGameObject(this);
    }
}
