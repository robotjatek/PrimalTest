using PrimalTestDotNet.AI.States;
using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI;

public class AIStateMachine
{
    private IState _state;
    private readonly Level.Level _level;
    private readonly Hero _hero;
    private readonly List<IGameObject> _gameObjects;

    public IState LeaveLevelState => new LeavingLevelState(this, _level);
    public IState UnwinnableState => new UnwinnableState(_level);
    public IState LookingForPotionState => new LookingForPotionState(this, _level);
    public IState LookingForSwordState => new LookingForSwordState(this, _level);
    public IState LookingForTreasureState => new LookingForTreasureState(this, _level);

    public AIStateMachine(Level.Level level, Hero hero, List<IGameObject> gameObjects)
    {
        _state = new LookingForTreasureState(this, level);

        _level = level;
        _hero = hero;
        _gameObjects = gameObjects;
    }

    public void ChangeState(IState state)
    {
        _state = state;
    }

    public void Update()
    {
        _state.Update(_hero, _gameObjects);
    }
}