#pragma once

class Hero;
class IntVector2D;

class ICollider
{
public:
    virtual bool isCollidingWith(const Hero& hero) const = 0;
    virtual bool isCollidingWith(const IntVector2D& point) const = 0;
    virtual ~ICollider() = default;
};

