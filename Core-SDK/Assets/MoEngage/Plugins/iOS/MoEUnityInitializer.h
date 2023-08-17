//
//  MoEUnityInitializer.h
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

@protocol SFSafariViewControllerDelegate;

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MoEngageSDK/MoEngageSDK.h>

@interface MoEUnityInitializer : NSObject
@property(assign, nonatomic, readonly) BOOL isSDKIntialized;

+(instancetype)sharedInstance;

- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions;
- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions withSDKState:(MoEngageSDKState)sdkState;
- (void)initializeSDKWithConfig:(MoEngageSDKConfig*)sdkConfig andLaunchOptions:(NSDictionary*)launchOptions;
- (void)initializeSDKWithConfig:(MoEngageSDKConfig*)sdkConfig withSDKState:(MoEngageSDKState)isSDKEnabled andLaunchOptions:(NSDictionary*)launchOptions;

- (void)setupSDKWithInitializePayload:(NSMutableDictionary*)payload;
@end
