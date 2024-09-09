#pragma once
#include <vector>
#include <unordered_map>
#include <list>
#include <memory>
#include "DistanceData.h"
#include <algorithm>
#include <iterator>
#include "IDrawable.h"
#include "IntVector2D.h"
#include "Level.h"
#include "Treasure.h"

class StateBase
{
public:
	StateBase(Level& level);
protected:
	Level& _level;
	std::vector<IntVector2D> getPath(const IntVector2D& from, const IntVector2D& to,
		const std::unordered_map<IntVector2D, DistanceData>& distanceData);
	std::unordered_map<IntVector2D, DistanceData> calculateDistanceData(const IntVector2D& start,
		const std::vector<std::shared_ptr<IGameObject>>& additionalWalls);

	template <typename T>
	static std::vector<std::shared_ptr<IGameObject>> findAllObjectsOfType(const std::vector<std::shared_ptr<IGameObject>>& gameObjects) {
		std::vector<std::shared_ptr<IGameObject>> result;

		/*std::copy_if(gameObjects.begin(), gameObjects.end(),
			std::back_inserter(result),
			[](const std::shared_ptr<IGameObject>& obj) {
				return std::dynamic_pointer_cast<T>(obj) != nullptr;
			});*/

		for (const auto& obj : gameObjects) {
			if (auto casted = std::dynamic_pointer_cast<T>(obj)) {
				result.push_back(casted);
			}
		}

		return result;
	}

	template <typename T>
	std::vector<std::shared_ptr<IGameObject>> filterObjectTypesOnPath(
		const std::vector<std::shared_ptr<IGameObject>>& objects,
		const std::vector<IntVector2D>& path) {
		std::vector<std::shared_ptr<IGameObject>> objectsOnPath;

		for (const auto& obj : objects) {
			if (auto casted = std::dynamic_pointer_cast<T>(obj)) {
				if (std::find(path.begin(), path.end(), obj->getPosition()) != path.end())
					objectsOnPath.push_back(casted);
			}
		}

		return objectsOnPath;
	}
private:
	static std::vector<IntVector2D> getNeighbors(const IntVector2D& node, std::vector<IntVector2D> unvisited);
};

