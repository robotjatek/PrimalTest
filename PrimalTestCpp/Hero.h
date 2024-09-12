#pragma once
#include "IDrawable.h"
#include <list>
#include <memory>
#include <vector>

class EntityRenderer;
class ICollider;
class IDeathHandler;
class IBackpackItem;

class Hero : public IDrawable {
private:
	IntVector2D _position;
	IntVector2D _lastPosition;
	ICollider& _collider;
	IDeathHandler& _deathHandler;
	std::list<std::shared_ptr<IGameObject>>& _gameObjects;
	std::vector<std::shared_ptr<IBackpackItem>> _backpackItems;
	int _health;
public:
	Hero(ICollider& collider, std::list<std::shared_ptr<IGameObject>>& gameObjects, int x, int y, IDeathHandler& deathHandler);
	Hero(const Hero&) = delete;
	Hero& operator=(const Hero&) = delete;
	char getSprite() const override;
	const IntVector2D& getPosition() const override;
	void draw(const EntityRenderer& entityRenderer) const override;
	void jumpOverTrap();
	void move(const IntVector2D& direction);
	void update();
	void kill();
	int getHealth() const;
	void setHealth(int health);
	bool hasSword() const;
	bool hasTreasure() const;
	void addBackpackItem(std::shared_ptr<IBackpackItem> item);
};
