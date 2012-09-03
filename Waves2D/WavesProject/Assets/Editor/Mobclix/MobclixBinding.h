//
//  MobClixBinding.h
//  MobClixTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>


void _mobclixStart();


void _mobclixSetRefreshTime( float refreshTime );


void _mobclixShowBanner( bool bannerIsOnBottom );


void _mobclixHideBanner();


void _mobclixRotateToOrientation( int orientation );


void _mobclixLogEvent( int logLevel, const char * processName, const char * eventName, const char * description, bool stopProcess );

