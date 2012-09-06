using UnityEngine;
using System;
using System.Collections.Generic;


public class StoreKitManager : MonoBehaviour
{
	// Events
	public delegate void ProductPurchasedEventHandler( string productIdentifier, string receipt, int quantity );
	public static event ProductPurchasedEventHandler purchaseSuccessful;
	
	public delegate void ProductListReceivedEventHandler( List<StoreKitProduct> productList );
	public static event ProductListReceivedEventHandler productListReceived;
	
	public delegate void StoreKitErrorEventHandler( string error );
	public static event StoreKitErrorEventHandler purchaseFailed;
	public static event StoreKitErrorEventHandler purchaseCancelled;
	public static event StoreKitErrorEventHandler receiptValidationFailed;
	
	public delegate void ValidateReceiptSuccessfulEventHandler();
	public static event ValidateReceiptSuccessfulEventHandler receiptValidationSuccessful;
	
	
    void Awake()
    {
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
    }
	
	
	// Called when a product is successfully paid for.  returnValue will hold the productIdentifer and receipt of the purchased product.
	public void productPurchased( string returnValue )
	{
		// split up into useful data
		string[] receiptParts = returnValue.Split( new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries );
		if( receiptParts.Length != 3 )
		{
			if( purchaseFailed != null )
				purchaseFailed( "Could not parse receipt information: " + returnValue );
			return;
		}
			
		string productIdentifier = receiptParts[0];
		string receipt = receiptParts[1];
		int quantity = int.Parse( receiptParts[2] );
		
		if( purchaseSuccessful != null )
			purchaseSuccessful( productIdentifier, receipt, quantity );
	}
	
	
	// Called when a product purchase fails
	public void productPurchaseFailed( string error )
	{
		if( purchaseFailed != null )
			purchaseFailed( error );
	}
	
		
	// Called when a product purchase is cancelled by the user or system
	public void productPurchaseCancelled( string error )
	{
		if( purchaseCancelled != null )
			purchaseCancelled( error );
	}
	
	
	// Called when the product list your required returns.  Automatically serializes the productString into StoreKitProduct's.
	public void productsReceived( string productString )
	{
        List<StoreKitProduct> productList = new List<StoreKitProduct>();

		// parse out the products
        string[] productParts = productString.Split( new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries );
        for( int i = 0; i < productParts.Length; i++ )
            productList.Add( StoreKitProduct.productFromString( productParts[i] ) );
		
		if( productListReceived != null )
			productListReceived( productList );
	}
	
	
	// Called when the validateReceipt call fails
	public void validateReceiptFailed( string error )
	{
		if( receiptValidationFailed != null )
			receiptValidationFailed( error );
	}
	
	
	// Called when the validateReceipt method finishes.  It does not automatically mean success.
	public void validateReceiptFinished( string statusCode )
	{
		if( statusCode == "0" )
		{
			if( receiptValidationSuccessful != null )
				receiptValidationSuccessful();
		}
		else
		{
			if( receiptValidationFailed != null )
				receiptValidationFailed( "Receipt validation failed with statusCode: " + statusCode );
		}
	}

}

