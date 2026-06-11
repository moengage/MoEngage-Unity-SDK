
//
//  MoEUnityAppController.mm
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import "UnityAppController.h"
#import "AppDelegateListener.h"
#import "MoEUnityInitializer.h"
#import "MoEngageUnityUtils.h"

@interface MoEUnityAppController : UnityAppController

@end

@implementation MoEUnityAppController

- (instancetype)init
{
    self = [super init];
    return self;
}

# pragma mark - UIApplicationDelegate methods

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    [super application:application didFinishLaunchingWithOptions:launchOptions];

    NSDictionary *moeConfig = [MoEngageUnityUtils fetchInfoPlistConfig];
    if (moeConfig != nil) {
        NSNumber *autoInitVal = moeConfig[@"IsSdkAutoInitialisationEnabled"];
        BOOL isSdkAutoInitEnabled = (autoInitVal == nil) ? YES : [autoInitVal boolValue];
        if (isSdkAutoInitEnabled) {
            [[MoEUnityInitializer sharedInstance] setupBridgeForAutoInit];
        }
    }
    return YES;
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(MoEUnityAppController)


