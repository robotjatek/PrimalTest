using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public class LookingForPotionState(AIStateMachine context, Level.Level level) : StateBase(level), IState
{
    public void Update(Hero hero, IEnumerable<IGameObject> gameObjects)
    {
        if (hero.Health > 1)
        {
            context.ChangeState(context.LookingForTreasureState);
            return;
        }

        // Look for potion
        var allMonsters = gameObjects.Where(o => o is Monster);
        var recalculatedDistanceData = CalculateDistanceData(hero.Position, allMonsters);
        var potion = gameObjects.Where(o => o is Potion).FirstOrDefault();
        if (potion == null)
        {
            context.ChangeState(context.UnwinnableState);
            return;
        }
        var pathToPotion = GetPath(hero.Position, potion.Position, recalculatedDistanceData);

        var node = pathToPotion.Last();
        var direction = node - hero.Position;
        hero.Move(direction);
    }
}
