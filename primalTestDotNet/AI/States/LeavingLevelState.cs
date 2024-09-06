using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public class LeavingLevelState(AIStateMachine context, Level.Level level) : StateBase(level), IState
{
    public void Update(Hero hero, IEnumerable<IGameObject> gameObjects)
    {
        var distanceData = CalculateDistanceData(hero.Position, []);
        // Try to exit the level
        var exit = gameObjects.First(o => o is Exit);
        var pathToExit = GetPath(hero.Position, exit.Position, distanceData);
        var node = pathToExit.Last();
        var direction = node - hero.Position;
        if (direction.Length > 1)
        {
            // A hero only can move one cell. If the path has longer movement vector it must be the end-goal without any real paths to it
            context.ChangeState(context.UnwinnableState);
            return;
        }

        hero.Move(direction);
    }
}
