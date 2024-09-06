using PrimalTestDotNet.Entities;
using PrimalTestDotNet.Level;

namespace PrimalTestDotNet;

class Hero(ICollider collider, int x, int y, IEnumerable<IGameObject> gameObjects, IDeathHandler deathHandler) : IDrawable
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

    // TODO: IGameObject BackPack?
    public bool HasSword { get; set; } = false;
    public bool HasTreasure { get; set; } = false;
    public char Sprite => 'h';

    public void InteractWith(IGameObject interactable)
    {
        interactable.Visit(this);
    }

    public void Kill()
    {
        Health = 0;
    }

    public void Update()
    {
        foreach (var item in gameObjects.ToList())
        {
            if (item.Position.X == Position.X && item.Position.Y == Position.Y)
                item.Visit(this);
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
