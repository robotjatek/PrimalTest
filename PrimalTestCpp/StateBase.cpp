#include "StateBase.h"
#include "Level.h"
#include "IDrawable.h"
#include <limits>
#include "IntVector2D.h"
#include "Hero.h"
#include <optional>

StateBase::StateBase(Level& level) : _level(level)
{
}

std::unordered_map<IntVector2D, DistanceData> StateBase::calculateDistanceData(const IntVector2D& start, const std::vector<std::shared_ptr<IGameObject>>& additionalWalls)
{
	std::unordered_map<IntVector2D, DistanceData> distanceData;

	std::vector<IntVector2D> visited;
	std::vector<IntVector2D> unvisited;
	for (unsigned int x = 0; x < _level.getX(); x++) {
		for (unsigned int y = 0; y < _level.getY(); y++) {
			if (_level.getCollisionData()[y][x] == false) {
				IntVector2D pos(x, y);
				bool isWall = std::any_of(additionalWalls.begin(), additionalWalls.end(),
					[&pos](const std::shared_ptr<IGameObject>& obj) {
						return obj->getPosition() == pos;
					});

				if (!isWall) {
					unvisited.push_back(pos);
				}
			}
		}
	}

	for (const auto& node : unvisited) {
		DistanceData d;
		d.previousVertex = std::nullopt;
		d.shortestDistance = (node == start) ? 0 : INT_MAX;
		distanceData[node] = d;
	}

	while (!unvisited.empty()
		&& std::any_of(unvisited.begin(), unvisited.end(),
			[&distanceData](const IntVector2D& vec) {
				return distanceData[vec].shortestDistance < INT_MAX;
			})) {

		auto nodeIt = std::min_element(unvisited.begin(), unvisited.end(),
			[&distanceData](const IntVector2D& a, const IntVector2D& b) {
				return distanceData[a].shortestDistance < distanceData[b].shortestDistance;
			});
		IntVector2D node = *nodeIt;
		unvisited.erase(nodeIt);

		auto neighbors = getNeighbors(node, unvisited);
		for (const auto& neighbor : neighbors) {
			int distance = distanceData[node].shortestDistance + 1; // Every neighbor is 1 away
			if (distance < distanceData[neighbor].shortestDistance) {
				// if the calculated distance is shorter than a previously calculated, update it
				distanceData[neighbor].shortestDistance = distance;
				distanceData[neighbor].previousVertex = node;
			}
		}

		visited.push_back(node);
	}

	return distanceData;
}

std::vector<IntVector2D> StateBase::getNeighbors(const IntVector2D& node, std::vector<IntVector2D> unvisited)
{
	std::vector<IntVector2D> possibleMoves = {
		{0, 1},  // Up
		{0, -1}, // Down
		{1, 0},  // Right
		{-1, 0}  // Left
	};

	std::vector<IntVector2D> neighbors;
	for (auto& move : possibleMoves) {
		IntVector2D neighbour(node.getX() + move.getX(), node.getY() + move.getY());
		if (std::find(unvisited.begin(), unvisited.end(), neighbour) != unvisited.end()) {
			neighbors.push_back(neighbour);
		}
	}

	return neighbors;
}

std::vector<IntVector2D> StateBase::getPath(const IntVector2D& from, const IntVector2D& to,
	const std::unordered_map<IntVector2D, DistanceData>& distanceData) {
	std::vector<IntVector2D> path = { to };

	std::optional<IntVector2D> node = to;

	while (node.has_value()) {
		auto it = distanceData.find(node.value());
		if (it != distanceData.end()) {
			auto prev = it->second.previousVertex;
			if (prev.has_value() && prev.value() != from) {
				path.push_back(prev.value());
			}
			node = prev;
		}
		else {
			node.reset();
		}
	}

	return path;
}
