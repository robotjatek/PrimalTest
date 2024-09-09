#pragma once
#include <string>
#include <vector>
#include <list>
#include <iostream>
#include <sstream>
#include <Windows.h>

#include "EntityRenderer.h"
#include "ICollider.h"
#include "IDeathHandler.h"
#include "IGameObjectContainer.h"
#include "ILeaveHandler.h"

class AIStateMachine;
class IGameObject;
class Hero;

enum AIState {
	ACTIVE,
	INACTIVE
};

enum GameState {
	RUNNING,
	FORFEIT,
	WIN,
	DEAD
};

class Level : public ICollider, public IDeathHandler, public IGameObjectContainer, public ILeaveHandler
{
private:
	std::vector<std::vector<bool>> _collisionData;
	std::list<std::shared_ptr<IGameObject>> _gameObjects;
	EntityRenderer _entityRenderer;
	std::shared_ptr<AIStateMachine> _ai;
	std::shared_ptr<Hero> _hero;
	GameState _gameState = GameState::RUNNING;
	AIState _aiState = AIState::INACTIVE;
	int _x;
	int _y;
public:
	Level(const std::string& levelData);
	void draw() const;
	void update();
	void setWall(int row, int column);
	void removeGameObject(IGameObject& gameObject);
	bool isCollidingWith(const Hero& hero) const override;
	bool isCollidingWith(const IntVector2D& point) const override;
	void onDeath() override;
	void onLeave(const Hero& hero) override;
	GameState getGameState() const;
	int getY() const;
	int getX() const;
	std::vector<std::vector<bool>> getCollisionData() const;
};

