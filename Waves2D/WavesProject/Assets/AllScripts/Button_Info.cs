using UnityEngine;
using System.Collections;

public class Button_Info : Button {
	public GameObject target;

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

       target.SendMessage("showWaveGUI", true, SendMessageOptions.DontRequireReceiver);
		
    
	}
	

}
