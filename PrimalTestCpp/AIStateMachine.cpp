#include "AIStateMachine.h"
#include "IState.h"

AIStateMachine::AIStateMachine(Level& level, Hero& hero, std::list<std::shared_ptr<IGameObject>>& gameObjects)
	: _level(level), _hero(hero), _gameObjects(gameObjects)
{
	_state = LOOKING_FOR_TREASURE_STATE();
}

void AIStateMachine::changeState(std::shared_ptr<IState> state) {
	_state = std::move(state);
}

void AIStateMachine::update() {
	_state->update(_hero, _gameObjects);
}
