#include "LookingForPotionState.h"
#include "Hero.h"
#include "AIStateMachine.h"
#include "Monster.h"
#include "Potion.h"

LookingForPotionState::LookingForPotionState(AIStateMachine& context, Level& level)
	: StateBase(level), _context(context) { }

void LookingForPotionState::update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects){
	if (hero.getHealth() > 1) {
		_context.changeState(_context.LOOKING_FOR_TREASURE_STATE());
		return;
	}

	auto potions = findAllObjectsOfType<Potion>(gameObjects);
	if (potions.size() == 0) {
		_context.changeState(_context.UNWINNABLE_STATE());
		return;
	}

	auto allMonsters = findAllObjectsOfType<Monster>(gameObjects);
	// monsters are walls...
	auto recalculatedDistanceData = calculateDistanceData(hero.getPosition(), allMonsters);
	auto& potion = potions.front();
	auto pathToPotion = getPath(hero.getPosition(), potion->getPosition(), recalculatedDistanceData);

	auto& node = pathToPotion.back();
	auto direction = node - hero.getPosition();

	// No clear path to the potion
	if (direction.getLength() > 1) {
		_context.changeState(_context.UNWINNABLE_STATE());
		return;
	}

	hero.move(direction);
}
