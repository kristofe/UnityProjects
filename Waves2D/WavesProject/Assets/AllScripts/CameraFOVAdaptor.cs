using UnityEngine;
using System.Collections;

public class CameraFOVAdaptor : MonoBehaviour {
	public float iPhoneFOV = 4.8f;
	public float iPadFOV = 4.27f;
	
	// Use this for initialization
	public void updateCamera () {
		if(AppController.onIPad)
			Camera.main.orthographicSize = iPadFOV;
		else
			Camera.main.orthographicSize = iPhoneFOV;
			
	
	}
	

}
