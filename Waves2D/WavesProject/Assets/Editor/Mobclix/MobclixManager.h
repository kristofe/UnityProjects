//
//  MobClixManager.h
//  MobClixTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "MobclixAds.h"


@interface MobclixManager : NSObject <MobclixAdViewDelegate>
{
	MobclixAdView *_adView;
	UIViewController *_controller;
	BOOL _isLandscapeLeft;
	BOOL _isPortraitUpsideDown;
	
	CGFloat _adViewWidth;
	CGFloat _adViewHeight;
	BOOL _isLandscape;
	BOOL _adBannerOnBottom;
}
@property (nonatomic, retain) UIViewController *controller;
@property (nonatomic, retain) MobclixAdView *adView;


+ (MobclixManager*)sharedManager;


- (void)start;

- (void)setRefreshTime:(CGFloat)refreshTime;

- (void)setBannerIsOnBottom:(BOOL)isBottom;

- (void)showBanner;

- (void)hideBanner;

- (void)rotateToOrientation:(UIInterfaceOrientation)orientation;

@end
