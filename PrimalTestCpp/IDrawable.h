#pragma once

#include "IntVector2D.h"
class EntityRenderer;
class Hero;

class IDrawable {
public:
	virtual char getSprite() const = 0;
	virtual const IntVector2D& getPosition() const = 0;
	virtual void draw(const EntityRenderer& entityRenderer) const = 0;
};

class IGameObject : public IDrawable {
public:
	virtual void interact(Hero& hero) = 0;
};