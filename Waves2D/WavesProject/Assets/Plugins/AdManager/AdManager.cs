using UnityEngine;
using System.Collections;


public class AdManager : MonoBehaviour
{
	// Events
	public delegate void AdManagerEventHandler( bool adDidShow );
	public static event AdManagerEventHandler adViewDidChange;
	
	// Fired when the adView is either shown or hidden
	public static bool adViewIsShowing = false;
	
	
    void Awake()
    {
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
    }


	// Called when an ad hides or shows
	public void adViewDidShow( string returnValue )
	{
		adViewIsShowing = ( returnValue == "1" );
	
		if( adViewDidChange != null )
			adViewDidChange( adViewIsShowing );
	}

}
