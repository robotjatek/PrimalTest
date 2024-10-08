#include "IntVector2D.h"
#include <math.h>

const IntVector2D IntVector2D::UNIT_X(1, 0);
const IntVector2D IntVector2D::UNIT_Y(0, 1);

IntVector2D::IntVector2D(int x, int y) {
	_x = x;
	_y = y;
}

int IntVector2D::getX() const {
	return _x;
}

int IntVector2D::getY() const {
	return _y;
}

IntVector2D IntVector2D::operator+(const IntVector2D& other) const {
	IntVector2D a(_x + other.getX(), _y + other.getY());
	return a;
}

IntVector2D& IntVector2D::operator+=(const IntVector2D& other) {
	_x += other._x;
	_y += other._y;

	return *this;
}

IntVector2D& IntVector2D::operator=(const IntVector2D& other) {
	_x = other._x;
	_y = other._y;

	return *this;
}

IntVector2D IntVector2D::operator-(const IntVector2D& other) const {
	return IntVector2D(_x - other._x, _y - other._y);
}

IntVector2D& IntVector2D::operator-=(const IntVector2D& other) {
	_x -= other._x;
	_y -= other._y;

	return *this;
}

IntVector2D IntVector2D::operator*(int other) const {
	return IntVector2D(_x * other, _y * other);
}

bool IntVector2D::operator==(const IntVector2D& other) const
{
	return _x == other._x && _y == other._y;
}

bool IntVector2D::operator!=(const IntVector2D& other) const
{
	return !(*this == other);
}

unsigned int IntVector2D::getLength() const
{
	return (unsigned int)sqrt(_x * _x + _y * _y);
}
