#include "LookingForTreasureState.h"
#include "Hero.h"
#include "Exit.h"
#include <algorithm>
#include "DistanceData.h"
#include "AIStateMachine.h"
#include "Treasure.h"
#include "Monster.h"
#include "StateBase.h"

LookingForTreasureState::LookingForTreasureState(AIStateMachine& context, Level& level)
	: StateBase(level), _context(context) { }

void LookingForTreasureState::update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects) {
	auto distanceData = calculateDistanceData(hero.getPosition(), {});
	if (hero.hasTreasure()) {
		_context.changeState(_context.LEAVING_LEVEL_STATE());
		return;
	}

	auto treasures = StateBase::findAllObjectsOfType<Treasure>(gameObjects);
	if (treasures.size() == 0) {
		_context.changeState(_context.UNWINNABLE_STATE());
		return;
	}

	// Only one treasure is supported at the moment
	auto pathToTreasure = getPath(hero.getPosition(), treasures[0]->getPosition(), distanceData);
	auto monsters = findAllObjectsOfType<Monster>(gameObjects);
	auto monstersOnPath = filterObjectTypesOnPath<Monster>(monsters, pathToTreasure);

	// Get a potion if the health is less than the maximum and there are monsters in the path
	// Keeping the hero alive is the highest priority
	if (monstersOnPath.size() > 0 && hero.getHealth() < 2) {
		_context.changeState(_context.LOOKING_FOR_POTION_STATE());
		return;
	}
	//get a sword if the path is not clear
	else if (monstersOnPath.size() > 0 && !hero.hasSword()) {
		_context.changeState(_context.LOOKING_FOR_SWORD_STATE());
		return;
	}
	// if no state change necessary move towards to treasure on the calculated path
	else {
		auto& node = pathToTreasure.back();
		auto direction = node - hero.getPosition();
		// hero can only move 1 cell at a time. If the length is different from one this is not a valid path
		if (direction.getLength() > 1) {
			// No path to treasure exists
			_context.changeState(_context.UNWINNABLE_STATE());
			return;
		}
		else {
			hero.move(direction);
			return;
		}
	}
}
