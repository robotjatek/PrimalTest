#pragma once
#include <limits>
#include "IntVector2D.h"
#include <optional>

struct DistanceData
{
	std::optional<IntVector2D> previousVertex;
	int shortestDistance = INT_MAX;
};

