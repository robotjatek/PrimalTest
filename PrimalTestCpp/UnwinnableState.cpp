#include "UnwinnableState.h"
#include "Hero.h"
#include "Exit.h"

UnwinnableState::UnwinnableState(AIStateMachine& context, Level& level) : StateBase(level), _context(context)
{
}

void UnwinnableState::update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects)
{
	auto distanceData = calculateDistanceData(hero.getPosition(), {});
	// forfeit when unwinnable
	// try to exit
	auto exits = findAllObjectsOfType<Exit>(gameObjects);
	auto& exit = exits.front();
	auto pathToExit = getPath(hero.getPosition(), exit->getPosition(), distanceData);
	auto& node = pathToExit.back();
	auto direction = node - hero.getPosition();

	if (direction.getLength() > 1) {
		// cant leave for some reason, exit is closed
		_level.onForfeit();
		return;
	}

	// move to exit
	hero.move(direction);
}
