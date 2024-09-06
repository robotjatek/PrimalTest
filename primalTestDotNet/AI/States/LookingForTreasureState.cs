using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public class LookingForTreasureState(AIStateMachine context, Level.Level level) : StateBase(level), IState
{
    public void Update(Hero hero, IEnumerable<IGameObject> gameObjects)
    {
        var distanceData = CalculateDistanceData(hero.Position, []);
        if (hero.HasTreasure)
        {
            context.ChangeState(context.LeaveLevelState);
            return;
        }

        var treasure = gameObjects.FirstOrDefault(o => o is Treasure);
        if (treasure == null)
        {
            context.ChangeState(context.UnwinnableState);
            return;
        }
        var pathToTreasure = GetPath(hero.Position, treasure.Position, distanceData);
        var monstersOnPath = gameObjects.Where(o => o is Monster) // filter to monsters only
            .Where(m => pathToTreasure.Contains(m.Position)) // keep monsters that are on the path
            .ToArray();

        if (monstersOnPath.Length > 0 && hero.Health < 2)
        {
            context.ChangeState(context.LookingForPotionState);
            return;
        }
        else if (monstersOnPath.Length > 0 && !hero.HasSword)
        {
            context.ChangeState(context.LookingForSwordState);
            return;
        }
        else if (hero.HasTreasure)
        {
            context.ChangeState(context.LeaveLevelState);
            return;
        }
        else
        {
            // Go for the treasure
            var node = pathToTreasure.Last();
            var direction = node - hero.Position;
            if (direction.Length > 1)
            {
                context.ChangeState(context.UnwinnableState);
                return;
            }
            else
            {
                hero.Move(direction);
                return;
            }
        }
    }
}
