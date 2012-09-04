//
//  AdManager.h
//  iAd
//
//  Created by Mike on 8/18/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <iAd/iAd.h>


@interface AdManager : NSObject <ADBannerViewDelegate>
{
	ADBannerView *_adView;
	UIViewController *_controller;
	BOOL _fireHideShowEvents;
	
	UIInterfaceOrientation _orientation;
	BOOL _adBannerOnBottom;
	BOOL _bannerIsVisible;
}
@property (nonatomic, retain) UIViewController *controller;
@property (nonatomic, retain) ADBannerView *adView;
@property (nonatomic, assign) BOOL fireHideShowEvents;


+ (AdManager*)sharedManager;

- (void)createAdBanner;

- (void)destroyAdBanner;

- (void)setBannerIsOnBottom:(BOOL)isBottom;

- (void)rotateToOrientation:(UIInterfaceOrientation)orientation;


@end
