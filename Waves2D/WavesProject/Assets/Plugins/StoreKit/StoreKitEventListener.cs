using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StoreKitEventListener : MonoBehaviour
{
	void Start()
	{
		// Listens to all the StoreKit events.  All event listeners MUST be removed before this object is disposed!
		StoreKitManager.purchaseSuccessful += purchaseSuccessful;
		StoreKitManager.purchaseCancelled += purchaseCancelled;
		StoreKitManager.purchaseFailed += purchaseFailed;
		StoreKitManager.receiptValidationFailed += receiptValidationFailed;
		StoreKitManager.receiptValidationSuccessful += receiptValidationSuccessful;
		StoreKitManager.productListReceived += productListReceived;
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
	}
	
	
	void productListReceived( List<StoreKitProduct> productList )
	{
		Debug.Log( "total productsReceived: " + productList.Count );
		
		// Do something more useful with the products than printing them to the console
		foreach( StoreKitProduct product in productList )
			Debug.Log( product.ToString() + "\n" );
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
	}
	
	
	void Update()
	{
	
	}
}
