#include "WaveGrid.h"

WaveGrid* WaveGrid::mInstance = NULL;

WaveGrid* WaveGrid::GetInstance()//int width, int height)
{
	if(mInstance == NULL)
	{
		printf("ERROR: Tried to get a NULL instance!!!!\n");
	}
	
	return WaveGrid::mInstance;
	
}

WaveGrid* WaveGrid::Construct(int width, int height)
{
	if(mInstance == NULL)
	{
		//printf("Constructing instance of WaveGrid(%d,%d)\n", width, height);
		WaveGrid::mInstance = new WaveGrid(width, height);
	}
		
	return WaveGrid::mInstance;
	
}

void WaveGrid::DestroyInstance()
{
	if(mInstance != NULL)
	{
		delete WaveGrid::mInstance;
		WaveGrid::mInstance = NULL;
	}

	
}
/*
WaveGrid::WaveGrid(int nx, int ny){
	//_waveZ = new Array(_gridCellsCount);
	//_waveZ1 = new Array(_gridCellsCount);
	//_waveDamping = new Array(_gridCellsCount);
	//_gridCells = (float*)malloc(sizeof(float)*_gridCellsCount);
	_NX = nx;
	_NY = ny;
	_gridCellsCount = _NX*_NY;

	_waveZ = (float*)calloc(_gridCellsCount,sizeof(float));
	_waveZ1 = (float*)calloc(_gridCellsCount,sizeof(float));
	_waveV = (float*)calloc((_gridCellsCount+2*_NX),sizeof(float));
	//_waveV1 = (float*)malloc(sizeof(float)*_gridCellsCount);
	_waveV1 = _waveV + 2*_NX; // WaveV1 and WaveV share the same memory space (except WaveV1 is shifted back by two rows)
	
	//_waveDamping = (float*)malloc(sizeof(float)*_gridCellsCount);
	
	_waveDT = 0.5f;
	_waveC = 1.0f; // wave speed
	_waveH = 1.0f; // grid cell width
	//_waveL = _waveH*(_gridDimension+2.0f); //width of entire grid
	_waveA = (_waveC*_waveC*_waveDT)/(_waveH*_waveH);
	_waveDiagStrength = 0.25f;
	_waveHorMultiplier = (1.0f - _waveDiagStrength) / 4.0f;
	_waveDiagMultiplier = (_waveDiagStrength) / 4.0f;
	_waveVelSmoothing = 0.1f;
	_waveVelDamping = 0.9999f;
	_waveHeightDamping = 0.9999f;
	//_accelerometerValues[0] = 0;
	//_accelerometerValues[1] = 0;
	//_accelerometerValues[2] = 0;
	
	initWaveData();
}

WaveGrid::~WaveGrid()
{
	if(_waveZ)
		free(_waveZ);
	if(_waveZ1)
		free(_waveZ1);
	if(_waveV)
		free(_waveV);
	//if(_waveV1)
	//	free(_waveV1);
	//if(_waveDamping)
	//	free(_waveDamping);
	//if(_gridCells)
//	free(_gridCells);
}

void WaveGrid::Update(float* colors)
{
	//animateWaves();
	
	for (int y=1;y<_NY-1;++y)
	{
		for (int x=1;x<_NX-1;++x)
		{
			int idx = IX(x,y);
			_waveZ[idx] = 
			((_waveZ1[IX(x-1,y)]+
			  _waveZ1[IX(x+1,y)]+
			  _waveZ1[IX(x,y-1)]+
			  _waveZ1[IX(x,y+1)]) / 2.0f) - _waveZ[IX(x,y)];
			
			//Drag (otherwise water never stop moving)
			_waveZ[IX(x,y)] -= _waveZ[IX(x,y)] * 0.05f;//DRAG		
		}
	}
	
	
	
	for (int y=0;y<_NY;++y)
	{
		for (int x=0;x<_NX;++x)
		{
			int i = (y*_NY) + x;
			float* pixel = colors + (i*4);
			float c = (_waveZ[i]*0.5f) + 0.5f;//Assumes that color is between -1 and 1;
			pixel[0] = c;
			pixel[1] = c;
			pixel[2] = c;
			pixel[3] = 1.0f;
		}
		
	}
	waveSwap();
}

void WaveGrid::animateWaves()
{	
	// Enforce boundary conditions
    for( int i=0; i < _NX; i++)
	{
		_waveZ[IX(i,0)] = _waveZ[IX(i,1)];
		_waveZ[IX(i,_NY-1)] = _waveZ[IX(i,_NY-2)];
	}
	
    for( int j=0; j < _NY; j++)
	{
		_waveZ[IX(0,j)] = _waveZ[IX(1,j)];
		_waveZ[IX(_NX-1,j)] = _waveZ[IX(_NX-2,j)];
	}
	
	//int JMP = _NX*2;
	
	//edges are unchanged
	for( int j=_NY-2 ; j>=1; j-- )
	{
		for( int i=1 ; i<_NX-1; i++ )
		{
			int id = IX(i,j);
			
			//					-1,0		+1,0		0,-1		0,+1
			//					-1,-1		-1,+1		+1,-1		+1,+1
			_waveV1[id] = _waveV[id] + _waveA*(
											   _waveHorMultiplier*( _waveZ[id-1] + _waveZ[id+1] + _waveZ[id-_NX] + _waveZ[id+_NX] )
											   + _waveDiagMultiplier*( _waveZ[id-_NX-1] + _waveZ[id-_NX+1] + _waveZ[id+_NX-1] + _waveZ[id+_NX+1] )
											   - _waveZ[id]
											   );
		}
	}
	
	
	for( int j = 1; j < _NY-1; j++ )
	{
		for( int i = 1; i < _NX-1; i++ )
		{
			int id = IX(i,j);
			
			//							-1,0			+1,0			0,-1				0,+1
			float neighborAvg =  (_waveV1[id-1] + _waveV1[id+1] + _waveV1[id-_NX] + _waveV1[id+_NX])*0.25f;
			_waveV[id] = LERP(_waveV1[id],neighborAvg, _waveVelSmoothing) * _waveVelDamping;
			
			_waveZ[id] += _waveDT * _waveV[id];
			_waveZ[id] *= _waveHeightDamping; //_waveDamping[targetIdx];
			
		}
	}
}

void WaveGrid::initWaveData()
{
	//for(int i=0; i<_gridCellsCount ; i++ )
	//{	
	//	_waveZ[i] = 0.0f;
	//	//_waveZ1[i] = 0.0f;
	//	_waveV[i]  = 0.0f;
	//	//_waveDamping[i] = _waveHeightDamping;	
	//}
	
	//Put a wave impulse in the middle			
	int midX = (int)(_NX*0.5f);
	int midY = (int)(_NY*0.5f);
	
	_waveZ[IX(midX,midY)] = 1.0f;
	//_waveZ[IX(midX,midY)] = _waveZ1[IX(midX,midY)] = 1.0f; 
}

void WaveGrid::MouseDown(float u, float v, int button)
{
	int idx = XY(u,v);
	_waveZ[idx] = 1.0f;
	//_waveZ[idx] = _waveZ1[idx] = 4.0f;
	//_waveV[idx] += 5.0f;
}

// TODO: Add height along entire path.
void WaveGrid::MouseDragged(float u, float v,float lu, float lv, int button)
{
	int idx = XY(u,v);
	_waveZ[idx] = 1.0f;
	//_waveZ[idx] = _waveZ1[idx] = 4.0f;
	//_waveV[idx] += 5.0f;
}

void WaveGrid::MouseUp(float u, float v, int button)
{
	int idx = XY(u,v);
	_waveZ[idx] = -1.0f;
	//_waveZ[idx] = _waveZ1[idx] = -4.0f;
	//_waveV[idx] += -5.0f;
}*/






WaveGrid::WaveGrid (int nx, int ny) 
{
	_NX = nx;
	_NY = ny;
	_gridCellCount = _NX * _NY;
	_waveZ = new float[_gridCellCount];
	_waveZ1 = new float[_gridCellCount];
	
	for(int i = 0; i < _gridCellCount; ++i)
	{
		_waveZ[i] = _waveZ1[i] = 0.0f;
	}
	
	int midX = (int)(_NX*0.5f);
	int midY = (int)(_NY*0.5f);
	
	_waveZ[IX(midX,midY)] = 1.0f;
}

WaveGrid::~WaveGrid()
{
	SAFE_DELETE_ARRAY(_waveZ);
	SAFE_DELETE_ARRAY(_waveZ1);
}


void WaveGrid::UpdateSim(Vector2* origUV1, Vector2* uv1, Vector2* uv2){
	for (int y=1;y<_NY-1;++y)
	{
		for (int x=1;x<_NX-1;++x)
		{
			int idx = IX(x,y);
			_waveZ[idx] = 
			((_waveZ1[IX(x-1,y)]+
			  _waveZ1[IX(x+1,y)]+
			  _waveZ1[IX(x,y-1)]+
			  _waveZ1[IX(x,y+1)]) / 2.0f) - _waveZ[idx];
			
			//Drag (otherwise water never stop moving)
			_waveZ[idx] -= _waveZ[IX(x,y)] * 0.05f;//DRAG
			
			
			
			
		}
	}
	
	for (int y=1;y<_NY-1;++y)
	{
		for (int x=1;x<_NX-1;++x)
		{
			int idx = IX(x,y);
			_waveZ[idx] = (0.8f*_waveZ[idx])+(0.2f*((_waveZ[IX(x-1,y)]+_waveZ[IX(x+1,y)]+_waveZ[IX(x,y-1)]+_waveZ[IX(x,y+1)])/4.0f));
			
			float v = _waveZ[idx];
			uv2[idx].x = 0.5f + (v - _waveZ[IX(x-1,y  )])*-10.0f;
			uv2[idx].y = 0.5f + (v - _waveZ[IX(x  ,y-1)])*-10.0f;
		}
	}
	for (int i=0;i<_gridCellCount;++i)
	{
		//float v = (_waveZ[i]*0.5f) + 0.5f;//Assumes that color is between -1 and 1;
		float perturbance = (_waveZ[i]*0.7f);
		uv1[i].x = origUV1[i].x + perturbance;
		uv1[i].y = origUV1[i].y + perturbance;
		
	}
	waveSwap();	
	
}
/*
int WaveGrid::IX(int i, int j)
{
	return (j*_NX) + i;	
}

int WaveGrid::XY(float x, float y)
{		
	int rows = (int)(y * _NY);
	int columns = (int)(x * _NX);
	return  IX(columns, rows);
}

void WaveGrid::waveSwap()
{
	float[] temp = _waveZ;
	_waveZ = _waveZ1;
	_waveZ1 = temp;
}*/

void WaveGrid::MouseDown(float u, float v, int button)
{
	//printf("MouseDown(%3.2f,%3.2f,%d)\n",u,v,button);
	float fracU = 2.0f/_NX;///((float)_gridCellCount);
	float fracV = 2.0f/_NY;///((float)_gridCellCount);
	u = CLAMP(u,fracU,1.0f-fracU);
	v = CLAMP(v,fracV,1.0f-fracV);
	int idx = XY(u,v);
	_waveZ[idx] = -1.0f;
	
}

// TODO: Add height along entire path.
void WaveGrid::MouseDragged(float u, float v,float lu, float lv, int button)
{
	float fracU = 2.0f/_NX;///((float)_gridCellCount);
	float fracV = 2.0f/_NY;///((float)_gridCellCount);
	u = CLAMP(u,fracU,1.0f-fracU);
	v = CLAMP(v,fracV,1.0f-fracV);
	int idx = XY(u,v);
	_waveZ[idx] = -1.0f;
	
}

void WaveGrid::MouseUp(float u, float v, int button)
{
	//int idx = XY(u,v);
	//_waveZ[idx] = 1.0f;
	
}

