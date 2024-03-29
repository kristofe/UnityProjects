using UnityEngine;
using System.Collections.Generic;


public class InAppPurchaseGUI : MonoBehaviour
{
	[System.NonSerialized]
	public bool showGUI = false;
	
#if UNITY_IPHONE
	private List<StoreKitProduct> _products;
	
	
	void Start()
	{
		// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
		// we will store the products locally so that we will know what is purchaseable and when we can purchase the products
		StoreKitManager.productListReceivedEvent += allProducts =>
		{
			Debug.Log( "received total products: " + allProducts.Count );
			_products = allProducts;
		};
	}


	void OnGUI()
	{
		if(showGUI == false)
			return;
		
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 960 || Screen.height >= 960 ) ? 320 : 160;
		float height = ( Screen.width >= 960 || Screen.height >= 960 ) ? 80 : 40;
		float heightPlus = height + 10.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Get Can Make Payments" ) )
		{
			bool canMakePayments = StoreKitBinding.canMakePayments();
			Debug.Log( "StoreKit canMakePayments: " + canMakePayments );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Product Data" ) )
		{
			// array of product ID's from iTunesConnect.  MUST match exactly what you have there!
			string[] productIdentifiers = new string[] { "com.blackicegamesnyc.remove_ads" };
			StoreKitBinding.requestProductData( productIdentifiers );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Restore Completed Transactions" ) )
		{
			StoreKitBinding.restoreCompletedTransactions();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Validate Receipt" ) )
		{
			// grab the transactions, then just validate the first one
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			if( transactionList.Count > 0 )
				StoreKitBinding.validateReceipt( transactionList[0].base64EncodedTransactionReceipt, true );
		}
		
		
		// enforce the fact that we can't purchase products until we retrieve the product data
		//if( _products != null && _products.Count > 0 )
		{
			if( GUI.Button( new Rect( xPos, yPos, width, height ), "Purchase Product" ) )
			{				
				Debug.Log( "preparing to purchase product: " + "com.blackicegamesnyc.remove_ads" );
				StoreKitBinding.purchaseProduct( "com.blackicegamesnyc.remove_ads", 1 );
			}
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Validate Subscription" ) )
		{
			// grab the transactions and if we have a subscription in there validate it
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			foreach(StoreKitTransaction t in transactionList )
			{
				if( t.productIdentifier == "sevenDays" )
				{
					StoreKitBinding.validateAutoRenewableReceipt( t.base64EncodedTransactionReceipt, "YOUR_SECRET_FROM_ITC", true );
					break;
				}
			}
		}

		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Saved Transactions" ) )
		{
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			
			// Print all the transactions to the console
			Debug.Log( "\ntotal transaction received: " + transactionList.Count );
			
			foreach( StoreKitTransaction transaction in transactionList )
				Debug.Log( transaction.ToString() + "\n" );
		}
		
	}
#endif
}
