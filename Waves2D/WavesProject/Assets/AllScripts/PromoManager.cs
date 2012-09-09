using UnityEngine;
using System.Collections;
using System;


public class PromoData
{
	public string appName;
	public string language;
	public string iconURL;
	public string linkURL;
	public string message;
	public Texture2D downloadedIcon;
	
	public PromoData(string[] fields)
	{
		appName = fields[0];
		language = fields[1];
		iconURL = fields[2];
		linkURL = fields[3];
		message = fields[4];
	}
	
	public void debugPrint()
	{
		Debug.Log ("PromoData: appName: " + appName + " language: " + language + " iconURL: " + iconURL + " linkURL: " + linkURL + " message: " + message + " textureDim: "
			+ downloadedIcon.width + ", " + downloadedIcon.height);
	}
};

public class PromoManager : MonoBehaviour {
	public string promoFileURL;
	public string appID;
	public string lastVersionDisplayed;
	public ArrayList promoData;
	public int showEveryCount;
	
	private int promoVersion;
	private int lastPromoVersion;
	private int runCounter;
	
	private Vector2 scrollPosition;
	
	private const string LAST_PROMO_SHOWN = "promo_shown";
	private const string RUN_COUNTER = "run_counter";
	
	
	// Use this for initialization
	void Start () {
		lastPromoVersion = -1;
		runCounter = 0;
#if UNITY_IPHONE
		lastPromoVersion = PlayerPrefs.GetInt(LAST_PROMO_SHOWN);
		runCounter = PlayerPrefs.GetInt(RUN_COUNTER);
		runCounter++;
		
#endif
		
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
	
	public IEnumerator processPromo()
	{
		WWW promoFile = new WWW(promoFileURL);
		
		while(promoFile.isDone == false)
			yield return 0;
		
		if(promoFile.error != null)
		{
			Debug.LogError("Couldn't load promotion at url: " + promoFileURL + " " + promoFile.error);
			return false;
		}
		
		//Got file.  Now parse it
		string txt = promoFile.text;
		
		string[] lines = txt.Split('\n');
		//Check version
		string firstLine = lines[0].Trim();
		promoVersion = Int32.Parse(firstLine);
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
			
			if(fields.Length < 5)
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
		if(promoVersion > lastPromoVersion || runCounter % showEveryCount == 0)
			SendMessage("setGUIState", AppController.GUIState.PROMO, SendMessageOptions.DontRequireReceiver);
		
		storeRunCounterState();
	}
	
	
	public void OnGUI()
	{
		if(AppController.getInstance().guiState != AppController.GUIState.PROMO)
			return;
		
		if(GUI.Button(new Rect(Screen.width - 30,0,30,30),"X"))
		{
			storePromoVersionState();
			SendMessageUpwards("setGUIState",AppController.GUIState.HIDDEN,SendMessageOptions.DontRequireReceiver);
		}
		
		GUI.Box(new Rect(0,0,320,480), "Try Our Other Products");
		
		scrollPosition = GUI.BeginScrollView(new Rect(20,32,280,400),scrollPosition,new Rect(0,0,260,promoData.Count*160),false, true);

		int currX = 0;
		int currY = 0;
		foreach(PromoData pd in promoData)
		{
			if(GUI.Button(new Rect(currX,currY,140,140), pd.downloadedIcon))
			{
				Application.OpenURL(pd.linkURL);
			}
			GUI.Label(new Rect(currX + 160,currY,100, 140),pd.message);
			currY += 160;
			
		}
		
		GUI.EndScrollView();
		
	}
	
}

