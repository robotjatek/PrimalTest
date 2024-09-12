#pragma once
#include "StateBase.h"
#include "IState.h"

class AIStateMachine;

class LookingForTreasureState : public StateBase, public IState
{
private:
	AIStateMachine& _context;
public:
	LookingForTreasureState(AIStateMachine& context, Level& level);
	LookingForTreasureState(const LookingForTreasureState&) = delete;
	LookingForTreasureState& operator=(const LookingForTreasureState&) = delete;
	void update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects) override;
};

