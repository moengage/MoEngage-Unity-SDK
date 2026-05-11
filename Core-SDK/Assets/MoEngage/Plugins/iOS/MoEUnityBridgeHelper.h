//
//  MoEUnityBridgeHelper.h
//  MoEngage
//


#import <Foundation/Foundation.h>

@interface MoEUnityBridgeHelper : NSObject

+ (void)initialize:(NSDictionary *)payload;
+ (void)setAppStatus:(NSDictionary *)payload;
+ (void)setUserAttribute:(NSDictionary *)payload;
+ (void)setAlias:(NSDictionary *)payload;
+ (void)trackEvent:(NSDictionary *)payload;
+ (void)registerForPush;
+ (void)showInApp:(NSDictionary *)payload;
+ (void)setInAppContexts:(NSDictionary *)payload;
+ (void)invalidateInAppContexts:(NSDictionary *)payload;
+ (void)getSelfHandledInApp:(NSDictionary *)payload;
+ (void)getSelfHandledInApps:(NSDictionary *)payload;
+ (void)updateSelfHandledInAppStatus:(NSDictionary *)payload;
+ (void)showNudge:(NSDictionary *)payload;
+ (void)identifyUser:(NSDictionary *)payload;
+ (void)getUserIdentities:(NSDictionary *)payload;
+ (void)registerForProvisionalPush;
+ (void)optOutGDPRTracking:(NSDictionary *)payload;
+ (void)resetUser:(NSDictionary *)payload;
+ (void)updateSdkState:(NSDictionary *)payload;

@end
