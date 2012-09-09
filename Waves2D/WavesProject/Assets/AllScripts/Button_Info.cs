using UnityEngine;
using System.Collections;

public class Button_Info : Button {
	public GameObject target;
	public AppController appControl;

	// Use this for initialization
	void Start () {
		mouseDownInside = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		ClickChecker();

	}
	
	protected override void MouseDown()
	{

		base.MouseDown();
	   print("SendMessage('showWaveGUI', true, SendMessageOptions.DontRequireReceiver)");
       target.SendMessage("setGUIState", AppController.GUIState.FLUID_PROPERTIES, SendMessageOptions.DontRequireReceiver);
		
		//appControl.showWaveGUI(true);
    
	}
	

}
