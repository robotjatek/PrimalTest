using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public class LookingForSwordState(AIStateMachine context, Level.Level level) : StateBase(level), IState
{
    public void Update(Hero hero, IEnumerable<IGameObject> gameObjects)
    {
        if (hero.HasSword)
        {
            context.ChangeState(context.LookingForTreasureState);
            return;
        }

        // Look for a sword without monsters on the path
        var allMonsters = gameObjects.Where(o => o is Monster);
        var sword = gameObjects.Where(o => o is Sword).FirstOrDefault();
        if (sword == null)
        {
            context.ChangeState(context.UnwinnableState);
            return;
        }
        // Consider monsters as walls in that case
        var recalculatedDistanceData = CalculateDistanceData(hero.Position, allMonsters);
        var pathToSword = GetPath(hero.Position, sword.Position, recalculatedDistanceData);

        var node = pathToSword.Last();
        var direction = node - hero.Position;
        hero.Move(direction);
    }
}
