using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public class UnwinnableState(Level.Level level) : StateBase(level), IState
{
    public void Update(Hero hero, IEnumerable<IGameObject> gameObjects)
    {
        var distanceData = CalculateDistanceData(hero.Position, []);
        // forfeit when unwinnable
        // try to exit
        var exit = gameObjects.FirstOrDefault(o => o is Exit);
        var pathToExit = GetPath(hero.Position, exit!.Position, distanceData);
        var node = pathToExit.Last();
        var direction = node - hero.Position;

        if (direction.Length > 1)
        {
            // If leaving without the treasure also fails => game over 
            _level.OnForfeit();
            return;
        }

        hero.Move(direction);
    }
}
