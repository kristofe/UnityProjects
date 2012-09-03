using UnityEngine;
using UnityEditor;
using System.Collections;

class WizardTextureSplitter : ScriptableWizard
{
    public Texture2D textureToSplit;
    public GameObject prefab;
    public int tileSize = 128;

    [MenuItem("CustomTools/Create Tiled BG")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Create Tiled BG", typeof(WizardTextureSplitter));

        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard("Create Light", typeof(WizardCreateLight));
    }

    void OnWizardCreate()
    {
        new TextureSplitter(textureToSplit, prefab, tileSize);
    }

    void OnWizardUpdate()
    {
        helpString = "Please set the parameters";
    }

}

public class TextureSplitter
{
    public Texture2D textureToSplit;
    public GameObject prefab;
    public int tileSize = 128;
	private int lastTileSizeCreated = 0;
	private GameObject parent;
	
    public TextureSplitter(Texture2D TextureToSplit, GameObject Prefab, int TileSize){
        textureToSplit = TextureToSplit;
        prefab = Prefab;
        tileSize = TileSize;
        parent = new GameObject();
        parent.name = "BackgroundTiles";
        TileTextures();
    }

    private void TileTextures()
	{
		
        lastTileSizeCreated = tileSize;
        int widthInTiles = (int)System.Math.Round((textureToSplit.width / ((float)tileSize)) + 0.5f);
        int heightInTiles = (int)System.Math.Round((textureToSplit.height / ((float)tileSize)) + 0.5f);

        for (int j = 0; j < heightInTiles; ++j)
        {
            int y = j * tileSize;
            for (int i = 0; i < widthInTiles; ++i)
            {
                int x = i * tileSize;
                Vector3 pos = new Vector3(x,y,0);
                Vector3 scale = new Vector3(tileSize, 1, tileSize);
                Texture2D t = WriteTexture(textureToSplit, new Vector2(x,y),tileSize, "splitBG_" + i.ToString("00") + "_" + j.ToString("00"));
                InstantiateTile(pos, scale, t);


            }
        }
	}

    private Texture2D WriteTexture(Texture2D source, Vector2 offset, int squareSize, string name)
    {
        Texture2D t = new Texture2D(squareSize, squareSize, TextureFormat.ARGB32, false);
        Color[] c = source.GetPixels((int)offset.x, (int)offset.y, squareSize, squareSize, 0);
        t.SetPixels(c);
        t.Apply();
        t.filterMode = FilterMode.Point;
        t.name = offset.x/squareSize + "_" + offset.y/squareSize;
        //byte[] b = t.EncodeToPNG();
        //string fileName = Application.dataPath + "/Resources/" + name + ".png";
        //System.IO.File.WriteAllBytes(fileName, b);
        //Texture2D ret = (Texture2D)Resources.Load(name);
        //textures.Add(t);
        return t;
    }

    private void InstantiateTile(Vector3 pos, Vector3 scale, Texture2D tex)
    {
        GameObject clone = (GameObject)EditorUtility.InstantiatePrefab(prefab);
        //Material mat = new Material(;
        //mat.CopyPropertiesFromMaterial(clone.renderer.material);
        //mat.mainTexture = tex;
        //clone.renderer.material = mat;
       
            clone.renderer.material.SetTexture("_MainTex", tex);
            clone.transform.parent = parent.transform;
            clone.transform.localScale = scale;
            clone.transform.localPosition = pos;
            clone.name = tex.name;
        
        
        
    }



}