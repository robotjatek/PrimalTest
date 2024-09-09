#pragma once
#include <list>
#include <memory>

class IGameObject;
class Hero;

class IState {
public:
	virtual void update(Hero& hero, std::list<std::shared_ptr<IGameObject>>& gameObjects) = 0;
};