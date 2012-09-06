using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour {
	public GameObject infoButton;
	public InAppPurchaseGUI iapGUI;
	public GUIMainScreen waveGUI;
	
	// Use this for initialization
	void Start () {
	
		print("Creating ad banner on bottom");
		AdBinding.createAdBanner( true );
	}
	
	void OnDisable()
	{
		print("Destroying ad banner");
		AdBinding.destroyAdBanner();
	}
	
	void showIAPGUI(bool v)
	{
		iapGUI.showGUI = v;
		if(v == false)
			showInfoButton(true);
	}
	
	void showWaveGUI(bool v)
	{
		waveGUI.showGUI = v;
		showInfoButton(!v);
	}
	
	void showInfoButton(bool v)
	{
		infoButton.guiTexture.enabled = v;
		Button_Info bi = (Button_Info)infoButton.GetComponent("Button_Info");
		if(bi)
			bi.enabled = v;
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
