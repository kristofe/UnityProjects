using UnityEngine;
using System.Collections.Generic;


public class InAppPurchaseGUI : MonoBehaviour
{
	public bool showGUI = false;
	
	void OnGUI()
	{
		if(showGUI == false)
			return;
		
		float yPos = 40.0f;
		float xPos = 40.0f;
		float width = 210.0f;
		string productID = "com.blackicegamesnyc.remove_ads";
		
		if( GUI.Button( new Rect( 285, 5, 30, 30 ), "X" ) )
		{
			SendMessage("showIAPGUI",false,SendMessageOptions.DontRequireReceiver);
		}
		
		if( GUI.Button( new Rect( xPos, yPos, width, 40 ), "Get Can Make Payments" ) )
		{
			bool canMakePayments = StoreKitBinding.canMakePayments();
			Debug.Log( "StoreKit canMakePayments: " + canMakePayments );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Get Product Data" ) )
		{
			// comma delimited list of product ID's from iTunesConnect.  MUST match exactly what you have there!
			//string productIdentifiers = "remove_ads";
			StoreKitBinding.requestProductData( productID );
			
			
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Restore Completed Transactions" ) )
		{
			StoreKitBinding.restoreCompletedTransactions();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Validate Receipt" ) )
		{
			// grab the transactions, then just validate the first one
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			if( transactionList.Count > 0 )
				StoreKitBinding.validateReceipt( transactionList[0].base64EncodedTransactionReceipt, true );
		}
		
		// Second column
		//xPos += xPos + width;
		//yPos = 10.0f;
		if( GUI.Button( new Rect( xPos, yPos, width, 40 ), "Purchase Product 1" ) )
		{
			StoreKitBinding.purchaseProduct( productID, 1 );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Purchase Product 2" ) )
		{
			StoreKitBinding.purchaseProduct( "anotherProduct", 1 );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += 50, width, 40 ), "Get Saved Transactions" ) )
		{
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			
			// Print all the transactions to the console
			Debug.Log( "\ntotal transaction received: " + transactionList.Count );
			
			foreach( StoreKitTransaction transaction in transactionList )
				Debug.Log( transaction.ToString() + "\n" );
		}
		
	}
}
