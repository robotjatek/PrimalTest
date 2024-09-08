#include "Potion.h"
#include "EntityRenderer.h"
#include "Hero.h"
#include "IGameObjectContainer.h"

Potion::Potion(IGameObjectContainer& container, int x, int y) : _position(x, y), _container(container) { }

char Potion::getSprite() const
{
    return 'i';
}

const IntVector2D& Potion::getPosition() const
{
    return _position;
}

void Potion::draw(const EntityRenderer& entityRenderer) const
{
    entityRenderer.draw(*this);
}

void Potion::interact(Hero& hero)
{
    hero.setHealth(hero.getHealth() + 1);
    _container.removeGameObject(*this);
}
