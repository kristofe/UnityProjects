//
//  MobClixManager.m
//  MobClixTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "MobclixManager.h"
#import "Mobclix.h"


void UnityPause( bool pause );


@implementation MobclixManager

@synthesize adView = _adView, controller = _controller;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (MobclixManager*)sharedManager
{
	static MobclixManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[MobclixManager alloc] init];
		
		return sharedSingleton;
}


- (id)init
{
	if( self = [super init] )
	{
		// sensible defaults
		_adBannerOnBottom = YES;
		
		// ad the proper adView based on the device
		if( ( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad ) )
			_adView = [[MobclixAdViewiPad_468x60 alloc] initWithFrame:CGRectMake( 0, 0, 468, 60 )];
		else
			_adView = [[MobclixAdViewiPhone_320x50 alloc] initWithFrame:CGRectMake( 0, 0, 320, 50 )];
		_adView.delegate = self;
		_adViewWidth = _adView.frame.size.width;
		_adViewHeight = _adView.frame.size.height;
		
		// Inject a view controller
		_controller = [[UIViewController alloc] initWithNibName:nil bundle:nil];
		[_controller.view addSubview:_adView];
		
		[[UIApplication sharedApplication].keyWindow addSubview:_controller.view];
		[[UIApplication sharedApplication].keyWindow bringSubviewToFront:_controller.view];
		_controller.view.frame = CGRectZero;
		
		_isLandscape = UIInterfaceOrientationIsLandscape( [UIApplication sharedApplication].statusBarOrientation );
		_isLandscapeLeft = [UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationLandscapeLeft;
		_isPortraitUpsideDown = [UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortraitUpsideDown;
	}
	return self;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark MobclixAdViewDelegate

- (void)adViewWillTouchThrough:(MobclixAdView*)adView
{
	UnityPause( true );
	_controller.view.alpha = 0;
}


- (void)adViewDidFinishTouchThrough:(MobclixAdView*)adView
{
	UnityPause( false );
	
	// readjust our frame before showing ourself
	[self setBannerIsOnBottom:_adBannerOnBottom];
	_controller.view.alpha = 1.0;
}


/* If you wish to use AdMob, enter your AdMob publisher key below and you will need to manually
   include the AdMob library and framework dependencies.  Also be sure to uncomment this method.
- (NSString*)adView:(MobclixAdView*)adView publisherKeyForSuballocationRequest:(MCAdsSuballocationType)suballocationType
{
	// AdMob integration
	if( suballocationType == kMCAdsSuballocationAdMob )
		return @"YOUR ADMOB PUBLISHER KEY";
	return nil;
}
*/


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)start
{
	[Mobclix start];
}


- (void)setRefreshTime:(CGFloat)refreshTime
{
	_adView.refreshTime = refreshTime;
}


- (void)setBannerIsOnBottom:(BOOL)isBottom
{
	_adBannerOnBottom = isBottom;
	
	// when we are landscape right, the view is totally flipped so we calculate everything opposite
	BOOL calculateForBannerOnBottom = ( _isLandscape && !_isLandscapeLeft ) ? !_adBannerOnBottom : _adBannerOnBottom;
	
	if( !_isLandscape && _isPortraitUpsideDown )
		calculateForBannerOnBottom = !_adBannerOnBottom;
	
	// Tell the adView what we want to see and position the adView
	CGRect frame = CGRectZero;
	
	CGFloat _screenWidth = [UIScreen mainScreen].bounds.size.width;
	CGFloat _screenHeight = [UIScreen mainScreen].bounds.size.height;

	if( _isLandscape )
	{
		frame = CGRectMake( 0, 0, _adViewHeight, _adViewWidth );
		
		if( _isLandscapeLeft )
			_controller.view.transform = CGAffineTransformMake( 0, -1, 1, 0, 0, 0 );
		else
			_controller.view.transform = CGAffineTransformMake( 0, 1, -1, 0, 0, 0 );
		
		// center the view because it is only _adViewWidth px accross
		frame.origin.y = ( _screenHeight - frame.size.height ) / 2;

		if( calculateForBannerOnBottom )
			frame.origin.x = _screenWidth - _adViewHeight;
	}
	else
	{
		frame = CGRectMake( 0, 0, _adViewWidth, _adViewHeight );
		
		if( _isPortraitUpsideDown )
			_controller.view.transform = CGAffineTransformMakeRotation( M_PI );
		else
			_controller.view.transform = CGAffineTransformIdentity;
		
		if( calculateForBannerOnBottom )
			frame.origin.y = _screenHeight - _adViewHeight;
		else
			frame.origin.y = 0;
	}
	
	_controller.view.frame = frame;
}


- (void)showBanner
{
	_controller.view.hidden = NO;
	[_adView resumeAdAutoRefresh];
}


- (void)hideBanner
{
	_controller.view.hidden = YES;
	[_adView pauseAdAutoRefresh];
}


- (void)rotateToOrientation:(UIInterfaceOrientation)orientation
{
	// set the appropriate ivars
	_isLandscape = UIInterfaceOrientationIsLandscape( orientation );
	_isLandscapeLeft = ( orientation == UIInterfaceOrientationLandscapeLeft );
	_isPortraitUpsideDown = ( orientation == UIInterfaceOrientationPortraitUpsideDown );
	
	// adjust requested ad types and relayout the adView
	[self setBannerIsOnBottom:_adBannerOnBottom];
	
	if( !_controller.view.hidden )
		[self showBanner];
}


@end
