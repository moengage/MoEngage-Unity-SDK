
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

    BOOL swizzleUnityAppController = [MoEngageUnityUtils isUnityAppControllerSwizzlingEnabled];
    if (!swizzleUnityAppController) {
        NSLog(@"MoEUnityAppController SubClass application:didFinishLaunchingWithOptions: called");
        [[MoEUnityInitializer sharedInstance] initializeSDKWithLaunchOptions:launchOptions];
    }
    return YES;
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(MoEUnityAppController)


