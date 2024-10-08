#define _CRTDBG_MAP_ALLOC
#include <iostream>
#include <fstream>
#include <sstream>
#include "Level.h"
#include "ConsoleTools.h"
#include <stdlib.h>
#include <crtdbg.h>

int main()
{
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);

	std::ifstream file("lvl.txt");
	if (!file.is_open())
		return 1;

	std::string levelData;
	std::ostringstream ss;
	ss << file.rdbuf();
	levelData = ss.str();

	auto level = std::make_shared<Level>(levelData);
	while (level->getGameState() == GameState::RUNNING) {
		level->draw();
		level->update();
	}
	level->draw();

	ConsoleTools::setCursorPosition(0, (int)level->getY());
	switch (level->getGameState()) {
	case GameState::DEAD:
		std::cout << "Game Over" << std::endl;
		break;
	case GameState::FORFEIT:
		std::cout << "Forfeit" << std::endl;
		break;
	case GameState::WIN:
		std::cout << "You win" << std::endl;
		break;
	default:
		throw 0;
	}
	file.close();
}