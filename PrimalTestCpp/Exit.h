#pragma once
#include "IDrawable.h"

class ILeaveHandler;

class Exit : public IGameObject
{
private:
	ILeaveHandler& _container;
	IntVector2D _position;

public:
	Exit(ILeaveHandler& container, int x, int y);
	char getSprite() const override;
	const IntVector2D& getPosition() const override;
	void draw(const EntityRenderer& entityRenderer) const override;
	void interact(Hero& hero) override;
};

