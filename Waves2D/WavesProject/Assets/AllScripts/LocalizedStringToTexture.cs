using UnityEngine;
using System.Collections;

public class LocalizedStringToTexture : MonoBehaviour {
	public RenderTexture renderTexture;
	public GUIStyle guiStyle;
	public string[] languages;
	public string[] ids;
	public Rect rect;
	
	private int currentLanguageIdx;
	private int currentIdIdx;
	private string id;
	private string language;
	
	// Use this for initialization
	void Start () {
		language = languages[0];
		id = ids[0];
		
		StringTable.loadFile(language);
		
		currentLanguageIdx = 0;
		currentIdIdx = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUI.Label(rect,StringTable.getString(id), guiStyle);
		
		if(GUI.Button(new Rect(0,Screen.height - 64,64,64),"Create Texture"))
		{
			createTexture();
			moveToNextID();
		}
	}
	
	private bool moveToNextID()
	{
		if(++currentIdIdx >= ids.Length - 1)
		{
			currentIdIdx = 0;
			currentLanguageIdx++;
			
			if(currentLanguageIdx < languages.Length)
			{
				StringTable.loadFile(languages[currentLanguageIdx]);
			}
			else{
				currentLanguageIdx = 0;
				return false;
			}
		}
		id = ids[currentIdIdx];
		language = languages[currentLanguageIdx];
		return true;
	}
	
	private void createTexture()
	{
		Texture2D tex = new Texture2D((int)rect.width,(int)rect.height,TextureFormat.ARGB32,false);
		Rect flippedRect = new Rect(rect.x,Screen.height - rect.height, rect.width,rect.height);
    	tex.ReadPixels(flippedRect,0,0,false);

    	//yield return 0;

    	//tex.SetPixel (0, 0, Color.white);
    	tex.Apply();	
		
		Color32[] colors = tex.GetPixels32();
		Color32[] newColors = new Color32[colors.Length];
		for(int i = 0; i < colors.Length; i++)
		{
			Color32 color = colors[i];
			color.a = color.r;
			newColors[i] = color;
		}
		
		tex.SetPixels32(newColors);
		
		byte[] bytes = tex.EncodeToPNG();
		
		Debug.Log("Creating image_label_" + id + "_" + language + ".png");
		
		System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/image_label_" + id + "_" + language + ".png", bytes);

	}
}
