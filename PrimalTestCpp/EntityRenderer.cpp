#include "EntityRenderer.h"
#include "IDrawable.h"
#include "ConsoleTools.h"
#include <iostream>

void EntityRenderer::draw(const IDrawable& entity) const {
	ConsoleTools::setCursorPosition(entity.getPosition().getX(), entity.getPosition().getY());
	std::cout << entity.getSprite();
}