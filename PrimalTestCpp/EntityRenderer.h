#pragma once

class IDrawable;

class EntityRenderer {
public:
	void draw(const IDrawable& entity) const;
};