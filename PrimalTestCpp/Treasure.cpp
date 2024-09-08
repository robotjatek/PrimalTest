#include "Treasure.h"
#include "EntityRenderer.h"
#include "Hero.h"
#include "IGameObjectContainer.h"

Treasure::Treasure(IGameObjectContainer& container, int x, int y) : _container(container), _position(x, y) { }

char Treasure::getSprite() const
{
    return 'k';
}

const IntVector2D& Treasure::getPosition() const
{
    return _position;
}

void Treasure::draw(const EntityRenderer& entityRenderer) const
{
    entityRenderer.draw(*this);
}

void Treasure::interact(Hero& hero)
{
    hero.addBackpackItem(std::dynamic_pointer_cast<IBackpackItem>(shared_from_this()));
    _container.removeGameObject(*this);
}
