#pragma once
#include "IDrawable.h"
#include "IBackpackItem.h"
#include <memory>

class IGameObjectContainer;

class Treasure : public IGameObject, public IBackpackItem, public std::enable_shared_from_this<Treasure>
{
private:
	IGameObjectContainer& _container;
	IntVector2D _position;
public:
	Treasure(IGameObjectContainer& container, int x, int y);
	Treasure(const Treasure&) = delete;
	Treasure& operator=(const Treasure&) = delete;
	char getSprite() const override;
	const IntVector2D& getPosition() const override;
	void draw(const EntityRenderer& entityRenderer) const override;
	void interact(Hero& hero) override;
};

