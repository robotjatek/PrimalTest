using PrimalTestDotNet.Level;

namespace PrimalTestDotNet.Entities;

class Monster(IGameObjectContainer container, int x, int y) : IGameObject
{
    public char Sprite => 's';

    public IntVector2 Position { get; } = new(x, y);

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }

    public void Visit(Hero hero)
    {
        if (hero.HasSword)
        {
            hero.Health--;
            container.RemoveGameObject(this);
        }
        else
        {
            hero.Kill();
        }
    }
}
