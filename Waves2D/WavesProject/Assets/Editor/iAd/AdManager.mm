//
//  AdManager.m
//  iAd
//
//  Created by Mike on 8/18/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "AdManager.h"
#import "AdViewController.h"


void UnityPause( bool pause );

void UnitySendMessage( const char * className, const char * methodName, const char * param );


@interface AdManager(Private)
- (void)adjustRequestedAdTypesBasedOnOrientation;
- (void)adjustAdViewFrameToShowAdView:(BOOL)animated;
@end



@implementation AdManager

@synthesize adView = _adView, controller = _controller, fireHideShowEvents = _fireHideShowEvents;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (AdManager*)sharedManager
{
	static AdManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[AdManager alloc] init];
	
	return sharedSingleton;
}


- (id)init
{
	// early out if we dont have iOS 4.0 iAd.framework
	if( !NSClassFromString( @"ADBannerView" ) )
		return nil;
	
	if( self = [super init] )
	{
		// sensible defaults
		_adBannerOnBottom = YES;
		
		// grab our orientation
		_orientation = [UIApplication sharedApplication].statusBarOrientation;
	}
	return self;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (void)adjustRequestedAdTypesBasedOnOrientation
{
	// set the contentSize and requiredContentSize so the adView knows what it can display
	if( UIInterfaceOrientationIsLandscape( _orientation ) )
	{
		_adView.requiredContentSizeIdentifiers = [NSSet setWithObject:ADBannerContentSizeIdentifier480x32];
		_adView.currentContentSizeIdentifier = ADBannerContentSizeIdentifier480x32;
	}
	else
	{
		_adView.requiredContentSizeIdentifiers = [NSSet setWithObject:ADBannerContentSizeIdentifier320x50];
		_adView.currentContentSizeIdentifier = ADBannerContentSizeIdentifier320x50;
	}
}


- (void)adjustAdViewFrameToShowAdView:(BOOL)animated
{
	if( animated )
		[UIView beginAnimations:nil context:nil];
	
	BOOL isLandscape = UIInterfaceOrientationIsLandscape( _orientation );
	BOOL isLandscapeLeft = _orientation == UIInterfaceOrientationLandscapeLeft;
	BOOL isPortraitUpsideDown = _orientation == UIInterfaceOrientationPortraitUpsideDown;
	
	// when we are landscape right or portraitUpsideDown, the view is totally flipped so we calculate everything opposite
	BOOL calculateForBannerOnBottom = ( isLandscape && !isLandscapeLeft || !isLandscape && isPortraitUpsideDown ) ? !_adBannerOnBottom : _adBannerOnBottom;
	
	// if landscape, everything is reversed (width for height) due to the transform
	if( isLandscape )
	{
		CGFloat xOffset = ( calculateForBannerOnBottom ) ? -_controller.view.frame.size.width : _controller.view.frame.size.width;
		
		// if the banner isn't visible, reverse the offset animation
		if( !_bannerIsVisible )
			xOffset *= -1;
		
		_controller.view.frame = CGRectOffset( _controller.view.frame, xOffset, 0 );
	}
	else
	{
		CGFloat yOffset = ( calculateForBannerOnBottom ) ? -_controller.view.frame.size.height : _controller.view.frame.size.height;
		
		// if the banner isn't visible, reverse the offset animation
		if( !_bannerIsVisible )
			yOffset *= -1;
		
		_controller.view.frame = CGRectOffset( _controller.view.frame, 0, yOffset );
	}
	
	if( animated )
		[UIView commitAnimations];
	
	// Bring the ad back into view in case we were viewing an ad
	_controller.view.hidden = NO;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)createAdBanner
{
	// if we have an adView dont create one
	if( _adView )
		return;
	
	_adView = [[ADBannerView alloc] initWithFrame:CGRectZero];
	_adView.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
	_adView.delegate = self;
	
	[self adjustRequestedAdTypesBasedOnOrientation];
	
	// Inject a view controller
	_controller = [[AdViewController alloc] initWithNibName:nil bundle:nil];
	_controller.view.userInteractionEnabled = YES;
	[_controller.view addSubview:_adView];
	
	[[UIApplication sharedApplication].keyWindow addSubview:_controller.view];
	[[UIApplication sharedApplication].keyWindow bringSubviewToFront:_controller.view];
}


- (void)destroyAdBanner
{
	// destroy the adView
	_adView.delegate = nil;
	[_adView removeFromSuperview];
	[_adView release];
	_adView = nil;
	
	// kill the view controller
	[_controller.view removeFromSuperview];
	[_controller release];
	_controller = nil;
	
	_bannerIsVisible = NO;
}


- (void)setBannerIsOnBottom:(BOOL)isBottom
{
	_adBannerOnBottom = isBottom;
	
	BOOL isLandscape = UIInterfaceOrientationIsLandscape( _orientation );
	BOOL isLandscapeLeft = _orientation == UIInterfaceOrientationLandscapeLeft;
	BOOL isPortraitUpsideDown = _orientation == UIInterfaceOrientationPortraitUpsideDown;
	
	// when we are landscape right or portraitUpsideDown, the view is totally flipped so we calculate everything opposite
	BOOL calculateForBannerOnBottom = ( isLandscape && !isLandscapeLeft || !isLandscape && isPortraitUpsideDown ) ? !_adBannerOnBottom : _adBannerOnBottom;
	
	// Tell the adView what we want to see and position the adView
	CGRect frame = CGRectZero;
	
	if( isLandscape )
	{
		frame = CGRectMake( 0, 0, 32, 480 );
		
		if( _orientation == UIInterfaceOrientationLandscapeLeft )
			_controller.view.transform = CGAffineTransformMake( 0, -1, 1, 0, 0, 0 );
		else
			_controller.view.transform = CGAffineTransformMake( 0, 1, -1, 0, 0, 0 );
		
		frame.origin.y = 0;
		
		if( calculateForBannerOnBottom )
			frame.origin.x = 320; // full screen width minus adView height and move off the screen
		else
			frame.origin.x -= 32; // just move off the screen the width of the banner
	}
	else
	{
		frame = CGRectMake( 0, 0, 320, 50 );
		
		// force the identity transform for portrait
		if( _orientation == UIInterfaceOrientationPortrait )
			_controller.view.transform = CGAffineTransformIdentity;
		else
			_controller.view.transform = CGAffineTransformMake( -1, 0, 0, -1, 0, 0 );

		if( calculateForBannerOnBottom )
			frame.origin.y = 480;
		else
			frame.origin.y = -50; // just move off the screen the height of the banner
	}
	
	// Set the controller frame and the _adView frame
	_controller.view.frame = frame;
	_adView.frame = ( isLandscape ) ? CGRectMake( 0, 0, 480, 32 ) : CGRectMake( 0, 0, 320, 50 );
}


- (void)rotateToOrientation:(UIInterfaceOrientation)orientation
{
	// set the appropriate ivars
	_orientation = orientation;
	
	// adjust requested ad types and relayout the adView
	[self adjustRequestedAdTypesBasedOnOrientation];
	[self setBannerIsOnBottom:_adBannerOnBottom];
	
	if( _bannerIsVisible )
		[self adjustAdViewFrameToShowAdView:NO];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark ADBannerViewDelegate

- (void)bannerView:(ADBannerView*)banner didFailToReceiveAdWithError:(NSError*)error
{
	NSLog( @"------ bannerView:didFailToReceiveAdWithError: %@", [error localizedDescription] );
	if( _bannerIsVisible )
    {
		_bannerIsVisible = NO;
		[self adjustAdViewFrameToShowAdView:YES];
		
		// fire the event if we want it
		if( _fireHideShowEvents )
			UnitySendMessage( "AdManager", "adViewDidShow", "0" );
    }
}


- (void)bannerViewDidLoadAd:(ADBannerView*)banner
{
    if( !_bannerIsVisible )
    {
		_bannerIsVisible = YES;
		[self adjustAdViewFrameToShowAdView:YES];

		// fire the event if we want it
		if( _fireHideShowEvents )
			UnitySendMessage( "AdManager", "adViewDidShow", "1" );
    }
}


- (BOOL)bannerViewActionShouldBegin:(ADBannerView*)banner willLeaveApplication:(BOOL)willLeave
{
	// Hide the ad while they view it to avoid having it jump position.  Landscape only due to the way Unity does things
	_controller.view.alpha = 0.0f;
	
	UnityPause( true );
	return YES;
}


- (void)bannerViewActionDidFinish:(ADBannerView*)banner
{
	[self setBannerIsOnBottom:_adBannerOnBottom];
	[self adjustAdViewFrameToShowAdView:NO];
	
	// reshow our view
	_controller.view.alpha = 1.0f;
	
	UnityPause( false );
}

@end
