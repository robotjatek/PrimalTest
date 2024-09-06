using PrimalTestDotNet.AI;
using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.Level;

public class Level : IGameObjectContainer, ILeaveHandler, ICollider, IDeathHandler
{
    private readonly List<IGameObject> _gameObjects = [];
    public bool[,] CollisionData { get; private set; }
    private readonly Hero _hero;
    private readonly EntityRenderer _entityRenderer = new();
    private readonly AIStateMachine _ai;

    // TODO: state machine
    public GameState GameState { get; private set; } = GameState.RUNNING;
    public AIState AIState { get; private set; } = AIState.INACTIVE;

    public int X { get; private set; }
    public int Y { get; private set; }

    public Level(string levelData)
    {
        var lines = levelData.Split(Environment.NewLine);
        var rows = lines.Length;
        var columns = levelData.Length == 0 ? 0 : lines[0].Length;
        X = columns;
        Y = rows;
        if (rows > 0 && columns > 0)
        {
            CollisionData = new bool[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var cell = lines[i][j];
                    switch (cell)
                    {
                        case 'x':
                            CollisionData[i, j] = true;
                            break;
                        case 'c':
                            _gameObjects.Add(new Trap(this, j, i));
                            break;
                        case 'i':
                            _gameObjects.Add(new Potion(this, j, i));
                            break;
                        case 'a':
                            _gameObjects.Add(new Sword(this, j, i));
                            break;
                        case 's':
                            _gameObjects.Add(new Monster(this, j, i));
                            break;
                        case 'h':
                            if (_hero != null)
                                throw new InvalidOperationException("Hero is already initialized");
                            _hero = new Hero(this, j, i, _gameObjects, this);
                            break;
                        case 'k':
                            _gameObjects.Add(new Treasure(this, j, i));
                            break;
                        case 'j':
                            _gameObjects.Add(new Exit(this, j, i));
                            break;
                    }
                }
            }

            _ai = new AIStateMachine(this, _hero!, _gameObjects);
        }
        else
            throw new Exception("Invalid level data");

        if (_hero == null)
            throw new InvalidOperationException("Hero is missing from level data");
    }

    public void OnLeave(Hero hero)
    {
        if (hero.HasTreasure)
            GameState = GameState.WIN;
        else
            GameState = GameState.FORFEIT;
    }

    public void RemoveGameObject(IGameObject gameObject)
    {
        _gameObjects.Remove(gameObject);
        // Potential Dispose() here
    }

    public void Draw()
    {
        Console.Clear();
        for (int i = 0; i < CollisionData.GetLength(0); i++)
        {
            for (int j = 0; j < CollisionData.GetLength(1); j++)
            {
                Console.Write(CollisionData[i, j] ? 'x' : ' ');
            }
            Console.WriteLine();
        }

        _hero.Draw(_entityRenderer);
        _gameObjects.ForEach(o => o.Draw(_entityRenderer));

        Console.SetCursorPosition(CollisionData.GetLength(1) + 1, 0);
        Console.Write("Health: " + _hero.Health);
        Console.SetCursorPosition(CollisionData.GetLength(1) + 1, 1);
        Console.Write("Sword acquired: " + _hero.HasSword);
        Console.SetCursorPosition(CollisionData.GetLength(1) + 1, 2);
        Console.Write("Treasure acquired: " + _hero.HasTreasure);
    }

    public void Update()
    {
        _hero.Update();
        HandleInput();
        if (AIState == AIState.ACTIVE)
        {
            _ai.Update();
            Thread.Sleep(100);
        }
    }

    private void HandleInput()
    {
        if (AIState == AIState.INACTIVE)
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    _hero.Move(IntVector2.UnitX * -1);
                    break;
                case ConsoleKey.RightArrow:
                    _hero.Move(IntVector2.UnitX);
                    break;
                case ConsoleKey.UpArrow:
                    _hero.Move(IntVector2.UnitY * -1);
                    break;
                case ConsoleKey.DownArrow:
                    _hero.Move(IntVector2.UnitY);
                    break;
                case ConsoleKey.Escape:
                    GameState = GameState.FORFEIT;
                    break;
                case ConsoleKey.Enter:
                    AIState = AIState.ACTIVE;
                    break;
            }
        }
    }

    public bool IsWall(int row, int column)
    {
        return CollisionData[row, column];
    }

    public void SetWall(int row, int column)
    {
        CollisionData[row, column] = true;
    }

    public bool IsCollidingWith(IGameObject gameObject)
    {
        if (gameObject.Position.X > CollisionData.GetLength(1) || gameObject.Position.Y > CollisionData.GetLength(0) ||
            gameObject.Position.X < 0 || gameObject.Position.Y < 0)
            return true;

        return CollisionData[gameObject.Position.Y, gameObject.Position.X];
    }

    public bool IsCollidingWith(Hero hero)
    {
        return CollisionData[hero.Position.Y, hero.Position.X];
    }

    public bool IsCollidingWith(IntVector2 position)
    {
        return CollisionData[position.Y, position.X];
    }

    public void OnDeath()
    {
        GameState = GameState.DEAD;
    }

    public void OnStuck()
    {
        GameState = GameState.STUCK;
    }
}
