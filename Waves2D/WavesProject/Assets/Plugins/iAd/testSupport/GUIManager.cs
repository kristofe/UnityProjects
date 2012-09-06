using UnityEngine;
using System.Collections.Generic;


public class GUIManager : MonoBehaviour
{
	bool isPad;
	
	void Start()
	{
		// hack to detect iPad 3 until Unity adds official support
		this.isPad = ( Screen.width >= 1024 || Screen.height >= 1024 );
		
		if( isPad )
		{
			// listen to interstital events for illustration purposes
			AdManager.interstitalAdFailed += delegate( string error )
			{
				Debug.Log( "interstitalAdFailed: " + error );
			};
			
			AdManager.interstitialAdLoaded += delegate()
			{
				Debug.Log( "interstitialAdLoaded" );
			};
		}
	}
	
	
	void OnGUI()
	{
		GUI.matrix = Matrix4x4.TRS( new Vector3( 0, 0, 0 ), Quaternion.identity, new Vector3( Screen.width / 1024.0f, Screen.height / 768.0f, 1 ) );
		
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 960 || Screen.height >= 960 ) ? 320 : 160;
		float height = ( Screen.width >= 960 || Screen.height >= 960 ) ? 80 : 40;
		float heightPlus = height + 10.0f;

		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Create Ad Banner" ) )
		{
			AdBinding.createAdBanner( true );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Destroy Ad Banner" ) )
		{
			AdBinding.destroyAdBanner();
		}

		
		if( isPad )
		{
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Initialize Interstitial" ) )
			{
				bool result = AdBinding.initializeInterstitial();
				Debug.Log( "initializeInterstitial: " + result );
			}
			
			
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Is Interstitial Loaded?" ) )
			{
				bool result = AdBinding.isInterstitalLoaded();
				Debug.Log( "isInterstitalLoaded: " + result );
			}
			
			
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Interstitial" ) )
			{
				bool result = AdBinding.showInterstitial();
				Debug.Log( "showInterstitial: " + result );
			}
		}

	}
}
