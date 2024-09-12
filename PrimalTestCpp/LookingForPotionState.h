#pragma once
#include "StateBase.h"
#include "IState.h"

class LookingForPotionState : public StateBase, public IState
{
private:
	AIStateMachine& _context;
public:
	LookingForPotionState(AIStateMachine& context, Level& level);
	LookingForPotionState(const LookingForPotionState&) = delete;
	LookingForPotionState operator=(const LookingForPotionState&) = delete;
	void update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects) override;
};

