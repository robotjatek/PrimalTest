#include "Level.h"

#include "IDrawable.h"
#include "EntityRenderer.h"
#include "Trap.h"
#include "Hero.h"
#include "Monster.h"
#include "Sword.h"
#include "Potion.h"
#include "Treasure.h"
#include "Exit.h"
#include "IntVector2D.h"
#include "ConsoleTools.h"

Level::Level(const std::string& levelData) {
	std::vector<std::vector<char>> grid;
	std::stringstream ss(levelData);
	std::string line;

	while (std::getline(ss, line)) {
		std::vector<char> row(line.begin(), line.end());
		grid.push_back(row);
	}

	int rows = (int)grid.size();
	int columns = (int)grid[0].size();
	_y = rows;

	for (int row = 0; row < grid.size(); row++) {
		std::vector<bool> collisionRow;

		for (int col = 0; col < grid[row].size(); col++) {
			collisionRow.push_back(false);

			char cell = grid[row][col];
			switch (cell) {
			case 'x':
				collisionRow[col] = true;
				break;
			case 'c':
				_gameObjects.push_back(std::make_shared<Trap>(Trap(*this, col, row)));
				break;
			case 'i':
				_gameObjects.push_back(std::make_shared<Potion>(*this, col, row));
				break;
			case 'a':
				_gameObjects.push_back(std::make_shared<Sword>(*this, col, row));
				break;
			case 's':
				_gameObjects.push_back(std::make_shared<Monster>(*this, col, row));
				break;
			case 'h':
				_hero = std::make_shared<Hero>(Hero(*this, _gameObjects, col, row, *this));
				break;
			case 'k':
				_gameObjects.push_back(std::make_shared<Treasure>(*this, col, row));
				break;
			case 'j':
				_gameObjects.push_back(std::make_shared<Exit>(*this, col, row));
				break;
			}
		}

		_collisionData.push_back(collisionRow);
	}
}

void Level::draw() const {
	ConsoleTools::clear();
	for (int i = 0; i < _collisionData.size(); i++) {
		for (int j = 0; j < _collisionData[i].size(); j++) {
			_collisionData[i][j] ? std::cout << "x" : std::cout << " ";
		}
		std::cout << std::endl;
	}
	_hero->draw(_entityRenderer);
	for (auto& gameObject : _gameObjects) {
		gameObject->draw(_entityRenderer);
	}

	ConsoleTools::setCursorPosition((int)_collisionData[0].size() + 1, 0);
	std::cout << "Health: " << _hero->getHealth();
	ConsoleTools::setCursorPosition((int)_collisionData[0].size() + 1, 1);
	std::cout << "Sword acquired: " << (_hero->hasSword() ? "true" : "false");
	ConsoleTools::setCursorPosition((int)_collisionData[0].size() + 1, 2);
	std::cout << "Treasure acquired: " << (_hero->hasTreasure() ? "true" : "false");
}

void Level::setWall(int row, int column) {
	_collisionData.at(column).at(row) = true;
}

void Level::removeGameObject(IGameObject& gameObject)
{
	_gameObjects.remove_if([&gameObject](const std::shared_ptr<IGameObject>& ptr) {
		return ptr.get() == &gameObject;
		});
}

bool Level::isCollidingWith(const Hero& hero) const
{
	const IntVector2D& pos = hero.getPosition();
	int x = pos.getX();
	int y = pos.getY();

	if (y < 0 || y >= _collisionData.size() || x < 0 || x >= _collisionData[y].size()) {
		return true;
	}

	return _collisionData[y][x];
}

bool Level::isCollidingWith(const IntVector2D& point) const {
	int x = point.getX();
	int y = point.getY();
	if (y < 0 || y >= _collisionData.size() || x < 0 || x >= _collisionData[y].size()) {
		return true;
	}

	return _collisionData[y][x];
}

void Level::update() {
	_hero->update();

	if (_gameState == GameState::RUNNING) {
		int key = ConsoleTools::getKey();
		switch (key) {
		case 75: // Left arrow key
			_hero->move(IntVector2D::UNIT_X * -1);
			break;
		case 77: // Right arrow key
			_hero->move(IntVector2D::UNIT_X);
			break;
		case 72: // Up arrow key
			_hero->move(IntVector2D::UNIT_Y * -1);
			break;
		case 80: // Down arrow key
			_hero->move(IntVector2D::UNIT_Y);
			break;
		case 27: // Escape key
			_gameState = GameState::FORFEIT;
			break;
		case 13: // Enter key
			//	aiState = AIState::ACTIVE;
			break;
		default:
			break;
		}
	}
}

GameState Level::getGameState() const {
	return _gameState;
}

void Level::onDeath() {
	_gameState = GameState::DEAD;
}

void Level::onLeave(const Hero& hero) {
	if (hero.hasTreasure()) {
		_gameState = GameState::WIN;
	}
	else {
		_gameState = GameState::FORFEIT;
	}
}

int Level::getY() const {
	return _y;
}