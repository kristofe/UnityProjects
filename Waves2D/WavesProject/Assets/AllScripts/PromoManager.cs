using UnityEngine;
using System.Collections;
using System;


public class PromoData
{
	public string appName;
	public string iconURL;
	public string linkURL;
	public Texture2D downloadedIcon;
	
	public PromoData(string[] fields)
	{
		appName = fields[0];
		iconURL = fields[1];
		linkURL = fields[2];
	}
	
	public void debugPrint()
	{
		Debug.Log ("PromoData: appName: " + appName + " iconURL: " + iconURL + " linkURL: " + linkURL +  " textureDim: "
			+ downloadedIcon.width + ", " + downloadedIcon.height);
	}
};

public class PromoManager : MonoBehaviour {
	public string promoFileBaseURL;
	public string appID;
	public string lastVersionDisplayed;
	public ArrayList promoData;
	public int showEveryCount;
	public GUISkin guiSkin;
	
	private int promoVersion;
	private int lastPromoVersion;
	private int runCounter;
	
	private Vector2 scrollPosition;
	
	private const string LAST_PROMO_SHOWN = "promo_shown";
	private const string RUN_COUNTER = "run_counter";
	
	private GUIStyle largeFont;
	private GUIStyle smallFont;
	
	
	// Use this for initialization
	void Start () {
		lastPromoVersion = -1;
		runCounter = 0;
#if UNITY_IPHONE
		lastPromoVersion = PlayerPrefs.GetInt(LAST_PROMO_SHOWN);
		runCounter = PlayerPrefs.GetInt(RUN_COUNTER);
		runCounter++;
		
#endif
		largeFont = new GUIStyle();
		largeFont.fontSize = 18;
		largeFont.fontStyle = FontStyle.Bold;
		largeFont.normal.textColor = Color.white;
		largeFont.alignment = TextAnchor.UpperCenter;
		largeFont.wordWrap = true;
		
		smallFont = new GUIStyle();
		smallFont.fontSize = 12;
		smallFont.fontStyle = FontStyle.Bold;
		smallFont.normal.textColor = Color.white;
		smallFont.alignment = TextAnchor.MiddleLeft;
		smallFont.wordWrap = true;
		
		promoData = new ArrayList();
		StartCoroutine("processPromo");
	}
	
	private void storePromoVersionState()
	{
#if UNITY_IPHONE
		PlayerPrefs.SetInt(LAST_PROMO_SHOWN,promoVersion);
#endif
	}
	
	private void storeRunCounterState()
	{
#if UNITY_IPHONE
		Debug.Log ("Storing Run Counter State: " + runCounter);
		PlayerPrefs.SetInt(RUN_COUNTER,runCounter);
#endif
	}
	
	public bool hasData()
	{
		return promoData.Count > 0;
	}
	
	public string generateURL()
	{
		string fileName = promoFileBaseURL;
		switch(Application.systemLanguage)
		{
		case SystemLanguage.Chinese:
			fileName += "_CHINESE.txt";
			break;
			
		case SystemLanguage.Korean:
			fileName += "_KOREAN.txt";
			break;
			
		case SystemLanguage.Japanese:
			fileName += "_JAPANESE.txt";
			break;
			
		case SystemLanguage.German:
			fileName += "_GERMAN.txt";
			break;
			
		case SystemLanguage.French:
			fileName += "_FRENCH.txt";
			break;
		
		case SystemLanguage.Spanish:
			fileName += "_SPANISH.txt";
			break;
			
		case SystemLanguage.Portuguese:
			fileName += "_PORTUGUESE.txt";
			break;
			
		case SystemLanguage.English:	
			fileName += "_ENGLISH.txt";
			break;
		default:
			fileName += "_ENGLISH.txt";
			break;
		}
		
		//return promoFileBaseURL + "_GERMAN.txt";
		
		return fileName;	
	}
	
	public IEnumerator processPromo()
	{
		string url = generateURL();
		WWW promoFile = new WWW(url);
		
		while(promoFile.isDone == false)
			yield return 0;
		
		if(promoFile.error != null)
		{
			Debug.LogError("Couldn't load promotion at url: " + url + " " + promoFile.error);
			return false;
		}
		
		//Got file.  Now parse it
		string txt = promoFile.text;
		Debug.Log (txt);
		
		string[] lines = txt.Split('\n');
		//Check version
		string firstLine = lines[0].Trim();
		//promoVersion = Int32.Parse(firstLine);
		Debug.Log("Promo Version: " + promoVersion + " lastPromoVersion: " + lastPromoVersion);
		
		
		foreach(string line in lines)
		{
			string l = line.Trim();
			
			//Remove comments
			if(l != null && l.Length > 0 && l[0] == '#')
				continue;
			int hashIndex = l.IndexOf('#');
			if(hashIndex > 0)
			{
				l = l.Substring(0,hashIndex - 1);
			}
			
			string[] fields = l.Split('|');
			
			if(fields.Length < 3)
				continue;
			
			PromoData pd = new PromoData(fields);
			
			//Don't show promo for our own app
			if(pd.appName == appID)
				continue;
			
			WWW icon = new WWW(pd.iconURL);
		
			while(icon.isDone == false)
				yield return 0;
			
			if(icon.error != null)
			{
				Debug.LogError("Couldn't load icon at url: " + pd.iconURL + " " + icon.error);
				return false;
			}
			
			pd.downloadedIcon = icon.texture;
			pd.debugPrint();
			promoData.Add(pd);
			
		}
		
		
		//Should we show the promo?
		//Show if there is a new version or we have run the app a certain multiple of time
		Debug.Log ("runCounter: " + runCounter + " showEveryCount: " + showEveryCount + " runCounter % showEveryCount: " + runCounter % showEveryCount);
		if(runCounter > 2 && (promoVersion > lastPromoVersion || runCounter % showEveryCount == 0))
			SendMessage("setGUIState", AppController.GUIState.PROMO, SendMessageOptions.DontRequireReceiver);
		
		storeRunCounterState();
	}
	
	
	public void OnGUI()
	{
		if(AppController.getInstance().guiState != AppController.GUIState.PROMO)
			return;
		
		GUI.skin = guiSkin;
		
		GUI.BeginGroup(AppController.guiRect);
		
		GUI.Box(new Rect(0,0,320,480),"");
		GUI.Box(new Rect(0,0,320,480),"");
		//GUI.Box(new Rect(0,0,320,480), StringTable.getString("other_apps"),largeFont);
		GUI.Box(new Rect(0,0,320,480), LocalizedCachedStringAsTexture.getInstance().getCachedString("other_apps"),largeFont);
		
		
		if(GUI.Button(new Rect(290,0,30,30),"X"))
		{
			storePromoVersionState();
			SendMessageUpwards("setGUIState",AppController.GUIState.HIDDEN,SendMessageOptions.DontRequireReceiver);
		}
		
		scrollPosition = GUI.BeginScrollView(new Rect(20,32,280,400),scrollPosition,new Rect(0,0,260,promoData.Count*160>400?promoData.Count*160:400),false, false);

		int currX = 0;
		int currY = 0;
		foreach(PromoData pd in promoData)
		{
			if(GUI.Button(new Rect(currX,currY,256,140), pd.downloadedIcon))
			{
				Application.OpenURL(pd.linkURL);
			}
			//GUI.Label(new Rect(currX + 145,currY,115, 140),pd.message,smallFont);
			currY += 160;
			
		}
		
		GUI.EndScrollView();
		
		GUI.EndGroup();
		
	}
	
}

