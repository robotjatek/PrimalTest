#pragma once
#include "IDrawable.h"

class Hero;
class IntVector2D;
class Level;

class Trap : public IGameObject {
private:
	IntVector2D _position;
	Level& _level;
public:
	Trap(Level& level, int x, int y);

	Trap(const Trap&) = delete;
	Trap& operator=(const Trap&) = delete;

	char getSprite() const override;

	void interact(Hero& hero) override;

	const IntVector2D& getPosition() const override;

	void draw(const EntityRenderer& entityRenderer) const override;
};