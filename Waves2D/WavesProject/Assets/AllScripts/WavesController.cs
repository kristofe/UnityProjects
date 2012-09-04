#if UNITY_EDITOR
#define PC
#else
#define IPHONE
#endif


using UnityEngine;
using System;
using System.Runtime.InteropServices;




public class WavesController : MonoBehaviour {
// Link to the Waves plugin and call the UpdateTexture function there.
#if PC
	[DllImport ("UnityWaves")]
    private static extern void UpdateWaves (IntPtr origUV1,IntPtr uv1,IntPtr uv2);
    [DllImport ("UnityWaves")]
    private static extern void InitWaves (int width, int height);
    [DllImport ("UnityWaves")]
    private static extern void DestroyWaves();
    [DllImport ("UnityWaves")]
    private static extern void WavesMouseDown (float u, float v, int buttonIdx);
    [DllImport ("UnityWaves")]
    private static extern void WavesMouseDragged (float u, float v,float lu, float lv, int buttonIdx);
#else
	[DllImport ("__Internal")]
    private static extern void UpdateWaves (IntPtr origUV1,IntPtr uv1,IntPtr uv2);
    [DllImport ("__Internal")]
    private static extern void InitWaves (int width, int height);
    [DllImport ("__Internal")]
    private static extern void DestroyWaves();
    [DllImport ("__Internal")]
    private static extern void WavesMouseDown (float u, float v, int buttonIdx);
	[DllImport ("__Internal")]
    private static extern void WavesMouseDragged (float u, float v,float lu, float lv, int buttonIdx);
#endif    
    public bool useNative = true;
    //public int width = 256;
    //public int height = 256;
    //private Texture2D m_Texture;

	private WaveGrid waveGrid;
	//public Vector3 size;
	public int gridDimensionX= 48;
	public int gridDimensionY= 72;
	//public Material material;
    //private Color[] m_Pixels;
    //private GCHandle m_PixelsHandle;
    //private Vector3[] vertices;
    
    [System.NonSerialized]
    private Vector2[] origUV1s;
    
	[System.NonSerialized]
	private Vector2[] uvs;
	
	[System.NonSerialized]
	private Vector2[] origUV2s;
	
	[System.NonSerialized]
	private Vector2[] uv2s;
	//private Vector4[] tangents;
    //private Color[] colors;
    
    //private Color[] colorPointer;
    //private GCHandle gchColorsPointer;
    
    private GCHandle gchOrigUV1sPointer;
    private GCHandle gchUV1sPointer;
    private GCHandle gchUV2sPointer;
    
    public MeshFilter meshFilter;
    private Mesh theMesh;
    
    private float lastUpdateTime;
    private Vector2 lastMousePosUV;
    private bool mouseDown;
   
    void Start () {
    	#if UNITY_EDITOR
    		print("UNITY_EDITOR");
   		#endif
    	#if UNITY_IPHONE
    		print("UNITY_IPHONE !!!");
   		#endif
    		
    	
    	mouseDown = false;
    	lastUpdateTime = 0;
    	theMesh = meshFilter.mesh;
    	int arrSize = gridDimensionX*gridDimensionY;
    	origUV1s = new Vector2[arrSize];
		//origUV2s = new Vector2[arrSize];
		uvs = new Vector2[arrSize];
		uv2s = new Vector2[arrSize];
		Vector2 o = new Vector2(0.5f,0.5f);
		Vector2[] uv = theMesh.uv;
		Array.Copy(uv,uvs,arrSize);
		Array.Copy(uv,origUV1s,arrSize);
		//Debug.Log("Expected uv count " + arrSize + " real count " + uv.Length);
		for(int i = 0; i < arrSize; ++i)
		{
			//origUV1s[i] = uvs[i] = uv[i];
			uv2s[i] = o;
		}
    	
		if(useNative)
		{
			InitWaves(gridDimensionX,gridDimensionY);
	    	// "pin" the array in memory, so we can pass direct pointer to it's data to the plugin,
	        // without costly marshaling of array of structures.
			//gchColorsPointer = GCHandle.Alloc(colors, GCHandleType.Pinned);
			
			gchOrigUV1sPointer = GCHandle.Alloc(origUV1s, GCHandleType.Pinned);
			gchUV1sPointer = GCHandle.Alloc(uvs, GCHandleType.Pinned);
			gchUV2sPointer = GCHandle.Alloc(uv2s, GCHandleType.Pinned);
    		
		}
		else
		{		
	    	waveGrid = new WaveGrid(gridDimensionX, gridDimensionY);
		}

		print("Creating ad banner on bottom");
		AdBinding.createAdBanner( true );
    }
    
    void OnDisable() {

    	
    	if(useNative)
    	{
	        // Free the pinned array handle.
	        DestroyWaves();
	        gchOrigUV1sPointer.Free();
			gchUV1sPointer.Free();
			gchUV2sPointer.Free();
    	}
		
		print("Destroying ad banner");
		AdBinding.destroyAdBanner();
    }

    
    
    void LateUpdate () {
		if(Time.time - lastUpdateTime > 0.03333f)
		{

			if(useNative)
			{
				UpdateWaves(gchOrigUV1sPointer.AddrOfPinnedObject(),gchUV1sPointer.AddrOfPinnedObject(), gchUV2sPointer.AddrOfPinnedObject());    
	        	theMesh.uv = uvs;
				theMesh.uv2 = uv2s;
			}
			else
			{
				waveGrid.UpdateSim(origUV1s, uvs,uv2s);
				theMesh.uv = uvs;
				theMesh.uv2 = uv2s;
			}
#if PC        	
        	if(Input.GetMouseButton(0))
        	{
        		MouseDragged();
        		
        	}else if(Input.GetMouseButtonDown(0))
        	{
        		MouseDown();
        	}
#else
			if(iPhoneInput.touchCount > 0)
				MouseDown();

#endif
        	lastUpdateTime = Time.time;
		}
    }
    
    
	


	
	public void MouseDown()
	{
		//Debug.Log("In OnMouseDown");
		
		
		
#if PC
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    	RaycastHit hit = new RaycastHit();
    	if(collider.Raycast(ray,out hit,100))
    	{
    		lastMousePosUV = Input.mousePosition;
			Vector2 uv = new Vector2(Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height);
			if(useNative)
				WavesMouseDown(uv.x,uv.y,0);
			else
				waveGrid.MouseDown(uv.x,uv.y,0);;
	    }
#else
		if(Application.platform == RuntimePlatform.OSXEditor)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    	RaycastHit hit = new RaycastHit();
	    	if(collider.Raycast(ray,out hit,100))
	    	{
				Vector2 uv = new Vector2(Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height);
				if(useNative)
					WavesMouseDown(uv.x,uv.y,0);
				else
					waveGrid.MouseDown(uv.x,uv.y,0);
				lastMousePosUV = uv;
		    }
		}
		else
		{
			if(iPhoneInput.touchCount > 0){
				for(int i = 0; i < iPhoneInput.touchCount; ++i)
				{
					iPhoneTouch touch = iPhoneInput.GetTouch(i);
					Vector2 uv = new Vector2(touch.position.x/Screen.width,touch.position.y/Screen.height);
					if(useNative)
					{
						WavesMouseDown(uv.x,uv.y,0);
					}
					else
					{
						waveGrid.MouseDown(uv.x,uv.y,0);
					}
					lastMousePosUV = uv;
					/*Ray ray = Camera.main.ScreenPointToRay(touch.position);
			    	RaycastHit hit = new RaycastHit();
			    	if(collider.Raycast(ray,out hit,100))
			    	{
			    		MouseDown(hit.textureCoord2.x,hit.textureCoord2.y,0);
			    		lastMousePosUV = touch.position;
			    	}
			    	*/
				}
			}
		}
#endif
	}
	
	public void MouseDragged()
	{
		
#if PC
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    	RaycastHit hit = new RaycastHit();
    	if(collider.Raycast(ray,out hit,100))
    	{
    		
			Vector2 uv = new Vector2(Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height);
			if(useNative)
				WavesMouseDown(uv.x,uv.y,0);
			else
				waveGrid.MouseDown(uv.x,uv.y,0);
			lastMousePosUV = Input.mousePosition;
	    }
#else
		if(Application.platform == RuntimePlatform.OSXEditor)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    	RaycastHit hit = new RaycastHit();
	    	if(collider.Raycast(ray,out hit,100))
	    	{
	    		Vector2 uv = new Vector2(Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height);
				if(useNative)
					WavesMouseDown(uv.x,uv.y,0);
				else
					waveGrid.MouseDown(uv.x,uv.y,0);
				lastMousePosUV = uv;
		    }
		}
		else
		{
			if(iPhoneInput.touchCount > 0){
				for(int i = 0; i < iPhoneInput.touchCount; ++i)
				{
					iPhoneTouch touch = iPhoneInput.GetTouch(i);
					Vector2 uv = new Vector2(touch.position.x/Screen.width,touch.position.y/Screen.height);
					if(useNative)
						WavesMouseDown(uv.x,uv.y,0);
					else
						waveGrid.MouseDown(uv.x,uv.y,0);
					lastMousePosUV = uv;
					/*Ray ray = Camera.main.ScreenPointToRay(touch.position);
			    	RaycastHit hit = new RaycastHit();
			    	if(collider.Raycast(ray,out hit,100))
			    	{
			    		waveGrid.MouseDrag(hit.textureCoord2.x,hit.textureCoord2.y,lastMousePosUV.x,lastMousePosUV.y,0);
			    		//MouseDrag(hit.textureCoord2.x,hit.textureCoord2.y,lastMousePosUV.x,lastMousePosUV.y,0);
			    		lastMousePosUV = touch.position;
			    	}*/
				}
			}
		}
#endif
	}
	 
}
