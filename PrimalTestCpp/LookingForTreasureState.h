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
	void update(Hero& hero, std::list<std::shared_ptr<IGameObject>>& gameObjects) override;
};

