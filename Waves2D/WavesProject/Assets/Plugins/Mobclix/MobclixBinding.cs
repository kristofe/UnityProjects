using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public enum MobclixLogLevel
{
	Debug = 0,
	Info = 1,
	Warn = 2,
	Error = 3,
	Fatal = 4
}



// All Objective-C exposed methods should be bound here
public class MobclixBinding
{
    [DllImport("__Internal")]
    private static extern void _mobclixStart();

	// Starts up the Mobclix integration
    public static void start()
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_mobclixStart();
    }


    [DllImport("__Internal")]
    private static extern void _mobclixSetRefreshTime( float refreshTime );

	// Sets the rate at which ads refresh.
    public static void setRefreshTime( float refreshTime )
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_mobclixSetRefreshTime( refreshTime );
    }
    
    	
	
    [DllImport("__Internal")]
    private static extern void _mobclixShowBanner( bool bannerIsOnBottom );
 
    public static void showBanner( bool bannerIsOnBottom )
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_mobclixShowBanner( bannerIsOnBottom );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _mobclixHideBanner();
 
    public static void hideBanner()
    {
        // Call plugin only when running on real device
        if( Application.platform != RuntimePlatform.OSXEditor )
			_mobclixHideBanner();
    }
	
	
	[DllImport("__Internal")]
	private static extern void _mobclixRotateToOrientation( int orientation );
	
	public static void rotateToOrientation( DeviceOrientation orientation )
	{
	    // Call plugin only when running on real device
	    if( Application.platform != RuntimePlatform.OSXEditor )
			_mobclixRotateToOrientation( (int)orientation );
	}
	
	
	[DllImport("__Internal")]
	private static extern void _mobclixLogEvent( int logLevel, string processName, string eventName, string description, bool stopProcess );
	
	// Logs events via Mobclix
	public static void logEvent( MobclixLogLevel logLevel, string processName, string eventName, string description, bool stopProcess )
	{
	    // Call plugin only when running on real device
	    if( Application.platform != RuntimePlatform.OSXEditor )
			_mobclixLogEvent( (int)logLevel, processName, eventName, description, stopProcess );
	}

}