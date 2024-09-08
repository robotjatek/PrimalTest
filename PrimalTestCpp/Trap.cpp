#include "Trap.h"
#include "EntityRenderer.h"
#include "Level.h"
#include "Hero.h"

Trap::Trap(Level& level, int x, int y) : _position(x, y), _level(level) {}

char Trap::getSprite() const {
	return 'c';
}

void Trap::interact(Hero& hero) {
	_level.setWall(_position.getX(), _position.getY());
	_level.removeGameObject(*this);
	hero.jumpOverTrap();
}

const IntVector2D& Trap::getPosition() const {
	return _position;
}

void Trap::draw(const EntityRenderer& entityRenderer) const {
	entityRenderer.draw(*this);
}