using UnityEngine;
using System.Collections;

[System.Serializable]
public class StringInfo
{
	public string id;
	public TextAnchor alignment = TextAnchor.UpperLeft;
}

public class LocalizedStringToTexture : MonoBehaviour {
	public RenderTexture renderTexture;
	public GUIStyle guiStyle;
	public string[] languages;
	public StringInfo[] ids;
	public Rect rect;
	
	private int currentLanguageIdx;
	private int currentIdIdx;
	private StringInfo info;
	private string language;
	
	private bool done;
	
	// Use this for initialization
	void Start () {
		language = languages[0];
		info = ids[0];
		
		StringTable.loadFile(language);
		
		currentLanguageIdx = 0;
		currentIdIdx = 0;
		done = false;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		guiStyle.alignment = info.alignment;
		GUI.Label(rect,StringTable.getString(info.id), guiStyle);
		
		//GUI.DrawTexture(new Rect(rect.x, rect.y + 128,rect.width,rect.height),LocalizedCachedStringAsTexture.getInstance().getCachedString("wave_options"));
		
		if(!done && GUI.Button(new Rect(0,Screen.height - 64,64,64),"Create Texture"))
		{
			//StartCoroutine("createTextures");
			createTexture();
			done = !moveToNextID();
		}
		
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Debug.Log ("LocalizedStringToTexture::OnRenderImage!");	
		
	}
	
	void OnPostRender()
	{
		
	}
	
	private IEnumerator createTextures()
	{
		do{
			yield return new WaitForSeconds(0.1f);
			createTexture();
			yield return 0;
			
		}
		while(moveToNextID());
	}
	
	private bool moveToNextID()
	{
		if(++currentIdIdx > ids.Length - 1)
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
		info = ids[currentIdIdx];
		language = languages[currentLanguageIdx];
		return true;
	}
	
	private void createTexture()
	{
		Texture2D tex = new Texture2D((int)rect.width,(int)rect.height,TextureFormat.ARGB32,false);
		Rect flippedRect = new Rect(rect.x,Screen.height - rect.height, rect.width,rect.height);
    	tex.ReadPixels(flippedRect,0,0,false);

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
		Debug.Log("Creating image_label_" + info.id + "_" + language + ".png");
		
		System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/image_label_" + info.id + "_" + language + ".png", bytes);
	}
}
