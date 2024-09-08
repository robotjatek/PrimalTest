#include "Monster.h"
#include "EntityRenderer.h"
#include "Hero.h"
#include "Level.h"

Monster::Monster(Level& level, int x, int y) : _level(level), _position(x, y) { }

char Monster::getSprite() const
{
	return 's';
}

const IntVector2D& Monster::getPosition() const
{
	return _position;
}

void Monster::draw(const EntityRenderer& entityRenderer) const
{
	entityRenderer.draw(*this);
}

void Monster::interact(Hero& hero)
{
	if (hero.hasSword()) {
		hero.setHealth(hero.getHealth() - 1);
		_level.removeGameObject(*this);
	}
	else {
		hero.kill();
	}
}
