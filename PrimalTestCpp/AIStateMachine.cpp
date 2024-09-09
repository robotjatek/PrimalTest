#include "AIStateMachine.h"
#include "IState.h"
#include <vector>

AIStateMachine::AIStateMachine(Level& level, Hero& hero, std::list<std::shared_ptr<IGameObject>>& gameObjects)
	: _level(level), _hero(hero), _gameObjects(gameObjects)
{
	_state = LOOKING_FOR_TREASURE_STATE();
}

void AIStateMachine::changeState(std::shared_ptr<IState> state) {
	_state = std::move(state);
}

void AIStateMachine::update() {
	std::vector<std::shared_ptr<IGameObject>> a(_gameObjects.begin(), _gameObjects.end());
	_state->update(_hero, a);
}
