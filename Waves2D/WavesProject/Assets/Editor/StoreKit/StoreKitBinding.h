//
//  StoreKitBinding.h
//  StoreKit
//
//  Created by Mike DeSaro on 8/18/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>


bool _canMakePayments();

void _requestProductData( const char *productIdentifier );

void _purchaseProduct( const char *product, int quantity );

void _restoreCompletedTransactions();

void _validateReceipt( const char *base64EncodedTransactionReceipt, bool isTest );

const char * _getAllSavedTransactions();
