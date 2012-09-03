
#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif

#include <math.h>
#include "WaveGrid.h"
// ------------------------------------------------------------------------
// Plugin itself


// Link following functions C-style (required for plugins)
extern "C"
{

	// The function we will call from Unity.
	//
	// We pass the Color array, texture size and current time to the plugin.
	// The C++ plugin then assigns noise-like colors to the color array.
	void EXPORT_API UpdateWaves(void* origUV1, void* uv1, void* uv2)
	{
		// safeguard - array must be not null
		//if( !colors )
		//	return;
		
		// Color structure in Unity is four RGBA floats
		Vector2* oUV1 = reinterpret_cast<Vector2*>(origUV1);
		Vector2* nUV1 = reinterpret_cast<Vector2*>(uv1);
		Vector2* nUV2 = reinterpret_cast<Vector2*>(uv2);
		
		WaveGrid* wg = WaveGrid::GetInstance();
		if(wg)
		{
			wg->UpdateSim(oUV1,nUV1,nUV2);
		}
		
	}

	void EXPORT_API InitWaves(int width, int height)
	{	
		WaveGrid::Construct(width, height);
		
	}
	
	void EXPORT_API DestroyWaves()
	{	
		WaveGrid::DestroyInstance();
		
	}
	
	void EXPORT_API WavesMouseDown(float u, float v, int button)
	{	
		WaveGrid* wg = WaveGrid::GetInstance();
		if(wg)
			wg->MouseDown(u, v, button);
		
	}	
	
	void EXPORT_API WavesMouseUp(float u, float v, int button)
	{	
		WaveGrid* wg = WaveGrid::GetInstance();
		if(wg)
			wg->MouseUp(u, v, button);
		
	}
	
	void EXPORT_API WavesMouseDragged(float u, float v, float lu, float lv,int button)
	{	
		WaveGrid* wg = WaveGrid::GetInstance();
		if(wg)
			wg->MouseDragged(u, v, lu, lv, button);
		
	}
	
} // end of export C block
