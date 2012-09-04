using UnityEngine;
using System.Runtime.InteropServices;


public static class AdBinding
{
    [DllImport("__Internal")]
    private static extern void _iAdCreateAdBanner( bool bannerOnBottom );

	// Starts up iAd requests and ads the ad view
    public static void createAdBanner( bool bannerOnBottom )
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_iAdCreateAdBanner( bannerOnBottom );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _iAdDestroyAdBanner();

	// Destroys the ad banner and removes it from view
    public static void destroyAdBanner()
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_iAdDestroyAdBanner();
    }	

	
    [DllImport("__Internal")]
    private static extern void _iAdRotateToOrientation( int orientation );

	// Switches the orientation of the ad view
    public static void rotateToOrientation( DeviceOrientation orientation )
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_iAdRotateToOrientation( (int)orientation );
    }
	
	
	
    [DllImport("__Internal")]
    private static extern void _iAdFireHideShowEvents( bool shouldFire );

	// Switches the orientation of the ad view
    public static void fireHideShowEvents( bool shouldFire )
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_iAdFireHideShowEvents( shouldFire );
    }

	
}
