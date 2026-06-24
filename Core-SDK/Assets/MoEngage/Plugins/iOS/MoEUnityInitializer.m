//
//  MoEUnityInitializer.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityInitializer.h"
#import "MoEUnityConstants.h"
#import <UserNotifications/UserNotifications.h>
#import <MoEngageSDK/MoEngageSDK.h>
#import "MoEngageUnityUtils.h"
@import MoEngagePluginBase;

@interface MoEUnityInitializer() <MoEngagePluginBridgeDelegate>
@property(assign, nonatomic) BOOL isSDKIntialized;
@property(nonatomic, strong) NSString* moeGameObjectName;

@end

@implementation MoEUnityInitializer

#pragma mark- Initialization

+(instancetype)sharedInstance{
    static dispatch_once_t onceToken;
    static MoEUnityInitializer *instance;
    dispatch_once(&onceToken, ^{
        instance = [[MoEUnityInitializer alloc] init];
    });
    return instance;
}

- (instancetype)init
{
    self = [super init];
    if (self) {
        self.isSDKIntialized = NO;
    }
    return self;
}

// Called when IsSdkAutoInitialisationEnabled=true — the SDK has already auto-initialized
// from Info.plist. We still call initializeDefaultInstanceWithAdditionalConfig: (via
// setupSDKFromInfoPlistWithLaunchOptions:) because MoEngagePlugin.process(.init) is what
// registers the InApp/Messaging/Auth delegate handlers with the SDK. Without it those
// handlers are never wired up and MoEngagePluginBridge's delegate is never reached.
// The SDK itself is idempotent and will not reinitialize if already initialized.
- (void)setupBridgeForAutoInit {
    self.isSDKIntialized = YES;
    [self setupSDKFromInfoPlistWithLaunchOptions:nil sdkState:nil];
}

- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions {
    self.isSDKIntialized = YES;
    [self setupSDKFromInfoPlistWithLaunchOptions:launchOptions sdkState:nil];
}

- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions withSDKState:(MoEngageSDKState)sdkState {
    self.isSDKIntialized = YES;
    [self setupSDKFromInfoPlistWithLaunchOptions:launchOptions sdkState:@(sdkState)];
}

- (void)initializeSDKWithConfig:(MoEngageSDKConfig*)sdkConfig andLaunchOptions:(NSDictionary*)launchOptions{
    self.isSDKIntialized = YES;
    [self setupSDKWithConfig:sdkConfig andLaunchOptions:launchOptions];
}

- (void)initializeSDKWithConfig:(MoEngageSDKConfig*)sdkConfig withSDKState:(MoEngageSDKState)sdkState andLaunchOptions:(NSDictionary*)launchOptions{
    self.isSDKIntialized = YES;
    [self setupSDKWithConfig:sdkConfig withSDKState:sdkState andLaunchOptions:launchOptions];
}

- (void)setupSDKWithInitializePayload:(NSMutableDictionary*)payload {
    NSString* gameObjectName = payload[@"data"][@"gameObjectName"];
    self.moeGameObjectName = gameObjectName;
    if (!self.isSDKIntialized) {
        // Fallback: client did not call initializeSDK* before Unity scene loaded.
        // Attempt file-based init; this will be a no-op if IsSdkAutoInitialisationEnabled=true
        // because the SDK already initialized itself, but commonSetUp still won't have run.
        [self setupSDKFromInfoPlistWithLaunchOptions:nil sdkState:nil];
    }
    [[MoEngagePluginBridge sharedInstance] pluginInitialized:payload];
}

-(void)setupSDKFromInfoPlistWithLaunchOptions:(NSDictionary* _Nullable)launchOptions sdkState:(NSNumber* _Nullable)sdkState {
    MoEngagePlugin *plugin = [[MoEngagePlugin alloc] init];
    MoEngageSDKDefaultInitializationConfig *config = [[MoEngageSDKDefaultInitializationConfig alloc] init];
    config.launchOptions = launchOptions;
    MoEngageSDKConfig *resolvedConfig = [plugin initializeDefaultInstanceWithAdditionalConfig:config];
    if (resolvedConfig != nil) {
        [self commonSetUp:plugin andSDKConfig:resolvedConfig];
    }
}

-(void)setupSDKWithConfig:(MoEngageSDKConfig*)sdkConfig withSDKState:(MoEngageSDKState)sdkState andLaunchOptions:(NSDictionary * _Nullable)launchOptions {
    if (sdkConfig.appId && sdkConfig.appId.length > 0) {
        MoEngagePlugin *plugin = [[MoEngagePlugin alloc] init];
        [plugin initializeInstanceWithSdkConfig:sdkConfig sdkState:sdkState launchOptions:launchOptions];
        [self commonSetUp:plugin andSDKConfig:sdkConfig];
    }
    else{
        NSAssert(NO, @"MoEngage - Provide the APP ID in MoEngage-Infos.plist under the WorkspaceId key.");
    }
}

-(void)setupSDKWithConfig:(MoEngageSDKConfig*)sdkConfig andLaunchOptions:(NSDictionary * _Nullable)launchOptions {
    if (sdkConfig.appId && sdkConfig.appId.length > 0) {
        MoEngagePlugin *plugin = [[MoEngagePlugin alloc] init];
        [plugin initializeDefaultInstanceWithSdkConfig:sdkConfig launchOptions:launchOptions];
        [self commonSetUp:plugin andSDKConfig:sdkConfig];
    }
    else{
        NSAssert(NO, @"MoEngage - Provide the APP ID in MoEngage-Infos.plist under the WorkspaceId key.");
    }
}

-(void)commonSetUp:(MoEngagePlugin*)plugin andSDKConfig:(MoEngageSDKConfig*)config {
    [[MoEngagePluginBridge sharedInstance] setPluginBridgeDelegate:self identifier:config.appId];
}

#pragma mark- Native to Unity Callbacks

-(void)sendCallbackToUnityForMethod:(NSString *)method withMessage:(NSDictionary *)messageDict {
    if (self.moeGameObjectName == nil) {
        NSLog(@"MoEngage: sendCallbackToUnityForMethod: dropped — game object name not set. method=%@", method);
        return;
    }
    NSString* message = [MoEngageUnityUtils dictToJson:messageDict];
    if (message == nil) {
        NSLog(@"MoEngage: sendCallbackToUnityForMethod: dropped — failed to serialize payload for method=%@", method);
        return;
    }
    UnitySendMessage([self.moeGameObjectName UTF8String], [method UTF8String], [message UTF8String]);
}

#pragma mark- MoEPluginBridgeDelegate Callbacks

- (void)sendMessageWithEvent:(NSString *)event message:(NSDictionary<NSString *,id> *)message {
    NSString* unityMethodName = nil;

    if ([event isEqualToString:kPushTokenGenerated]){
        unityMethodName = kUnityMethodNamePushTokenGenerated;
    }
    else if ([event isEqualToString:kPushClicked]) {
        unityMethodName = kUnityMethodNamePushClicked;
    }
    if ([event isEqualToString:kInAppShown]){
        unityMethodName = kUnityMethodNameInAppShown;
    }
    else if ([event isEqualToString:kInAppSelfHandled]){
        unityMethodName = kUnityMethodNameInAppSelfHandled;
    }
    else if ([event isEqualToString:kInAppClicked]){
        unityMethodName = kUnityMethodNameInAppClicked;
    }
    else if ([event isEqualToString:kInAppDismissed]){
        unityMethodName = kUnityMethodNameInAppDismissed;
    }
    else if ([event isEqualToString:kInAppCustomAction]){
        unityMethodName = kUnityMethodNameInAppCustomAction;
    }
    else if ([event isEqualToString:kLogoutComplete]){
        unityMethodName = kUnityMethodNameLogoutComplete;
    }
    if(unityMethodName != nil){
        [self sendCallbackToUnityForMethod:unityMethodName withMessage:message];
    }
}

@end
