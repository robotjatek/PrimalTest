#pragma once
#include "IDrawable.h"
#include "IBackpackItem.h"
#include <memory>

class IGameObjectContainer;

class Sword : public IGameObject, public IBackpackItem, public std::enable_shared_from_this<Sword>
{
private:
	IGameObjectContainer& _container;
	IntVector2D _position;
public:
	Sword(IGameObjectContainer& level, int x, int y);
	Sword(const Sword&) = delete;
	Sword& operator=(const Sword&) = delete;
	char getSprite() const override;
	const IntVector2D& getPosition() const override;
	void draw(const EntityRenderer& entityRenderer) const override;
	void interact(Hero& hero) override;
};

