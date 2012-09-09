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
		foreach(string f in fields)
		{
			Debug.Log(f);
		}
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
	
	
	
	// Use this for initialization
	void Start () {
		promoData = new ArrayList();
		
		StartCoroutine("processPromo");
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
		int version = Int32.Parse(firstLine);
		Debug.Log("Promo Version: " + version);
		
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
		
		SendMessage("setGUIState", AppController.GUIState.PROMO, SendMessageOptions.DontRequireReceiver);
		
	}
	
	
	public void OnGUI()
	{
		if(AppController.getInstance().guiState != AppController.GUIState.PROMO)
			return;
		
		if(GUI.Button(new Rect(0,0,30,30),"X"))
		{
			SendMessageUpwards("setGUIState",AppController.GUIState.HIDDEN,SendMessageOptions.DontRequireReceiver);
		}
		
		GUI.Box(new Rect(0,0,320,480), "Try Our Other Products");
		
		int currX = 20;
		int currY = 64;
		foreach(PromoData pd in promoData)
		{
			if(GUI.Button(new Rect(currX,currY,140,140), pd.downloadedIcon))
			{
				Application.OpenURL(pd.linkURL);
			}
			GUI.Label(new Rect(currX + 160,currY,480-160, 140),pd.message);
			currY += 160;
			
		}
		
	}
	// Update is called once per frame
	void Update () {
	
	}
}

