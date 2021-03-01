//
//  MoEUnityInitializer.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityInitializer.h"
#import <MoEPluginBase/MoEPluginBase.h>
#import <MOInApp/MOInApp.h>
#import "MoEngageConfiguration.h"
#import "MoEUnityConstants.h"

@interface MoEUnityInitializer() <MoEPluginBridgeDelegate>
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

- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions {
    [self intializeSDKWithLaunchOptions:launchOptions andSDKState:[[MoEngageCore sharedInstance] isSDKEnabled]];
}

- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions andSDKState:(BOOL)isSDKEnabled{
    MOSDKConfig* sdkConfig = [self getSDKConfigFromFile];
    [self intializeSDKWithConfig:sdkConfig withSDKState:isSDKEnabled andLaunchOptions:launchOptions];
}


- (void)intializeSDKWithConfig:(MOSDKConfig*)sdkConfig andLaunchOptions:(NSDictionary*)launchOptions{
    [self intializeSDKWithConfig:sdkConfig withSDKState:[[MoEngageCore sharedInstance] isSDKEnabled] andLaunchOptions:launchOptions];
}

- (void)intializeSDKWithConfig:(MOSDKConfig*)sdkConfig withSDKState:(BOOL)isSDKEnabled andLaunchOptions:(NSDictionary*)launchOptions{
    self.isSDKIntialized = YES;
    [self setupSDKWithConfig:sdkConfig withSDKState:isSDKEnabled andLaunchOptions:launchOptions];
}

- (void)setupSDKWithGameObject:(NSString*)gameObjectName {
    self.moeGameObjectName = gameObjectName;
    if (!self.isSDKIntialized) {
        //this will works as fallback method if AppDelegate Swizzling doesn't work
        MOSDKConfig* sdkConfig = [self getSDKConfigFromFile];
        [self setupSDKWithConfig:sdkConfig  withSDKState:[[MoEngageCore sharedInstance] isSDKEnabled] andLaunchOptions:nil];
    }
    [[MoEPluginBridge sharedInstance] pluginInitialized];
}

-(void)setupSDKWithConfig:(MOSDKConfig*)sdkConfig withSDKState:(BOOL)isSDKEnabled andLaunchOptions:(NSDictionary * _Nullable)launchOptions {
    
    if (kMoEngageLogsEnabled) {
        [MoEngage enableSDKLogs:true];
    }
    
    [MoEPluginBridge sharedInstance].bridgeDelegate = self;
    
    if (sdkConfig.moeAppID && sdkConfig.moeAppID.length > 0) {
        sdkConfig.pluginIntegrationType = UNITY;
        sdkConfig.pluginIntegrationVersion = kUnityPluginVersion;
        [[MoEPluginInitializer sharedInstance] intializeSDKWithConfig:sdkConfig withSDKState:isSDKEnabled andLaunchOptions:launchOptions];
    }
    else{
        NSAssert(NO, @"MoEngage - Provide the APP ID for your MoEngage App in MoEngageConfiguration.h file. To get the AppID login to your MoEngage account, after that go to Settings -> App Settings. You will find the App ID in this screen.");
    }
}

-(MOSDKConfig*)getSDKConfigFromFile{
    MOSDKConfig* sdkConfig = [[MoEngage sharedInstance] getDefaultSDKConfiguration];
    
    /* MoEngage - Create a MoEngageConfiguration.h file in Project's Assets > Plugin folder and provide the APP ID and Region for MoEngage Integration as shown below:
     
     #define kMoEngageAppID @"Your App ID"
     #define kMoEngageRegion @"DEFAULT"  // DEFAULT/EU/SERV3
     */
    sdkConfig.moeDataCenter = [self getMoEngageDataCenter];
    sdkConfig.moeAppID = kMoEngageAppID;
    
    sdkConfig.pluginIntegrationType = UNITY;
    sdkConfig.pluginIntegrationVersion = kUnityPluginVersion;
    sdkConfig.encryptNetworkRequests = [MoEUnityInitializer shouldEncryptNetworkRequests];
    sdkConfig.optOutIDFATracking = [MoEUnityInitializer shouldOptOutIDFATracking];
    sdkConfig.optOutIDFVTracking = [MoEUnityInitializer shouldOptOutIDFVTracking];

    NSString* appGroupID = [self getAppGroupID];
    if (appGroupID && appGroupID.length > 0) {
        sdkConfig.appGroupID = appGroupID;
    }
    
    return sdkConfig;

}

-(MODataCenter)getMoEngageDataCenter{
#ifdef kMoEngageDataCenter
    NSString* dataCenter = kMoEngageDataCenter;
    dataCenter = dataCenter.uppercaseString;
    if ([dataCenter isEqualToString:@"DATA_CENTER_02"]){
        return DATA_CENTER_02;
    }
    else if ([dataCenter isEqualToString:@"DATA_CENTER_03"]){
        return  DATA_CENTER_03;
    }
#endif
    
#ifdef kMoEngageRegion
    NSString* region = kMoEngageRegion;
    region = region.uppercaseString;
    if ([region isEqualToString:@"EU"]){
        return DATA_CENTER_02;
    }
    else if ([region isEqualToString:@"SERV3"]){
        return  DATA_CENTER_03;
    }
#endif
    
    return DATA_CENTER_01;
}

-(NSString*)getAppGroupID{
    NSString *parentBundleIdentifier = [MOCoreUtils bundleIdentifier];
    NSString *appGroupID = [NSString stringWithFormat:@"group.%@.moengage",parentBundleIdentifier];
    return appGroupID;
}

#pragma mark- MoEPluginBridgeDelegate Callbacks

-(void)sendMessageWithName:(NSString *)name andPayload:(NSDictionary *)payloadDict{
    // TODO: Remove mapper in the next release -- To use same internal name as used in MoEPluginBase
    NSString* unityMethodName = nil;
    if ([name isEqualToString:kEventNamePushTokenGenerated]){
        unityMethodName = kUnityMethodNamePushTokenGenerated;
    }
    else if ([name isEqualToString:kEventNamePushClicked]) {
        unityMethodName = kUnityMethodNamePushClicked;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignShown]){
        unityMethodName = kUnityMethodNameInAppShown;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignClicked]){
        unityMethodName = kUnityMethodNameInAppClicked;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignDismissed]){
        unityMethodName = kUnityMethodNameInAppDismissed;
    }
    else if ([name isEqualToString:kEventNameInAppSelfHandledCampaign]){
        unityMethodName = kUnityMethodNameInAppSelfHandled;
    }
    else if ([name isEqualToString:kEventNameInAppCampaignCustomAction]){
        unityMethodName = kUnityMethodNameInAppCustomAction;
    }
    
    if(unityMethodName != nil){
        NSDictionary* unityPayload = [payloadDict validObjectForKey:@"payload"];
        [self sendCallbackToUnityForMethod:unityMethodName withMessage:unityPayload];
    }
}

#pragma mark- Native to Unity Callbacks

-(void)sendCallbackToUnityForMethod:(NSString *)method withMessage:(NSDictionary *)messageDict {
    if (self.moeGameObjectName != nil) {
        NSString* objectName = self.moeGameObjectName;
        NSString* message = [self dictToJson:messageDict];
        UnitySendMessage([objectName UTF8String], [method UTF8String], [message UTF8String]);
    }
}

-(NSString *)dictToJson:(NSDictionary *)dict {
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
    if(err != nil) {
        return nil;
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

#pragma mark- Utils Swizzling

+(BOOL)isUnityAppControllerSwizzlingEnabled{
    BOOL swizzleUnityAppController = false;
#ifdef kMoEngageUnityControllerSwizzlingEnabled
    swizzleUnityAppController = kMoEngageUnityControllerSwizzlingEnabled;
#endif
    return swizzleUnityAppController;
}

+(BOOL)shouldEncryptNetworkRequests{
    BOOL shouldEncrypt = false;
#ifdef kMoEngageEncryptNetworkRequests
    shouldEncrypt = kMoEngageEncryptNetworkRequests;
#endif
    return shouldEncrypt;
}

+(BOOL)shouldOptOutIDFVTracking{
    BOOL shouldOptOut = false;
#ifdef kDefaultIDFVTrackingOptedOut
    shouldOptOut = kDefaultIDFVTrackingOptedOut;
#endif
    return shouldOptOut;
}


+(BOOL)shouldOptOutIDFATracking{
    BOOL shouldOptOut = false;
#ifdef kDefaultIDFATrackingOptedOut
    shouldOptOut = kDefaultIDFATrackingOptedOut;
#endif
    return shouldOptOut;
}

@end
