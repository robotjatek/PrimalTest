#pragma once

class IGameObject;

class IGameObjectContainer {
public:
	virtual void removeGameObject(IGameObject& gameObject) = 0;
	virtual ~IGameObjectContainer() = default;
};