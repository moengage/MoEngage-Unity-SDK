//
//  MoEUnityBridgeHelper.m
//  MoEngage
//

#import "MoEUnityBridgeHelper.h"
#import "MoEUnityInitializer.h"
#import "MoEUnityConstants.h"
#import <UserNotifications/UserNotifications.h>
#import <MoEngageSDK/MoEngageSDK.h>
@import MoEngagePluginBase;

@implementation MoEUnityBridgeHelper

+ (void)initialize:(NSDictionary *)payload {
    [[MoEUnityInitializer sharedInstance] setupSDKWithInitializePayload:(NSMutableDictionary *)payload];
}

+ (void)setAppStatus:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] setAppStatus:(NSMutableDictionary *)payload];
}

+ (void)setUserAttribute:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] setUserAttribute:(NSMutableDictionary *)payload];
}

+ (void)setAlias:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] setAlias:(NSMutableDictionary *)payload];
}

+ (void)trackEvent:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] trackEvent:(NSMutableDictionary *)payload];
}

+ (void)registerForPush {
    [[MoEngagePluginBridge sharedInstance] registerForPush];
}

+ (void)showInApp:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] showInApp:(NSMutableDictionary *)payload];
}

+ (void)setInAppContexts:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] setInAppContext:(NSMutableDictionary *)payload];
}

+ (void)invalidateInAppContexts:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] resetInAppContext:(NSMutableDictionary *)payload];
}

+ (void)getSelfHandledInApp:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] getSelfHandledInApp:(NSMutableDictionary *)payload];
}

+ (void)getSelfHandledInApps:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] getSelfHandledInApps:(NSMutableDictionary *)payload completionBlock:^(NSDictionary<NSString *,id> * _Nonnull result) {
        [[MoEUnityInitializer sharedInstance] sendCallbackToUnityForMethod:kUnityMethodNameInAppSelfHandledCampaigns withMessage:result];
    }];
}

+ (void)showNudge:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] showNudge:(NSMutableDictionary *)payload];
}

+ (void)identifyUser:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] identifyUser:(NSMutableDictionary *)payload];
}

+ (void)getUserIdentities:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] getUserIdentities:(NSMutableDictionary *)payload completionHandler:^(NSDictionary<NSString *,NSString *> * _Nonnull identities) {
        [[MoEUnityInitializer sharedInstance] sendCallbackToUnityForMethod:kUnityMethodNameUserIdentities withMessage:identities];
    }];
}

+ (void)registerForProvisionalPush {
    [[MoEngagePluginBridge sharedInstance] registerForProvisionalPush];
}

+ (void)updateSelfHandledInAppStatus:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] updateSelfHandledImpression:(NSMutableDictionary *)payload];
}

+ (void)optOutGDPRTracking:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] optOutDataTracking:(NSMutableDictionary *)payload];
}

+ (void)resetUser:(NSDictionary *)payload {
    [[MoEngagePluginBridge sharedInstance] resetUser:(NSMutableDictionary *)payload];
}

+ (void)updateSdkState:(NSDictionary *)payload {
    if (payload) {
        [[MoEngagePluginBridge sharedInstance] updateSDKState:(NSMutableDictionary *)payload];
    }
}

@end
