using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.Level;

interface ICollider
{
    bool IsCollidingWith(IGameObject gameObject);
    bool IsCollidingWith(Hero hero);
    bool IsCollidingWith(IntVector2 position);
}
