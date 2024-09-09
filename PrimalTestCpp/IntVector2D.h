#pragma once
#include <functional>

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
	bool operator==(const IntVector2D& other) const;
	bool operator!=(const IntVector2D& other) const;
	int getLength() const;
};

namespace std {
	template<>
	struct hash<IntVector2D> {
		std::size_t operator()(const IntVector2D& obj) const {
			std::size_t h1 = std::hash<int>()(obj.getX());
			std::size_t h2 = std::hash<int>()(obj.getY());

			return h1 ^ (h2 << 1);
		}
	};
}