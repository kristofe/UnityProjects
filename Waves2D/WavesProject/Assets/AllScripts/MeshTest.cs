using UnityEngine;
using System.Collections;

public class MeshTest : MonoBehaviour {

	public Vector3 size;
	public int gridDimension = 32;
	// Use this for initialization
	void Start () {
		GenerateGridMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void GenerateGridMesh ()
	{
		// Create the game object containing the renderer
		gameObject.AddComponent("MeshFilter");
		gameObject.AddComponent("MeshRenderer");
		gameObject.AddComponent("MeshCollider");
		//if (material)
		//	renderer.material = material;
		//else
			renderer.material.color = Color.white;
		
		//_wakeTexture = new ScrollingTexture(material,"_WakeTex",_gridDimension,_foamDecay,_foamSpreadRate);
	
	
		// Retrieve a mesh instance
		//var mesh : Mesh = GetComponent(MeshFilter).mesh;
		//var mf:MeshFilter = GetComponent(MeshFilter);
		//theMesh = mf.mesh;
		Mesh theMesh = (GetComponent("MeshFilter") as MeshFilter).mesh;
		
		
		int width = gridDimension;// Mathf.Min(heightMap.width, 255);
		int height = gridDimension;//Mathf.Min(heightMap.height, 255);
	
		
		int y = 0;
		int x = 0;
	
		// Build vertices and UVs
		Vector3[] vertices = new Vector3[height * width];
		Vector2[] uvs = new Vector2[height * width];
		Vector2[] uvs2 = new Vector2[height * width];
		Vector4[] tangents = new Vector4[height * width];
		Color[] colors = new Color[height*width];
	
		
		Vector2 uvScale = new Vector2 (1.0f / (width - 1), 1.0f / (height - 1));
		Vector3 sizeScale = new Vector3 (size.x / (width - 1), size.y, size.z / (height - 1));
		//Vector3 oneOverSizeScale = new Vector3(1.0f/sizeScale.x,1.0f/sizeScale.y,1.0f/sizeScale.z);
		
	
		//float cellXZDim = sizeScale.x;
		//print("cellXZDim " + cellXZDim);
		//print("sizeScale " + sizeScale);
		//float cellUVDim = uvScale.x;
			
		//var oneOverWidth:float = 1.0/width;
		//var oneOverHeight:float = 1.0/height;
		
		for (y=0;y<height;y++)
		{
			for (x=0;x<width;x++)
			{
				//var pixelHeight = heightMap.GetPixel(x, y).grayscale;
				//var vertex = Vector3 (x, pixelHeight, y);
				Vector3 vertex = new Vector3(x,0,y);
				vertices[y*width + x] = Vector3.Scale(sizeScale, vertex);
				uvs[y*width + x] = Vector2.Scale(new Vector2 (x, y), uvScale);
				uvs2[y*width + x] = Vector2.Scale(new Vector2 (x, y), uvScale);
				//var xparam:float = (x*oneOverWidth)*8.0;
				//var yparam:float = (y*oneOverHeight)*8.0;
				//var grayVal:float = Perlin.NoiseNormalized(xparam,yparam);
				//print(xparam + " " + yparam + " " + grayVal);
				//var c:Color = new Color(grayVal,grayVal,grayVal,1.0);
				colors[y*width + x] = new Color(1,1,1,1);//c;
	
				// Calculate tangent vector: a vector that goes from previous vertex
				// to next along X direction. We need tangents if we intend to
				// use bumpmap shaders on the mesh.
				Vector3 vertexL = new Vector3( x-1, 0/* heightMap.GetPixel(x-1, y).grayscale*/, y );
				Vector3 vertexR = new Vector3( x+1, 0 /*heightMap.GetPixel(x+1, y).grayscale*/, y );
				Vector3 tan = Vector3.Scale( sizeScale, vertexR - vertexL ).normalized;
				
				tangents[y*width + x] = new Vector4( tan.x, tan.y, tan.z, -1.0f );
			}
		}
		
		// Assign them to the mesh
		theMesh.vertices = vertices;
		theMesh.uv = uvs;
		theMesh.uv2 = uvs2;
		theMesh.colors = colors;
	
		// Build triangle indices: 3 indices into vertex array for each triangle
		int[] triangles = new int[(height - 1) * (width - 1) * 6];
		int index = 0;
		for (y=0;y<height-1;y++)
		{
			for (x=0;x<width-1;x++)
			{
				// For each grid cell output two triangles
				triangles[index++] = (y     * width) + x;
				triangles[index++] = ((y+1) * width) + x;
				triangles[index++] = (y     * width) + x + 1;
	
				triangles[index++] = ((y+1) * width) + x;
				triangles[index++] = ((y+1) * width) + x + 1;
				triangles[index++] = (y     * width) + x + 1;
			}
		}
		// And assign them to the mesh
		theMesh.triangles = triangles;
			
		// Auto-calculate vertex normals from the mesh
		theMesh.RecalculateNormals();
		
		// Assign tangents after recalculating normals
		theMesh.tangents = tangents;
		
		(GetComponent("MeshCollider") as MeshCollider).sharedMesh  = theMesh;
	
		//transform.position = new Vector3( -(0.5*size.x),-(0.5*size.y),0);
		
		//transform.Rotate(-90.0,0,0);
		
		//print("There are " + _gridDimension*_gridDimension + " Fluid Cells");
		
	}

}
