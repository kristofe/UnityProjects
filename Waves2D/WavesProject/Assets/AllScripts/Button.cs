#define PC
using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	protected bool mouseDownInside;
	protected Vector2 buttonSize;
	public Vector2 touchOffsetPercent;
	public Vector3 touchPosition;
	public Vector3 buttonCenter;
	public Vector2 buttonCenter2D;
	public Vector2 buttonPosition;
	public Rect buttonRect;
	public Color buttonUpColor = new Color(0.5f,0.5f,0.5f,0.25f);
	public Color buttonDownColor = new Color(0.05f,0.05f,0.05f,0.5f);
	// Use this for initialization
	void Start () {
		init();
	}
	
	protected void init()
	{
		mouseDownInside = false;
		buttonRect = guiTexture.GetScreenRect();
		Vector3 screenPosition = new Vector3(buttonRect.x + (buttonRect.width*0.5f), buttonRect.y + (buttonRect.height * 0.5f),0);
		buttonSize = new Vector2(buttonRect.width,buttonRect.height);
		buttonPosition = new Vector2(buttonRect.x,buttonRect.y);
		if(Camera.current)
		{
			buttonCenter = Camera.current.ScreenToWorldPoint(screenPosition);
			buttonCenter2D = new Vector2(buttonCenter.x,buttonCenter.y);
		}
		guiTexture.color = buttonUpColor;
		
	}
	
	// Update is called once per frame
	void Update () {
		ClickChecker();

	}
	
	protected virtual void ClickChecker()
	{	
		#if IPHONE
		if(Application.platform != RuntimePlatform.OSXEditor)
		{
			CheckForClickIPhone();
		}else
		{
		
			CheckForClickPC();

		}
		#else
			CheckForClickPC();
		#endif
	}
		
	protected virtual void CheckForClickPC()
	{

		if(MouseInside(Input.mousePosition))
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(!mouseDownInside)
				{
					MouseDown();
				}
			}
			else if(Input.GetMouseButton(0))
			{
				MouseHold();	
			}
			else if(Input.GetMouseButtonUp(0))
			{
				MouseUp();	
			}
			
		}else{
			if(mouseDownInside){
				MouseUp();
			}
		}		
	}
	
	#if IPHONE
	protected virtual void CheckForClickIPhone()
	{
		bool touchInside = false;
		if(iPhoneInput.touchCount > 0)
	    {
			for(int i = 0; i < iPhoneInput.touchCount; ++i)
			{
				iPhoneTouch touch = iPhoneInput.GetTouch(i);
				if(MouseInside(touch.position))
				{
					touchInside = true;
					if(touch.phase == iPhoneTouchPhase.Began)
					{
						if(!mouseDownInside)
						{
							MouseDown();
						}
					}
					else if(touch.phase == iPhoneTouchPhase.Stationary || touch.phase == iPhoneTouchPhase.Moved)
					{
						MouseHold();	
					}
					else if(touch.phase == iPhoneTouchPhase.Ended)
					{
						MouseUp();	
					}
					
				}

				//Debug.Log("Touch("+i+"): " + touch + "\ntouch.position: " + touch.position);
				//Debug.Log("Button::CheckForClickIPhone()  tapped outside button ");
			}
	
	    }
 		if(touchInside == false && mouseDownInside)//Mouse Up Outside the button.
		{
			MouseUp();
		}
		
	}
	#endif
	
	protected bool MouseInside(Vector2 pos)
	{
		touchPosition = new Vector3(pos.x,pos.y,0);
		if(guiTexture.HitTest(touchPosition))
		{
			Vector2 buttonPos = pos - buttonPosition;
			touchOffsetPercent = new Vector2(((buttonPos.x/buttonSize.x) -0.5f)*2.0f,((buttonPos.y/buttonSize.y) - 0.5f)*2.0f);
			return true;
		}
		return false;
	}
	
	protected virtual void MouseDown()
	{
		mouseDownInside = true;
		guiTexture.color = buttonDownColor;
		//Debug.Log("MouseDown");
	}
	
	
	protected virtual void MouseHold()
	{
		if(!mouseDownInside)
		{
			return;
		}
		//Debug.Log("MouseHold");
	}
	
	protected virtual void MouseUp()
	{
		mouseDownInside = false;
		guiTexture.color = buttonUpColor;
		//Debug.Log("MouseUp");	
	}
}
