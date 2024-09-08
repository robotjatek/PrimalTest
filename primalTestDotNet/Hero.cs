using PrimalTestDotNet.Entities;
using PrimalTestDotNet.Level;

namespace PrimalTestDotNet;

public class Hero(ICollider collider, int x, int y, IEnumerable<IGameObject> gameObjects, IDeathHandler deathHandler) : IDrawable
{
    public IntVector2 Position { get; set; } = new IntVector2(x, y);
    private IntVector2 LastPosition { get; set; } = new IntVector2(x, y);

    private int _health = 2;

    public int Health
    {
        get => _health;
        set
        {
            _health = Math.Clamp(value, 0, 2);
            if (_health == 0)
                deathHandler.OnDeath();
        }
    }

    public char Sprite => 'h';

    private readonly HashSet<IBackpackItem> _backpackItems = [];

    public bool HasSword => _backpackItems.Any(i => i is Sword);

    public bool HasTreasure => _backpackItems.Any(i => i is Treasure);

    public void AddItemToBackpack(IBackpackItem item)
    {
        _backpackItems.Add(item);
    }

    public void InteractWith(IGameObject interactable)
    {
        interactable.Interact(this);
    }

    public void Kill()
    {
        Health = 0;
    }

    public void Update()
    {
        // Iterating through a copy: this way the application wont crash on item removal
        foreach (var item in gameObjects.ToList())
        {
            if (item.Position.X == Position.X && item.Position.Y == Position.Y)
                item.Interact(this);
        }
    }

    public void Move(IntVector2 direction)
    {
        LastPosition = new IntVector2(Position.X, Position.Y);
        Position += direction;
        if (collider.IsCollidingWith(this))
            Position -= direction;
    }

    public void Draw(EntityRenderer renderer)
    {
        renderer.Draw(this);
    }

    public void JumpOverTrap()
    {
        var direction = Position - LastPosition;
        var nextPosition = Position + direction;

        if (collider.IsCollidingWith(nextPosition))
            Kill();
        else
            Move(direction);
    }
}
