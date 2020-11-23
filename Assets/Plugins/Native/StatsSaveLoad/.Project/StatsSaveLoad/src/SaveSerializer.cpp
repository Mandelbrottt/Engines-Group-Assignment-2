#include "SaveSerializer.h"

#include <string>

#include <fstream>

void
__stdcall
SaveStatsToFile(const wchar_t* a_path, float a_time, int a_accuracy) {

	std::wstring dir = a_path;
	dir              = dir.substr(0, dir.find_last_of('/'));

	_wmkdir(dir.c_str());

	std::ofstream file(a_path, std::ios::binary);

	if (!file) {
		return;
	}

	file << a_time << ' ' << a_accuracy;
}

void
__stdcall
LoadStatsFromFile(const wchar_t* a_path, float* a_pTime, int* a_pAccuracy) {
	if (!(a_pTime || a_pAccuracy)) {
		return;
	}

	std::ifstream file(a_path, std::ios::binary);

	if (!file) {
		*a_pTime     = -1;
		*a_pAccuracy = -1;
		return;
	}

	file >> *a_pTime >> *a_pAccuracy;
}
