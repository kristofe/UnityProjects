using UnityEngine;


public class MobclixAdapter : MonoBehaviour
{
	public bool bannerOnBottom = true;
	public bool autorotateAds = true;
	public bool isLandscape = true;
	public float refreshRate = 30.0f;

	
	void Start()
	{
		MobclixBinding.start();
		MobclixBinding.showBanner( bannerOnBottom );
		MobclixBinding.setRefreshTime( refreshRate );
		
		// if we are not autorotating ads we can get out of here and destroy ourself
		if( !autorotateAds )
			Destroy( gameObject );
	}
	

	void Update()
	{
		// only update if we switched orientation
		if( iPhoneSettings.screenOrientation != ( iPhoneScreenOrientation )Input.deviceOrientation )
		{
			if( isLandscape )
			{
				// Allow rotating to landscape left/right
				if( Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight )
				{
					MobclixBinding.rotateToOrientation( Input.deviceOrientation );
					iPhoneSettings.screenOrientation = ( iPhoneScreenOrientation )Input.deviceOrientation;
				}
			}
			else
			{
				// Allow rotating to portrait and portrait upsideDown
				if( Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown )
				{
					MobclixBinding.rotateToOrientation( Input.deviceOrientation );
					iPhoneSettings.screenOrientation = ( iPhoneScreenOrientation )Input.deviceOrientation;
				}
			}
		}
	}


}
