using UnityEngine;
using System.Collections;

public class GUIMainScreen : MonoBehaviour {
	public Material targetMaterial;
	public Texture newTexture01;
	public Texture newTexture02;
	public Texture newTexture03;
	public Texture newTexture04;
	public Texture newTexture05;
	public Texture newTexture06;
	
	public Texture newTexture07;
	public Texture newTexture08;
	public Texture newTexture09;
	public Texture newTexture10;
	public Texture newTexture11;
	public Texture newTexture12;
	
	public Texture texture01;
	public Texture texture02;
	public Texture texture03;
	public Texture texture04;
	public Texture texture05;
	public Texture texture06;
	public Texture texture07;
	public Texture texture08;
	public Texture texture09;

	public Texture noBanner;
	
	public GUISkin guiSkin;
	public GUIStyle textStyle;
	
	//[System.NonSerialized]
	private Vector2 scrollPosition;
	private Vector2 scrollPosition2;
	
	[System.NonSerialized]
	public bool showGUI = false;
	
	void Start () {
		GUIStyleState gss = new GUIStyleState();
		scrollPosition = Vector2.zero;
		scrollPosition2 = Vector2.zero;
		gss.textColor = Color.white;
		textStyle.alignment = TextAnchor.MiddleCenter;
		textStyle.normal = gss;
		showGUI = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ToggleGUI()
	{
		showGUI = !showGUI;
		
	}
	
	void OnGUI()
	{
		if(!showGUI)
			return;
			
		GUI.skin = guiSkin;

		DrawBackground();
		DrawMaps();	
		DrawButtons();

	}
	
	void DrawBackground()
	{
		GUI.Box(new Rect(0,0,320,480), "*");
		//GUI.Label(new Rect(0,25,320,25), "Reflection", textStyle);
		//GUI.Label(new Rect(0,260,320,25), "Background",textStyle);
	}
	
	
	void DrawMaps() 
	{
		scrollPosition = GUI.BeginScrollView(new Rect(0,50,320,140),scrollPosition,new Rect(0,0,300,220),false, true);

		if(GUI.Button(new Rect(20,0,64,64),newTexture01))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture01);
		}
		if(GUI.Button(new Rect(20,70,64,64),newTexture02))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture02);
		}
		if(GUI.Button(new Rect(20,0+140,64,64),newTexture03))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture03);
		}
		
		if(GUI.Button(new Rect(90,0,64,64),newTexture04))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture04);
		}
		if(GUI.Button(new Rect(90,70,64,64),newTexture05))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture05);
		}
		if(GUI.Button(new Rect(90,140,64,64),newTexture06))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture06);
		}
		
		
		
		if(GUI.Button(new Rect(160,0,64,64),newTexture07))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture07);
		}
		if(GUI.Button(new Rect(160,70,64,64),newTexture08))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture08);
		}
		if(GUI.Button(new Rect(160,140,64,64),newTexture09))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture09);
		}
		
		if(GUI.Button(new Rect(230,0,64,64),newTexture10))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture10);
		}
		if(GUI.Button(new Rect(230,70,64,64),newTexture11))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture11);
		}
		if(GUI.Button(new Rect(230,140,64,64),newTexture12))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture12);
		}
		
		GUI.EndScrollView();

		scrollPosition2 = GUI.BeginScrollView(new Rect(0,205,320,140),scrollPosition2,new Rect(0,0,300,150));
		if(GUI.Button(new Rect(20,0,64,64),texture01))
		{
			targetMaterial.SetTexture("_MainTex", texture01);
		}
		if(GUI.Button(new Rect(20,70,64,64),texture02))
		{
			targetMaterial.SetTexture("_MainTex", texture02);
		}

		
		if(GUI.Button(new Rect(90,0,64,64),texture03))
		{
			targetMaterial.SetTexture("_MainTex", texture03);
		}
		if(GUI.Button(new Rect(90,70,64,64),texture04))
		{
			targetMaterial.SetTexture("_MainTex", texture04);
		}

		
		if(GUI.Button(new Rect(160,0,64,64),texture05))
		{
			targetMaterial.SetTexture("_MainTex", texture05);
		}
		if(GUI.Button(new Rect(160,70,64,64),texture06))
		{
			targetMaterial.SetTexture("_MainTex", texture06);
		}

		
		
		if(GUI.Button(new Rect(230,0,64,64),texture07))
		{
			targetMaterial.SetTexture("_MainTex", texture07);
		}
		if(GUI.Button(new Rect(230,70,64,64),texture08))
		{
			targetMaterial.SetTexture("_MainTex", texture08);
		}
		GUI.EndScrollView();
	}
	
	
	void DrawButtons()
	{
		if(GUI.Button(new Rect(0,0,30,30),"X"))
		{
			SendMessageUpwards("showWaveGUI",false,SendMessageOptions.DontRequireReceiver);
		}	
		
		if(AppController.showAds)
		{
			
			if(GUI.Button(new Rect(92,345,128,64),noBanner))
			{
				StoreKitBinding.purchaseProduct( "com.blackicegamesnyc.remove_ads", 1 );
			}
		}

	}
}
