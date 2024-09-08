#include "Exit.h"
#include "EntityRenderer.h"

#include "ILeaveHandler.h"

Exit::Exit(ILeaveHandler& container, int x, int y) : _container(container), _position(x, y) {}

char Exit::getSprite() const
{
    return 'j';
}

const IntVector2D& Exit::getPosition() const
{
    return _position;
}

void Exit::draw(const EntityRenderer& entityRenderer) const
{
    entityRenderer.draw(*this);
}

void Exit::interact(Hero& hero)
{
    _container.onLeave(hero);
}
