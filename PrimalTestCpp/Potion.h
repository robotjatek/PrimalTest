#pragma once
#include "IDrawable.h"

class IGameObjectContainer;

class Potion : public IGameObject
{
private:
	IntVector2D _position;
	IGameObjectContainer& _container;
public:
	Potion(IGameObjectContainer& container, int x, int y);
	char getSprite() const override;
	const IntVector2D& getPosition() const override;
	void draw(const EntityRenderer& entityRenderer) const override;
	void interact(Hero& hero) override;
};

