#include "LookingForSwordState.h"
#include "Hero.h"
#include "AIStateMachine.h"
#include "Monster.h"
#include "Sword.h"

LookingForSwordState::LookingForSwordState(AIStateMachine& context, Level& level)
	: StateBase(level), _context(context)
{
}

void LookingForSwordState::update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects)
{
	if (hero.hasSword()) {
		_context.changeState(_context.LOOKING_FOR_TREASURE_STATE());
		return;
	}

	// Look for sword without monsters on the path
	auto allMonsters = findAllObjectsOfType<Monster>(gameObjects);
	auto swords = findAllObjectsOfType<Sword>(gameObjects);
	// No sword found, but there are monsters in the way => forfeit
	if (swords.size() == 0) {
		_context.changeState(_context.UNWINNABLE_STATE());
		return;
	}

	// monsters are considered as walls when the hero does not have a sword
	auto& sword = swords.front();
	auto recalculatedDistanceData = calculateDistanceData(hero.getPosition(), allMonsters);
	auto pathToSword = getPath(hero.getPosition(), sword->getPosition(), recalculatedDistanceData);

	// move to sword
	auto& node = pathToSword.back();
	auto direction = node - hero.getPosition();
	hero.move(direction);
}
