using UnityEngine;
using System.Collections;


public class AdManager : MonoBehaviour
{
	// Events and delegates
	public delegate void AdManagerEmptyEventHandler();
	public delegate void AdManagerStringEventHandler( string error );
	public delegate void AdManagerEventHandler( bool adDidShow );
	
	// Fired when the adView is either shown or hidden
	public static event AdManagerEventHandler adViewDidChange;
	
	// Fired when an interstial ad fails to load or show
	public static event AdManagerStringEventHandler interstitalAdFailed;
	
	// Fired when an interstitial ad is loaded and ready to show
	public static event AdManagerEmptyEventHandler interstitialAdLoaded;
	
	
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
	
	
	public void interstitialFailed( string error )
	{
		if( interstitalAdFailed != null )
			interstitalAdFailed( error );
	}
	
	
	
	public void interstitialLoaded( string empty )
	{
		if( interstitialAdLoaded != null )
			interstitialAdLoaded();
	}

}
