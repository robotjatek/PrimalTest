#pragma once
class IntVector2D {
public:
	static const IntVector2D UNIT_X;
	static const IntVector2D UNIT_Y;
private:
	int _x;
	int _y;
public:
	IntVector2D(int x, int y);
	int getX() const;
	int getY() const;
	IntVector2D operator+(const IntVector2D& other) const;
	IntVector2D& operator+=(const IntVector2D& other);
	IntVector2D& operator=(const IntVector2D& other);
	IntVector2D operator-(const IntVector2D& other) const;
	IntVector2D& operator-=(const IntVector2D& other);
	IntVector2D operator*(int i) const;
};