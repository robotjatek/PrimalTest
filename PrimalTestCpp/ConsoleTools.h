#pragma once
#ifdef _WIN32
#include "Windows.h"
#include <conio.h>
#endif

class ConsoleTools {
public:
	static void setCursorPosition(int x, int y) {
#ifdef _WIN32
		COORD p = { (short) x, (short) y };
		SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), p);
#endif
	}

	static void clear() {
#ifdef _WIN32
		system("cls");
#endif // _WIN32
	}

	static int getKey() {
		return _getch();
	}
};