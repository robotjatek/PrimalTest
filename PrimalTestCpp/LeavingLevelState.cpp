#include "LeavingLevelState.h"
#include "Hero.h"
#include "Exit.h"
#include "AIStateMachine.h"

LeavingLevelState::LeavingLevelState(AIStateMachine& context, Level& level) 
	: StateBase(level), _context(context) { }

void LeavingLevelState::update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects)
{
	auto distanceData = calculateDistanceData(hero.getPosition(), {});
	// only one exit is supported at the moment
	auto exits = findAllObjectsOfType<Exit>(gameObjects);
	auto& exit = exits.front();
	auto pathToExit = getPath(hero.getPosition(), exit->getPosition(), distanceData);
	auto& node = pathToExit.back();
	auto direction = node - hero.getPosition();
	if (direction.getLength() > 1) {
		_context.changeState(_context.UNWINNABLE_STATE());
		return;
	}

	hero.move(direction);
}
