//
//  MoEngageConfiguration.m
//  MoEngage
//
//  Created by Chengappa on 11/01/21.
//  Copyright © 2021 MoEngage. All rights reserved.
//

#import <objc/runtime.h>
#import "UnityAppController.h"
#import "MoEUnityInitializer.h"
#import "MoEngageUnityUtils.h"
#import <UserNotifications/UserNotifications.h>

@implementation UnityAppController (MoEngage)


+ (void)load {
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        
        BOOL swizzleUnityAppController = [MoEngageUnityUtils isUnityAppControllerSwizzlingEnabled];
        if(swizzleUnityAppController){
            Class class = [self class];
            
            SEL appDidFinishLaunching = @selector(application:didFinishLaunchingWithOptions:);
            SEL swizzledAppDidFinishLaunching = @selector(moengage_swizzled_application:didFinishLaunchingWithOptions:);
            [self swizzleMethodWithClass:class originalSelector:appDidFinishLaunching andSwizzledSelector:swizzledAppDidFinishLaunching];
        }
    });
}

#pragma mark- Swizzle Method

+ (void)swizzleMethodWithClass:(Class)class originalSelector:(SEL)originalSelector andSwizzledSelector:(SEL)swizzledSelector {
    Method originalMethod = class_getInstanceMethod(class, originalSelector);
    Method swizzledMethod = class_getInstanceMethod(class, swizzledSelector);
    
    BOOL didAddMethod =
    class_addMethod(class,
                    originalSelector,
                    method_getImplementation(swizzledMethod),
                    method_getTypeEncoding(swizzledMethod));
    
    if (didAddMethod) {
        class_replaceMethod(class,
                            swizzledSelector,
                            method_getImplementation(originalMethod),
                            method_getTypeEncoding(originalMethod));
    } else {
        method_exchangeImplementations(originalMethod, swizzledMethod);
    }
}

#pragma mark- Application LifeCycle methods

- (BOOL)moengage_swizzled_application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions{
    
    // Uncomment the below line if you have conflicting implementations in the project for obtaining the Push callbacks.
    // [UNUserNotificationCenter currentNotificationCenter].delegate = [MoEPluginInitializer sharedInstance];
    NSLog(@"MoEngageSwizzledAppController application:didFinishLaunchingWithOptions: called");
    [[MoEUnityInitializer sharedInstance] initializeSDKWithLaunchOptions:launchOptions];
    
    return [self moengage_swizzled_application:application didFinishLaunchingWithOptions:launchOptions];
}


@end
