#pragma once
#include <vector>
#include <memory>

class IGameObject;
class Hero;

class IState {
public:
	virtual void update(Hero& hero, std::vector<std::shared_ptr<IGameObject>>& gameObjects) = 0;
	virtual ~IState() = default;
};