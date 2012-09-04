using UnityEngine;


public class AdAdapter : MonoBehaviour
{
	public bool bannerOnBottom = true;
	public bool autorotateAds = true;
	public bool fireHideShowEvents = false;
	public bool allowLandscapeOrientations = true;
	public bool allowPortraitOrientations = false;
	
	
	public void Start()
	{
		// start up iAd and destroy ourself
		AdBinding.createAdBanner( bannerOnBottom );
		
		// do we want to listen to hide/show events?
		if( fireHideShowEvents )
		{
			// we are listenting to events, so dont destroy this object or else native will have no one to talk to
			DontDestroyOnLoad( this );
			
			AdBinding.fireHideShowEvents( true );
			AdManager.adViewDidChange += adViewDidChange;
		}
		
		// if we are not autorotating ads or listening to events we can get out of here and destroy ourself
		if( !autorotateAds && !fireHideShowEvents )
			Destroy( gameObject );
	}


	void adViewDidChange( bool adDidShow )
	{
		Debug.Log( "Ad is visible? " +  adDidShow );
	}
	
	
	void Update()
	{
		// only update if we switched orientation
		if( iPhoneSettings.screenOrientation != ( iPhoneScreenOrientation )Input.deviceOrientation )
		{
			if( allowLandscapeOrientations )
			{
				// Allow rotating to landscape left/right
				if( Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight )
				{
					AdBinding.rotateToOrientation( Input.deviceOrientation );
					iPhoneSettings.screenOrientation = ( iPhoneScreenOrientation )iPhoneInput.orientation;
				}
			}
			
			
			if( allowPortraitOrientations )
			{
				// Allow rotating to landscape left/right
				if( Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown )
				{
					AdBinding.rotateToOrientation( Input.deviceOrientation );
					iPhoneSettings.screenOrientation = ( iPhoneScreenOrientation )iPhoneInput.orientation;
				}
			}
		}
	}

}
