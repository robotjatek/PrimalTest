#pragma once
#include "StateBase.h"
#include "IState.h"

class LookingForSwordState : public StateBase, public IState
{
private:
	AIStateMachine& _context;
public:
	LookingForSwordState(AIStateMachine& context, Level& level);
	LookingForSwordState(const LookingForSwordState&) = delete;
	LookingForSwordState& operator=(const LookingForSwordState&) = delete;
	void update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects) override;
};

