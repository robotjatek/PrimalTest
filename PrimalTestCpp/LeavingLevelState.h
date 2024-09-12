#pragma once
#include "StateBase.h"
#include "IState.h"

class LeavingLevelState : public StateBase, public IState
{
private:
	AIStateMachine& _context;
public:
	LeavingLevelState(AIStateMachine& context, Level& level);
	LeavingLevelState(const LeavingLevelState&) = delete;
	LeavingLevelState operator=(const LeavingLevelState) = delete;
	void update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects) override;
};

