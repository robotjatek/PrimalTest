namespace PrimalTestDotNet.Entities;

interface IGameObject : IDrawable
{
    void Visit(Hero hero);
}
