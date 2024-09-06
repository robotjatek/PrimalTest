namespace PrimalTestDotNet.Entities;

public interface IGameObject : IDrawable
{
    void Visit(Hero hero);
}
