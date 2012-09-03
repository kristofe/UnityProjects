#ifndef _WAVE_GRID_H
#define _WAVE_GRID_H

//These are overly paranoid.  You can delete a null pointer.
#define SAFE_DELETE(p)       { if(p) { delete (p);     (p)=NULL; } }
#define SAFE_DELETE_ARRAY(p) { if(p) { delete[] (p);   (p)=NULL; } }

#define CLAMP(v,min,max) ((v) < (min) ? (min) : ((v) > (max) ? (max) : (v)))
#define LERP(t,a,b) ((a)+((t)*((b)-(a))))

#include <stdlib.h>
#include <stdio.h>

struct Vector2
{
	float x;
	float y;
};

class WaveGrid
{
public:
	WaveGrid(int nx, int ny);
	~WaveGrid();

	void UpdateSim(Vector2* origUV1, Vector2* uv1, Vector2* uv2);

	void MouseDown(float u, float v, int button);
	void MouseDragged(float u, float v,float lu, float lv, int button);
	void MouseUp(float u, float v, int button);
	
	static WaveGrid* GetInstance();//int width = 10, int height = 10);
	static WaveGrid* Construct(int width = 10, int height = 10);
	static void DestroyInstance();
	
private:
	//void animateWaves();
	//void initWaveData();
	
private:
	static WaveGrid* mInstance;
	int _NX;
	int _NY;
	int _gridCellCount;
	float* _waveZ;
	float* _waveZ1;
	/*int		_NX;
	int		_NY;
	int		_gridCellsCount;
	
	//Wave Sim Data

	float	_waveC;
	float	_waveH;
	float	_waveL;
	float	_waveD;
	float	_waveA;
	float	_waveB;
	float	_waveDT;
	float   _waveDiagStrength;
	float   _waveHorMultiplier;
	float   _waveDiagMultiplier;
	float   _waveVelSmoothing;
	float   _waveVelDamping;
	float	_waveHeightDamping;

	//Arrays
	float*	_waveZ;
	float*	_waveZ1;
	float*  _waveV;
	float*  _waveV1;
	//float*	_waveDamping;
	 */



	//INLINED FUNCTIONS
	inline int XY(float x, float y)
	{		
		int rows = (int)(y * _NY);
		int columns = (int)(x * _NX);
		return  IX(columns, rows);
	}
	
	inline int IX(int x, int y)
	{
		//x = CLAMP(x,0,_NX);
		//y = CLAMP(y,0,_NY);
		return y*_NX + x;
	}
	
	 void waveSwap()
	 {
		 //float* tempArray = _waveZ1;
		 //_waveZ1 = _waveZ;
		 //_waveZ = tempArray;
		 
		 float* temp = _waveZ;
		 _waveZ = _waveZ1;
		 _waveZ1 = temp;
	 }
	 
};

#endif