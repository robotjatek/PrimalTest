#pragma once

class IDeathHandler {
public:
	virtual void onDeath() = 0;
	virtual ~IDeathHandler() = default;
};