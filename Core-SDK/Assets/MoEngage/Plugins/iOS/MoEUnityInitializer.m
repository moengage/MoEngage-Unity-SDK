//
//  MoEUnityInitializer.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityInitializer.h"
#import "MoEngageConfiguration.h"
#import "MoEUnityConstants.h"
#import <MoEngageSDK/MoEngageSDK.h>
#import <MoEngagePluginBase/MoEngagePluginBase-Swift.h>
#import "MoEngageUnityUtils.h"

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

- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions {
    MoEngageSDKConfig* sdkConfig = [self getSDKConfigFromFile];
    [self initializeSDKWithConfig:sdkConfig andLaunchOptions:launchOptions];
}

- (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions withSDKState:(MoEngageSDKState)sdkState {
    MoEngageSDKConfig* sdkConfig = [self getSDKConfigFromFile];
    [self initializeSDKWithConfig:sdkConfig withSDKState:sdkState andLaunchOptions:launchOptions];
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
        //this will works as fallback method if AppDelegate Swizzling doesn't work
        MoEngageSDKConfig* sdkConfig = [self getSDKConfigFromFile];
        [self setupSDKWithConfig:sdkConfig andLaunchOptions:nil];
    }
    [[MoEngagePluginBridge sharedInstance]pluginInitialized:payload];
}

-(void)setupSDKWithConfig:(MoEngageSDKConfig*)sdkConfig withSDKState:(MoEngageSDKState)sdkState andLaunchOptions:(NSDictionary * _Nullable)launchOptions {
    if (sdkConfig.appId && sdkConfig.appId.length > 0) {
        MoEngagePlugin *plugin = [[MoEngagePlugin alloc] init];
        [plugin initializeInstanceWithSdkConfig:sdkConfig sdkState:sdkState launchOptions:launchOptions];
        [self commonSetUp:plugin andSDKConfig:sdkConfig];
    }
    else{
        NSAssert(NO, @"MoEngage - Provide the APP ID for your MoEngage App in MoEngageConfiguration.h file. To get the AppID login to your MoEngage account, after that go to Settings -> App Settings. You will find the App ID in this screen.");
    }
}


-(void)setupSDKWithConfig:(MoEngageSDKConfig*)sdkConfig andLaunchOptions:(NSDictionary * _Nullable)launchOptions {
    if (sdkConfig.appId && sdkConfig.appId.length > 0) {
        MoEngagePlugin *plugin = [[MoEngagePlugin alloc] init];
        [plugin initializeDefaultInstanceWithSdkConfig:sdkConfig launchOptions:launchOptions];
        [self commonSetUp:plugin andSDKConfig:sdkConfig];
    }
    else{
        NSAssert(NO, @"MoEngage - Provide the APP ID for your MoEngage App in MoEngageConfiguration.h file. To get the AppID login to your MoEngage account, after that go to Settings -> App Settings. You will find the App ID in this screen.");
    }
}

-(void)commonSetUp:(MoEngagePlugin*)plugin andSDKConfig:(MoEngageSDKConfig*)config {
    [plugin trackPluginInfo:kUnity version:kUnityPluginVersion];
    [[MoEngagePluginBridge sharedInstance] setPluginBridgeDelegate:self identifier:config.appId];
}

-(MoEngageSDKConfig*)getSDKConfigFromFile{
    MoEngageDataCenter dataCenter = [MoEngageUnityUtils fetchDataCenter];
    
    MoEngageSDKConfig* sdkConfig = [[MoEngageSDKConfig alloc] initWithAppId:kMoEngageAppID dataCenter:dataCenter];
    
    sdkConfig.analyticsPeriodicFlushDuration = [MoEngageUnityUtils fetchPeriodicFlushDuration];
    sdkConfig.analyticsDisablePeriodicFlush = [MoEngageUnityUtils isPeriodicFlushDisabled];
    sdkConfig.enableLogs = [MoEngageUnityUtils isLogsEnabled];

    sdkConfig.storageConfig = [[MoEngageStorageConfig alloc] initWithEncryptionConfig:[[MoEngageStorageEncryptionConfig alloc] initWithIsEncryptionEnabled:[MoEngageUnityUtils isStorageEncryptionEnabled]]];
    
    sdkConfig.networkConfig = [[MoEngageNetworkRequestConfig alloc] initWithAuthorizationConfig:[[MoEngageNetworkAuthorizationConfig alloc]initWithIsJwtEnbaled:[MoEngageUnityUtils isJWTEnabled]] dataSecurityConfig:[[MoEngageNetworkDataSecurityConfig alloc] initWithIsEncryptionEnabled:[MoEngageUnityUtils isApiEncryptionEnabled] encryptionKeyDebug:[MoEngageUnityUtils fetchDebugNetworkApiKey] encryptionKeyRelease:[MoEngageUnityUtils fetchReleaseNetworkApiKey]] sslVerificationConfig:[[MoEngageSSLVerificationConfig alloc] initWithIsEnabled:false certificateData:nil]];
    
    NSString* keyChainGroupName = [MoEngageUnityUtils fetchKeyChainGroupName];
    if (keyChainGroupName) {
        sdkConfig.keyChainConfig = [[MoEngageKeyChainConfig alloc]initWithGroupName: keyChainGroupName];
    }
    
    NSString* appGroupID = [self getAppGroupID];
    if (appGroupID && appGroupID.length > 0) {
        sdkConfig.appGroupID = appGroupID;
    }
    
    return sdkConfig;
    
}

-(NSString*)getAppGroupID{
    NSString *parentBundleIdentifier = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleIdentifier"];
    NSString *appGroupID = [NSString stringWithFormat:@"group.%@.moengage",parentBundleIdentifier];
    return appGroupID;
}

#pragma mark- Native to Unity Callbacks

-(void)sendCallbackToUnityForMethod:(NSString *)method withMessage:(NSDictionary *)messageDict {
    if (self.moeGameObjectName != nil) {
        NSString* objectName = self.moeGameObjectName;
        NSString* message = [MoEngageUnityUtils dictToJson:messageDict];
        UnitySendMessage([objectName UTF8String], [method UTF8String], [message UTF8String]);
    }
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
    if(unityMethodName != nil){
        [self sendCallbackToUnityForMethod:unityMethodName withMessage:message];
    }
}

@end
