namespace PrimalTestDotNet.Entities;

public interface IGameObject : IDrawable
{
    void Interact(Hero hero);
}
