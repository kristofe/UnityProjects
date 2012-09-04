//
//  AdBinding.m
//  iAd
//
//  Created by Mike on 8/18/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "AdManager.h"


void _iAdCreateAdBanner( bool bannerOnBottom )
{
	[[AdManager sharedManager] createAdBanner];
	[[AdManager sharedManager] setBannerIsOnBottom:bannerOnBottom];
}


void _iAdDestroyAdBanner()
{
	[[AdManager sharedManager] destroyAdBanner];
}


void _iAdRotateToOrientation( int orientation )
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
	
	[[AdManager sharedManager] rotateToOrientation:screenOrientation];
}


void _iAdFireHideShowEvents( bool shouldFire )
{
	[AdManager sharedManager].fireHideShowEvents = shouldFire;
}