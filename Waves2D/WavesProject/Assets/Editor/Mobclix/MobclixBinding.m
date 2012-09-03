//
//  MobClixBinding.m
//  MobClixTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "MobclixBinding.h"
#import "Mobclix.h"
#import "MobclixManager.h"


// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]


void _mobclixStart()
{
	[[MobclixManager sharedManager] start];
}


void _mobclixSetRefreshTime( float refreshTime )
{
	[[MobclixManager sharedManager] setRefreshTime:refreshTime];
}


void _mobclixShowBanner( bool bannerIsOnBottom )
{
	[[MobclixManager sharedManager] setBannerIsOnBottom:bannerIsOnBottom];
	[[MobclixManager sharedManager] showBanner];
}


void _mobclixHideBanner()
{
	[[MobclixManager sharedManager] hideBanner];
}


void _mobclixRotateToOrientation( int orientation )
{
	UIInterfaceOrientation screenOrientation;

	if( orientation == 1 )
		screenOrientation = UIInterfaceOrientationPortrait;
	else if( orientation == 2 )
		screenOrientation = UIInterfaceOrientationPortraitUpsideDown;
	else if( orientation == 3 )
		screenOrientation = UIInterfaceOrientationLandscapeRight;
	else if( orientation == 4 )
		screenOrientation = UIInterfaceOrientationLandscapeLeft;
	
	[[MobclixManager sharedManager] rotateToOrientation:screenOrientation];
}


void _mobclixLogEvent( int logLevel, const char * processName, const char * eventName, const char * description, bool stopProcess )
{
	MobclixLogLevel level;
	
	if( logLevel == 0 )
		level = LOG_LEVEL_DEBUG;
	else if( logLevel == 1 )
		level = LOG_LEVEL_INFO;
	else if( logLevel == 2 )
		level = LOG_LEVEL_WARN;
	else if( logLevel == 3 )
		level = LOG_LEVEL_ERROR;
	else if( logLevel == 4 )
		level = LOG_LEVEL_FATAL;
	
	[Mobclix logEventWithLevel:level
				   processName:GetStringParam( processName )
					 eventName:GetStringParam( eventName )
				   description:GetStringParam( description )
						  stop:stopProcess];
}

