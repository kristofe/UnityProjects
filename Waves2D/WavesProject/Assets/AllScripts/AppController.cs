using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AppController : MonoBehaviour {
	public enum GUIState {
		HIDDEN,
		FLUID_PROPERTIES,
		PROMO
	}
		
	public GameObject infoButton;
	public InAppPurchaseGUI iapGUI;
	public GUIMainScreen waveGUI;
	public PromoManager promoMgr;
	public CameraFOVAdaptor cameraMgr;
	
	[System.NonSerialized]
	public static bool showAds = true;
	
	[System.NonSerialized]
	public static bool onIPad = false;
	
	[System.NonSerialized]
	public static Rect guiRect = new Rect(0,0,320,480);
	
	private static AppController instance;
	public GUIState guiState;
	
	private const string DISABLE_ADS = "disable_ads";

	public void Start () {
		
		AppController.instance = this;
		StringTable.loadFile();
		
		int disableAds = PlayerPrefs.GetInt(DISABLE_ADS);
		print ("PlayerPrefs.GetInt(DISABLE_ADS) = " + disableAds);
		AppController.showAds = disableAds == 0;
		if(AppController.showAds)
		{
			print("Creating ad banner on bottom");
			AdBinding.createAdBanner( true );
		}
		
		if(Screen.width > 320)
		{
			onIPad = true;
			guiRect.x = Screen.width/2 -160;
			guiRect.y = Screen.height/2 - 240;
		}
		cameraMgr.updateCamera();
	}
	
	public static AppController getInstance()
	{
		return instance;
	}
	
	public PromoManager getPromoMgr()
	{
		return promoMgr;
	}
	
	public void setGUIState(GUIState gs)
	{
		guiState = gs;
		if(guiState == GUIState.HIDDEN)
		{
			showInfoButton(true);
		}
		else
		{
			showInfoButton(false);
		}
	}
	
	public void showInfoButton(bool v)
	{
		infoButton.guiTexture.enabled = v;
		Button_Info bi = (Button_Info)infoButton.GetComponent("Button_Info");
		if(bi)
			bi.enabled = v;
	}
	
	public void permanentlyRemoveAds()
	{
		PlayerPrefs.SetInt(DISABLE_ADS,1);
		AppController.showAds = false;
		print("Destroying ad banner");
		AdBinding.destroyAdBanner();
	}
	
#if UNITY_IPHONE
	void OnEnable()
	{
		print ("Adding storekit listeners");
		// Listens to all the StoreKit events.  All event listeners MUST be removed before this object is disposed!
		StoreKitManager.productPurchaseAwaitingConfirmationEvent += productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelled;
		StoreKitManager.purchaseFailedEvent += purchaseFailed;
		StoreKitManager.receiptValidationFailedEvent += receiptValidationFailed;
		StoreKitManager.receiptValidationRawResponseReceivedEvent += receiptValidationRawResponseReceived;
		StoreKitManager.receiptValidationSuccessfulEvent += receiptValidationSuccessful;
		StoreKitManager.productListReceivedEvent += productListReceived;
		StoreKitManager.productListRequestFailedEvent += productListRequestFailed;
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinished;
		
		string[] productIdentifiers = new string[] { "com.blackicegamesnyc.remove_ads" };
		StoreKitBinding.requestProductData( productIdentifiers );
	}
	
	
	void OnDisable()
	{
		// Remove all the event handlers
		print ("Removing storekit listeners");
		StoreKitManager.productPurchaseAwaitingConfirmationEvent -= productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelled;
		StoreKitManager.purchaseFailedEvent -= purchaseFailed;
		StoreKitManager.receiptValidationFailedEvent -= receiptValidationFailed;
		StoreKitManager.receiptValidationRawResponseReceivedEvent -= receiptValidationRawResponseReceived;
		StoreKitManager.receiptValidationSuccessfulEvent -= receiptValidationSuccessful;
		StoreKitManager.productListReceivedEvent -= productListReceived;
		StoreKitManager.productListRequestFailedEvent -= productListRequestFailed;
		StoreKitManager.restoreTransactionsFailedEvent -= restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinishedEvent -= restoreTransactionsFinished;
	
	}
	
	
	void productListReceived( List<StoreKitProduct> productList )
	{
		Debug.Log( "total productsReceived: " + productList.Count );
		
		// Do something more useful with the products than printing them to the console
		foreach( StoreKitProduct product in productList )
			Debug.Log( product.ToString() + "\n" );
	}
	
	
	void productListRequestFailed( string error )
	{
		Debug.Log( "productListRequestFailed: " + error );
	}
	
	
	void receiptValidationSuccessful()
	{
		Debug.Log( "receipt validation successful" );
	}
	
	
	void receiptValidationFailed( string error )
	{
		Debug.Log( "receipt validation failed with error: " + error );
	}
	
	
	void receiptValidationRawResponseReceived( string response )
	{
		Debug.Log( "receipt validation raw response: " + response );
	}
	

	void purchaseFailed( string error )
	{
		Debug.Log( "purchase failed with error: " + error );
	}
	

	void purchaseCancelled( string error )
	{
		Debug.Log( "purchase cancelled with error: " + error );
	}
	
	
	void productPurchaseAwaitingConfirmationEvent( StoreKitTransaction transaction )
	{
		Debug.Log( "productPurchaseAwaitingConfirmationEvent: " + transaction );
	}
	
	
	void purchaseSuccessful( StoreKitTransaction transaction )
	{
		Debug.Log( "purchased product: " + transaction );
		permanentlyRemoveAds();
	}
	
	
	void restoreTransactionsFailed( string error )
	{
		Debug.Log( "restoreTransactionsFailed: " + error );
	}
	
	
	void restoreTransactionsFinished()
	{
		Debug.Log( "restoreTransactionsFinished" );
	}
#endif
	
}
