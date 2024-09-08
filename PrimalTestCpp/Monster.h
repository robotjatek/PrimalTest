#pragma once
#include "IDrawable.h"

class Level;

class Monster : public IGameObject
{
private:
	Level& _level;
	IntVector2D _position;
public:
	Monster(Level& level, int x, int y);
	char getSprite() const override;
	const IntVector2D& getPosition() const override;
	void draw(const EntityRenderer& entityRenderer) const override;
	void interact(Hero& hero) override;
};

