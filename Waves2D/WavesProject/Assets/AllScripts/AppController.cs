using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AppController : MonoBehaviour {
	public GameObject infoButton;
	public InAppPurchaseGUI iapGUI;
	public GUIMainScreen waveGUI;
	public bool showAds = true;
	
	private const string SHOW_ADS = "show_ads";

	void Start () {
	// Listens to all the StoreKit events.  All event listeners MUST be removed before this object is disposed!
		StoreKitManager.purchaseSuccessful += purchaseSuccessful;
		StoreKitManager.purchaseCancelled += purchaseCancelled;
		StoreKitManager.purchaseFailed += purchaseFailed;
		StoreKitManager.receiptValidationFailed += receiptValidationFailed;
		StoreKitManager.receiptValidationSuccessful += receiptValidationSuccessful;
		StoreKitManager.productListReceived += productListReceived;
		
		showAds =  PlayerPrefs.GetInt(SHOW_ADS) != 0;
		if(showAds)
		{
			print("Creating ad banner on bottom");
			AdBinding.createAdBanner( true );
		}
	}
	
	void OnDisable()
	{
		// Remove all the event handlers
		StoreKitManager.purchaseSuccessful -= purchaseSuccessful;
		StoreKitManager.purchaseCancelled -= purchaseCancelled;
		StoreKitManager.purchaseFailed -= purchaseFailed;
		StoreKitManager.receiptValidationFailed -= receiptValidationFailed;
		StoreKitManager.receiptValidationSuccessful -= receiptValidationSuccessful;
		StoreKitManager.productListReceived -= productListReceived;;
		
		print("Destroying ad banner");
		AdBinding.destroyAdBanner();
	}
	
	void showIAPGUI(bool v)
	{
		iapGUI.showGUI = v;
		if(v == false)
			showInfoButton(true);
	}
	
	void showWaveGUI(bool v)
	{
		waveGUI.showGUI = v;
		showInfoButton(!v);
	}
	
	void showInfoButton(bool v)
	{
		infoButton.guiTexture.enabled = v;
		Button_Info bi = (Button_Info)infoButton.GetComponent("Button_Info");
		if(bi)
			bi.enabled = v;
	}
	
	void permanentlyRemoveAds()
	{
		PlayerPrefs.SetInt(SHOW_ADS,1);
		showAds = false;
		print("Destroying ad banner");
		AdBinding.destroyAdBanner();
	}
	void productListReceived( List<StoreKitProduct> productList )
	{
		Debug.Log( "total productsReceived: " + productList.Count );
		
		// Do something more useful with the products than printing them to the console
		foreach( StoreKitProduct product in productList )
			Debug.Log( product.ToString() + "\n" );
		
		permanentlyRemoveAds();
	}
	
	
	void receiptValidationSuccessful()
	{
		Debug.Log( "receipt validation successful" );
	}
	
	
	void receiptValidationFailed( string error )
	{
		Debug.Log( "receipt validation failed with error: " + error );
	}
	

	void purchaseFailed( string error )
	{
		Debug.Log( "purchase failed with error: " + error );
	}
	

	void purchaseCancelled( string error )
	{
		Debug.Log( "purchase cancelled with error: " + error );
	}
	
	
	void purchaseSuccessful( string productIdentifier, string receipt, int quantity )
	{
		Debug.Log( "purchased product: " + productIdentifier + ", quantity: " + quantity );
		permanentlyRemoveAds();
	}
	
}
