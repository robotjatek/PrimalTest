#pragma once

class Hero;

class ILeaveHandler {
public:
	virtual void onLeave(const Hero& hero) = 0;
	virtual ~ILeaveHandler() = default;
};