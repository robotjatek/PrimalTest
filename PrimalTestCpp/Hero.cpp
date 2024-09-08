#include "Hero.h"
#include "EntityRenderer.h"
#include "ICollider.h"
#include "IDeathHandler.h"
#include "IBackpackItem.h"
#include "Sword.h"
#include "Treasure.h"
#include <vector>
#include <algorithm>

Hero::Hero(ICollider& collider, std::list<std::shared_ptr<IGameObject>>& gameObjects, int x, int y, IDeathHandler& deathHandler): 
	_position(x, y), _lastPosition(x, y), _collider(collider), _gameObjects(gameObjects), _deathHandler(deathHandler) {
	_health = 2;
}

void Hero::draw(const EntityRenderer& renderer) const
{
	renderer.draw(*this);
}

char Hero::getSprite() const
{
	return 'h';
}

const IntVector2D& Hero::getPosition() const
{
	return _position;
}

void Hero::jumpOverTrap() {
	auto direction = _position - _lastPosition;
	auto nextPosition = _position + direction;

	if (_collider.isCollidingWith(nextPosition))
		kill();
	else
		move(direction);
}

void Hero::move(const IntVector2D& direction) {
	_lastPosition = IntVector2D(_position.getX(), _position.getY());
	_position += direction;
	
	if (_collider.isCollidingWith(*this))
		_position -= direction;
}

void Hero::update() {
	std::vector<std::shared_ptr<IGameObject>> copy(_gameObjects.begin(), _gameObjects.end());
	for (auto& gameObject : copy) {
		if (gameObject->getPosition().getX() == _position.getX() && gameObject->getPosition().getY() == _position.getY())
			gameObject->interact(*this);
	}
}

void Hero::kill() {
	setHealth(0);
}

int Hero::getHealth() const {
	return _health;
}

void Hero::setHealth(int health) {
	_health = std::clamp(health, 0, 2);
	if (_health == 0)
		_deathHandler.onDeath();
}

bool Hero::hasSword() const {
	return std::any_of(_backpackItems.begin(), _backpackItems.end(), [](const auto& item) {
		return std::dynamic_pointer_cast<Sword>(item) != nullptr;
	});
}

bool Hero::hasTreasure() const {
	return std::any_of(_backpackItems.begin(), _backpackItems.end(), [](const auto& item) {
		return std::dynamic_pointer_cast<Treasure>(item) != nullptr;
		});
}

void Hero::addBackpackItem(std::shared_ptr<IBackpackItem> item) {
	_backpackItems.push_back(item);
}
