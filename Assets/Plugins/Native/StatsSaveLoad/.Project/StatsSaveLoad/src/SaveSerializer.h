#pragma once

#ifdef __cplusplus
extern "C" {
#endif
	extern
	void
	__declspec(dllexport) __stdcall
	SaveStatsToFile(const wchar_t* a_path, float a_time, int a_accuracy);

	extern
	void
	__declspec(dllexport) __stdcall
	LoadStatsFromFile(const wchar_t* a_path, float* a_pTime, int* a_pAccuracy);

#ifdef __cplusplus
}
#endif
