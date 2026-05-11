//
//  MoEUnityInitializer.h
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.

#import <Foundation/Foundation.h>

@protocol SFSafariViewControllerDelegate;

// Forward-declare MoEngage SDK types used in method signatures so callers
// don't need to import MoEngageSDK themselves.
@class MoEngageSDKConfig;
typedef NS_ENUM(NSInteger, MoEngageSDKState);

@interface MoEUnityInitializer : NSObject
@property(assign, nonatomic, readonly) BOOL isSDKIntialized;

+(instancetype)sharedInstance;

- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions;
- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions withSDKState:(MoEngageSDKState)sdkState;
- (void)initializeSDKWithConfig:(MoEngageSDKConfig*)sdkConfig andLaunchOptions:(NSDictionary*)launchOptions;
- (void)initializeSDKWithConfig:(MoEngageSDKConfig*)sdkConfig withSDKState:(MoEngageSDKState)isSDKEnabled andLaunchOptions:(NSDictionary*)launchOptions;

- (void)setupSDKWithInitializePayload:(NSMutableDictionary*)payload;
- (void)sendCallbackToUnityForMethod:(NSString *)method withMessage:(NSDictionary *)messageDict;
@end
