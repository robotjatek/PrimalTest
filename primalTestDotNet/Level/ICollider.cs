using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.Level;

public interface ICollider
{
    bool IsCollidingWith(IGameObject gameObject);
    bool IsCollidingWith(Hero hero);
    bool IsCollidingWith(IntVector2 position);
}
