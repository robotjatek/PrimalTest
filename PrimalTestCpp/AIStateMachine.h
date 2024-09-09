#pragma once
#include <list>
#include <memory>
#include "LookingForTreasureState.h"
#include "LeavingLevelState.h"
#include "UnwinnableState.h"
#include "LookingForSwordState.h"
#include "LookingForPotionState.h"

class Level;
class Hero;
class IGameObject;
class IState;

class AIStateMachine
{
private:
	std::shared_ptr<IState> _state;
	Level& _level;
	Hero& _hero;
	std::list<std::shared_ptr<IGameObject>>& _gameObjects;
public:
	std::shared_ptr<IState> LOOKING_FOR_TREASURE_STATE() {
		return std::make_shared<LookingForTreasureState>(*this, _level);
	}

	std::shared_ptr<IState> LEAVING_LEVEL_STATE() {
		return std::make_shared<LeavingLevelState>(*this, _level);
	}

	std::shared_ptr<IState> UNWINNABLE_STATE() {
		return std::make_shared<UnwinnableState>(*this, _level);
	}

	std::shared_ptr<IState> LOOKING_FOR_SWORD_STATE() {
		return std::make_shared<LookingForSwordState>(*this, _level);
	}

	std::shared_ptr<IState> LOOKING_FOR_POTION_STATE() {
		return std::make_shared<LookingForPotionState>(*this, _level);
	}

	AIStateMachine(Level& level, Hero& hero, std::list<std::shared_ptr<IGameObject>>& gameObjects);
	void changeState(std::shared_ptr<IState> state);
	void update();
};

