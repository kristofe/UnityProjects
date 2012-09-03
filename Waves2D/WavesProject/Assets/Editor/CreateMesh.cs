using UnityEngine;
using UnityEditor;
using System.Collections;

class CreateMesh : ScriptableWizard
{
   	public Vector3 size = new Vector3(9.6f,10.0f,6.4f);
	public int gridDimensionX= 72;
	public int gridDimensionY= 48;
	public Material material;
    //private Color[] m_Pixels;
    //private GCHandle m_PixelsHandle;
    private Vector3[] vertices;
    private Vector2[] origUV1s;
	private Vector2[] uvs;
	private Vector2[] origUV2s;
	private Vector2[] uv2s;
	private Vector4[] tangents;
    private Color[] colors;
    private Mesh theMesh;

    [MenuItem("CustomTools/Create Mesh")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Create A Custom Mesh", typeof(CreateMesh));

        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard("Create Light", typeof(WizardCreateLight));
    }

    void OnWizardCreate()
    {
    	GenerateGridMesh();
        /*Transform[] objects;
        
        if(!topLevelOnly)
        {
        	if(includePrefabs)
        	{
        		objects = Selection.GetTransforms(SelectionMode.Deep);
        	}
        	else
        	{
        		objects = Selection.GetTransforms(SelectionMode.Deep | SelectionMode.ExcludePrefab);
        	}
        }
        else
        {
            if(includePrefabs)
        	{
        		objects = Selection.GetTransforms(SelectionMode.TopLevel);
        	}
        	else
        	{
        		objects = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        	}   	
        }
        
        for(int i = 0; i < objects.Length; i++)
        {
        	Transform t = objects[i];
        	if(scalePosition)
        	{
	        	Vector3 p = t.localPosition;
	        	//if(scalePositionZ)
	        	//{
	        	//	p = new Vector3(p.x*scale.y,p.y*scale.y,p.z*scale.z);
	        	//}
	        	//else
	        	//{
	        	//	p = new Vector3(p.x*scale,p.y*scale,p.z);	
	        	//}
	        	t.localPosition = new Vector3(p.x*scale.y,p.y*scale.y,p.z*scale.z);;
        	}
        	if(scaleSize)
        	{
        		Vector3 s = t.localScale;
        		//if(scaleScaleZ)
        		//{
        		//	s = s*scale;
        		//}
        		//else
        		//{
        		//	s = new Vector3(s.x*scale,s.y*scale,s.z);
        		//}
        		t.localScale = new Vector3(s.x*scale.y,s.y*scale.y,s.z*scale.z);;
        	}
        	
        }
        
        */
    }

    void OnWizardUpdate()
    {
        helpString = "Generate a Grid Mesh";
    }
    
     public void GenerateGridMesh ()
	{
		GameObject go = new GameObject();
		// Create the game object containing the renderer
		go.AddComponent("MeshFilter");
		go.AddComponent("MeshRenderer");
		go.AddComponent("MeshCollider");
		if (material)
			go.renderer.material = material;
		//else
		//	renderer.material.color = Color.black;
		
		//_wakeTexture = new ScrollingTexture(material,"_WakeTex",_gridDimension,_foamDecay,_foamSpreadRate);
	
	
		// Retrieve a mesh instance
		//var mesh : Mesh = GetComponent(MeshFilter).mesh;
		//var mf:MeshFilter = GetComponent(MeshFilter);
		//theMesh = mf.mesh;
		theMesh = (go.GetComponent("MeshFilter") as MeshFilter).mesh;
		
		
		int gwidth = gridDimensionX;// Mathf.Min(heightMap.width, 255);
		int gheight = gridDimensionY;//Mathf.Min(heightMap.height, 255);
	
		
		int y = 0;
		int x = 0;
	
		// Build vertices and UVs
		int arrSize = gheight*gwidth;
		vertices = new Vector3[arrSize];
		origUV1s = new Vector2[arrSize];
		origUV2s = new Vector2[arrSize];
		uvs = new Vector2[arrSize];
		uv2s = new Vector2[arrSize];
		tangents = new Vector4[arrSize];
		colors = new Color[arrSize];
	
		
		Vector2 uvScale = new Vector2 (1.0f / (gwidth - 1), 1.0f / (gheight - 1));
		Vector3 sizeScale = new Vector3 (size.x / (gwidth - 1), size.y, size.z / (gheight - 1));
		//Vector3 oneOverSizeScale = new Vector3(1.0f/sizeScale.x,1.0f/sizeScale.y,1.0f/sizeScale.z);
		
	
		//float cellXZDim = sizeScale.x;
		//print("cellXZDim " + cellXZDim);
		//print("sizeScale " + sizeScale);
		//float cellUVDim = uvScale.x;
			
		//var oneOverWidth:float = 1.0/width;
		//var oneOverHeight:float = 1.0/height;
		
		for (y=0;y<gheight;y++)
		{
			for (x=0;x<gwidth;x++)
			{
				//var pixelHeight = heightMap.GetPixel(x, y).grayscale;
				//var vertex = Vector3 (x, pixelHeight, y);
				Vector3 vertex = new Vector3(x,0,y);
				int idx = y*gwidth + x;
				vertices[idx] = Vector3.Scale(sizeScale, vertex);
				uvs[idx] = Vector2.Scale(new Vector2 (x, y), uvScale);
				uv2s[idx] = new Vector2(0.5f,0.5f);
				origUV1s[idx] = uvs[idx];
				origUV2s[idx] = uv2s[idx];
				//var xparam:float = (x*oneOverWidth)*8.0;
				//var yparam:float = (y*oneOverHeight)*8.0;
				//var grayVal:float = Perlin.NoiseNormalized(xparam,yparam);
				//print(xparam + " " + yparam + " " + grayVal);
				//var c:Color = new Color(grayVal,grayVal,grayVal,1.0);
				colors[idx] = new Color(1,1,1,1);//c;
	
				// Calculate tangent vector: a vector that goes from previous vertex
				// to next along X direction. We need tangents if we intend to
				// use bumpmap shaders on the mesh.
				Vector3 vertexL = new Vector3( x-1, 0/* heightMap.GetPixel(x-1, y).grayscale*/, y );
				Vector3 vertexR = new Vector3( x+1, 0 /*heightMap.GetPixel(x+1, y).grayscale*/, y );
				Vector3 tan = Vector3.Scale( sizeScale, vertexR - vertexL ).normalized;
				
				tangents[idx] = new Vector4( tan.x, tan.y, tan.z, -1.0f );
			}
		}
		
		// Assign them to the mesh
		theMesh.vertices = vertices;
		theMesh.uv = uvs;
		theMesh.uv2 = uv2s;
		theMesh.colors = colors;
	
		// Build triangle indices: 3 indices into vertex array for each triangle
		int[] triangles = new int[(gheight - 1) * (gwidth - 1) * 6];
		int index = 0;
		for (y=0;y<gheight-1;y++)
		{
			for (x=0;x<gwidth-1;x++)
			{
				// For each grid cell output two triangles
				triangles[index++] = (y     * gwidth) + x;
				triangles[index++] = ((y+1) * gwidth) + x;
				triangles[index++] = (y     * gwidth) + x + 1;
	
				triangles[index++] = ((y+1) * gwidth) + x;
				triangles[index++] = ((y+1) * gwidth) + x + 1;
				triangles[index++] = (y     * gwidth) + x + 1;
			}
		}
		// And assign them to the mesh
		theMesh.triangles = triangles;
			
		// Auto-calculate vertex normals from the mesh
		theMesh.RecalculateNormals();
		
		// Assign tangents after recalculating normals
		theMesh.tangents = tangents;
		
		(go.GetComponent("MeshCollider") as MeshCollider).sharedMesh  = theMesh;
	
		//transform.position = new Vector3( -(0.5*size.x),-(0.5*size.y),0);
		
		//transform.Rotate(-90.0,0,0);
		
		//print("There are " + _gridDimension*_gridDimension + " Fluid Cells");
		
	}

}
