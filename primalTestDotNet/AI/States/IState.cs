using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public interface IState
{
    void Update(Hero hero, IEnumerable<IGameObject> gameObjects);
}
