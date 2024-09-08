#include "Sword.h"
#include "EntityRenderer.h"
#include "IGameObjectContainer.h"
#include "Hero.h"

Sword::Sword(IGameObjectContainer& container, int x, int y) : _container(container), _position(x, y) {}

char Sword::getSprite() const
{
	return 'a';
}

const IntVector2D& Sword::getPosition() const
{
	return _position;
}

void Sword::draw(const EntityRenderer& entityRenderer) const
{
	entityRenderer.draw(*this);
}

void Sword::interact(Hero& hero)
{
	hero.addBackpackItem(std::dynamic_pointer_cast<IBackpackItem>(shared_from_this()));
	_container.removeGameObject(*this);
}
