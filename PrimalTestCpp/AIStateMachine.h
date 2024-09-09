#pragma once
#include <list>
#include <memory>
#include "LookingForTreasureState.h"

class Level;
class Hero;
class IGameObject;
class IState;

class AIStateMachine
{
private:
	std::shared_ptr<IState> LOOKING_FOR_TREASURE_STATE() {
		return std::make_shared<LookingForTreasureState>(LookingForTreasureState(*this, _level));
	}

	std::shared_ptr<IState> _state;
	Level& _level;
	Hero& _hero;
	std::list<std::shared_ptr<IGameObject>>& _gameObjects;
public:
	AIStateMachine(Level& level, Hero& hero, std::list<std::shared_ptr<IGameObject>>& gameObjects);
	void changeState(std::shared_ptr<IState> state);
	void update();
};

