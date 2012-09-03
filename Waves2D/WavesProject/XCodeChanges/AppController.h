#import <UIKit/UIKit.h>
#import "Mobclix.h"
#import "MobclixAds.h"

@interface AppController : NSObject<UIAccelerometerDelegate, UIApplicationDelegate,MobclixAdViewDelegate>
{
	UIWindow*			_window;
	UIWindow*            adView; 
	MMABannerXLAdView*      bannerAd; 
}
- (void) startUnity:(UIApplication*)application;
- (void) startRendering:(UIApplication*)application;
- (void) showAdViewMobclix; 
- (UIWindow*) getAdView;
@end

#define NSTIMER_BASED_LOOP 0
#define THREAD_BASED_LOOP 1
#define EVENT_PUMP_BASED_LOOP 2
