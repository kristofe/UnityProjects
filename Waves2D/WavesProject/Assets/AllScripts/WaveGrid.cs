using UnityEngine;
using System.Collections;

public class WaveGrid {
	public int _NX;
	public int _NY;
	private int _gridCellCount;
	private float[] _waveZ;
	private float[] _waveZ1;
	
	
	public WaveGrid (int nx, int ny) 
	{
		_NX = nx;
		_NY = ny;
		_gridCellCount = _NX * _NY;
		_waveZ = new float[_gridCellCount];
		_waveZ1 = new float[_gridCellCount];
		
		
		int midX = (int)(_NX*0.5f);
		int midY = (int)(_NY*0.5f);
	
		_waveZ[IX(midX,midY)] = 1.0f;
	}
	

	
	public void UpdateSim(Vector2[] origUV1, Vector2[] uv1, Vector2[] uv2){
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
				_waveZ[idx] = (0.6f*_waveZ[idx])+(0.4f*((_waveZ[IX(x-1,y)]+_waveZ[IX(x+1,y)]+_waveZ[IX(x,y-1)]+_waveZ[IX(x,y+1)])/4.0f));
				  
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
	
	private int IX(int i, int j)
	{
		return (j*_NX) + i;	
	}
	
	private int XY(float x, float y)
	{		
		int rows = (int)(y * _NY);
		int columns = (int)(x * _NX);
		return  IX(columns, rows);
	}
	
	private void waveSwap()
	{
		float[] temp = _waveZ;
		_waveZ = _waveZ1;
		_waveZ1 = temp;
	}
	
	public void MouseDown(float u, float v, int button)
	{
		float fracU = 2.0f/_NX;///((float)_gridCellCount);
		float fracV = 2.0f/_NY;///((float)_gridCellCount);
		u = Mathf.Clamp(u,fracU,1.0f-fracU);
		v = Mathf.Clamp(v,fracV,1.0f-fracV);
		int idx = XY(u,v);
		_waveZ[idx] = -1.0f;

	}
	
	// TODO: Add height along entire path.
	public void MouseDrag(float u, float v,float lu, float lv, int button)
	{
		float fracU = 2.0f/_NX;///((float)_gridCellCount);
		float fracV = 2.0f/_NY;///((float)_gridCellCount);
		u = Mathf.Clamp(u,fracU,1.0f-fracU);
		v = Mathf.Clamp(v,fracV,1.0f-fracV);
		int idx = XY(u,v);
		_waveZ[idx] = -1.0f;

	}
	
	public void MouseUp(float u, float v, int button)
	{
		//int idx = XY(u,v);
		//_waveZ[idx] = 1.0f;

	}
	
}
