#define PC

using UnityEngine;
using System.Collections;

public class LoadNextLevel : MonoBehaviour {
	public bool autoLoadNextLevel = true;
	// Use this for initialization
	void Start () {
		if(autoLoadNextLevel)
			Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	// Update is called once per frame
	void Update () 
	{	
#if PC        	
        if(Input.GetMouseButtonDown(0))
        {
        	guiText.text = "Loading...";
        	Application.LoadLevel(Application.loadedLevel + 1);
        }
#else
		if(iPhoneInput.touchCount > 0)
		{
			guiText.text = "Loading...";
			Application.LoadLevel(Application.loadedLevel + 1);
		}

#endif
	}
}
