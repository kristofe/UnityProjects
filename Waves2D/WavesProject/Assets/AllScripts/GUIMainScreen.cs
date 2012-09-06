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

	
	public GUISkin guiSkin;
	public GUIStyle textStyle;
	private Vector2 scrollPosition;
	
	[System.NonSerialized]
	public bool showGUI = false;
	
	void Start () {
		GUIStyleState gss = new GUIStyleState();;
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
		//if(iPhoneSettings.generation == iPhoneGeneration.iPad1Gen)
		{
			 GUI.BeginGroup (new Rect (Screen.width/2 - 320/2, Screen.height / 2 - 480/2, 320, 480));
		}
		DrawBackground();
		DrawButtons();	
		
		//if(iPhoneSettings.generation == iPhoneGeneration.iPad1Gen)
		{
			 GUI.EndGroup ();
		}
		if(GUI.changed)
		{
			//FIgure out what contact has been chosen
			//Debug.Log(scroll_y + " => " + scrollAreaHeight);
			
		}
	}
	
	void DrawBackground()
	{
		//GUI.Box(new Rect(0,0,320,480), "Fluid Options");
		GUI.Label(new Rect(0,25,320,25), "Reflection", textStyle);
		GUI.Label(new Rect(0,260,320,25), "Background",textStyle);
	}
	
	void DrawScrollList()
	{
		
		scrollPosition = GUI.BeginScrollView(new Rect(0 +10,20+20,310,200),scrollPosition,new Rect(0,0,280,250));

		int currHeight = 0;
		
		for(int i = 0; i < 10; ++i)
		{
			GUI.Label(new Rect(0,currHeight,260,25),"Movie " + i);	
			currHeight += 25;
		}

	
		GUI.EndScrollView();
		
	}
	
	
	
	void DrawButtons()
	{
		int oyBg = 235;
		if(GUI.Button(new Rect(0,0,30,30),"X"))
		{
			SendMessageUpwards("showWaveGUI",false,SendMessageOptions.DontRequireReceiver);
		}
		
		if(GUI.Button(new Rect(0+20,0+50,64,64),newTexture01))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture01);
		}
		if(GUI.Button(new Rect(0+20,0+120,64,64),newTexture02))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture02);
		}
		if(GUI.Button(new Rect(0+20,0+190,64,64),newTexture03))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture03);
		}
		
		if(GUI.Button(new Rect(0+90,0+50,64,64),newTexture04))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture04);
		}
		if(GUI.Button(new Rect(0+90,0+120,64,64),newTexture05))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture05);
		}
		if(GUI.Button(new Rect(0+90,0+190,64,64),newTexture06))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture06);
		}
		
		
		
		if(GUI.Button(new Rect(0+160,0+50,64,64),newTexture07))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture07);
		}
		if(GUI.Button(new Rect(0+160,0+120,64,64),newTexture08))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture08);
		}
		if(GUI.Button(new Rect(0+160,0+190,64,64),newTexture09))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture09);
		}
		
		if(GUI.Button(new Rect(0+230,0+50,64,64),newTexture10))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture10);
		}
		if(GUI.Button(new Rect(0+230,0+120,64,64),newTexture11))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture11);
		}
		if(GUI.Button(new Rect(0+230,0+190,64,64),newTexture12))
		{
			targetMaterial.SetTexture("_EnvTex", newTexture12);
		}
		
		
		/////////////////////////////////////////////////////////
		if(GUI.Button(new Rect(0+20,oyBg+50,64,64),texture01))
		{
			targetMaterial.SetTexture("_MainTex", texture01);
		}
		if(GUI.Button(new Rect(0+20,oyBg+120,64,64),texture02))
		{
			targetMaterial.SetTexture("_MainTex", texture02);
		}

		
		if(GUI.Button(new Rect(0+90,oyBg+50,64,64),texture03))
		{
			targetMaterial.SetTexture("_MainTex", texture03);
		}
		if(GUI.Button(new Rect(0+90,oyBg+120,64,64),texture04))
		{
			targetMaterial.SetTexture("_MainTex", texture04);
		}

		
		
		
		if(GUI.Button(new Rect(0+160,oyBg+50,64,64),texture05))
		{
			targetMaterial.SetTexture("_MainTex", texture05);
		}
		if(GUI.Button(new Rect(0+160,oyBg+120,64,64),texture06))
		{
			targetMaterial.SetTexture("_MainTex", texture06);
		}

		
		
		if(GUI.Button(new Rect(0+230,oyBg+50,64,64),texture07))
		{
			targetMaterial.SetTexture("_MainTex", texture07);
		}
		if(GUI.Button(new Rect(0+230,oyBg+120,64,64),texture08))
		{
			targetMaterial.SetTexture("_MainTex", texture08);
		}

		
		if(GUI.Button(new Rect(50,0,244,30),"Remove Advertisements"))
		{
			showGUI = false;
			SendMessageUpwards("showIAPGUI",true,SendMessageOptions.DontRequireReceiver);
			
		}

	}
}
