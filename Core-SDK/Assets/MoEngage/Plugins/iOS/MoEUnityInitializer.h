//
//  MoEUnityInitializer.h
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MoEngage/MoEngage.h>

@interface MoEUnityInitializer : NSObject
@property(assign, nonatomic, readonly) BOOL isSDKIntialized;

+(instancetype)sharedInstance;
+(BOOL)isUnityAppControllerSwizzlingEnabled;

- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions;
- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions andSDKState:(BOOL)isSDKEnabled;

- (void)intializeSDKWithConfig:(MOSDKConfig*)sdkConfig andLaunchOptions:(NSDictionary*)launchOptions;
- (void)intializeSDKWithConfig:(MOSDKConfig*)sdkConfig withSDKState:(BOOL)isSDKEnabled andLaunchOptions:(NSDictionary*)launchOptions;

- (void)setupSDKWithGameObject:(NSString*)gameObjectName;

@end
