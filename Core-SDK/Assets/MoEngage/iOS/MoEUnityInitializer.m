//
//  MoEUnityInitializer.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityInitializer.h"
#import "MoEUnityConstants.h"
#import "MoEUnityMessageQueueHandler.h"
#import <MoEngage/MoEngage.h>
#import <UserNotifications/UserNotifications.h>
#import <MOInApp/MOInApp.h>
#import "MoEngageConfiguration.h"

@interface MoEUnityInitializer() <UNUserNotificationCenterDelegate, MOInAppNativDelegate>
@property(assign, nonatomic) BOOL isSDKIntialized;
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
    self.isSDKIntialized = YES;
    [self setupSDKWithLaunchOptions:launchOptions];
}

- (void)setupSDKWithGameObject:(NSString*)gameObjectName {
    if (!self.isSDKIntialized) {
        //this will works as fallback method if AppDelegate Swizzling doesn't work
        [self setupSDKWithLaunchOptions: nil];
    }
    [MoEUnityMessageQueueHandler sharedInstance].gameObjectName = gameObjectName;
    [[MoEUnityMessageQueueHandler sharedInstance] flushMessageQueue];
}

-(void)setupSDKWithLaunchOptions:(NSDictionary * _Nullable)launchOptions{
    if (kMoEngageLogsEnabled) {
        [MoEngage debug:LOG_ALL];
    }
    
    if (kDefaultIDFATrackingOptedOut) {
        [[MOAnalytics sharedInstance] optOutOfIDFATracking:true];
    }
    
    if (kDefaultIDFVTrackingOptedOut) {
        [[MOAnalytics sharedInstance] optOutOfIDFVTracking:true];
    }
    
    [MoEngage setAppGroupID:[self getAppGroupID]];
    
    if (@available(iOS 10.0, *)) {
        [UNUserNotificationCenter currentNotificationCenter].delegate = self;
    }
    // Add Push Callback Observers
    [self addObserversForPushCallbacks];
    
    /* MoEngage - Create a MoEngageConfiguration.h file in Project's Assets > Plugin folder and provide the APP ID and Region for MoEngage Integration as shown below:
     
     #define kMoEngageAppID @"Your App ID"
     #define kMoEngageRegion @"DEFAULT"  // DEFAULT/EU/SERV3
     */
    
    NSString* region = kMoEngageRegion;
    [self setMoEngageRegion:region];
    
    NSString* moeAppID = kMoEngageAppID;
    if (moeAppID.length > 0) {
#ifdef DEBUG
        [[MoEngage sharedInstance] initializeDevWithAppID:moeAppID withLaunchOptions:launchOptions];
#else
        [[MoEngage sharedInstance] initializeProdWithAppID:moeAppID withLaunchOptions:launchOptions];
#endif
    }
    else{
        NSAssert(NO, @"MoEngage - Provide the APP ID for your MoEngage App in MoEngageConfiguration.h file. To get the AppID login to your MoEngage account, after that go to Settings -> App Settings. You will find the App ID in this screen.");
    }
    
    
    [MOInApp sharedInstance].inAppDelegate = self;
    
    if([[UIApplication sharedApplication] isRegisteredForRemoteNotifications]){
        if (@available(iOS 10.0, *)) {
            [[MoEngage sharedInstance] registerForRemoteNotificationWithCategories:nil withUserNotificationCenterDelegate:self];
        } else {
            [[MoEngage sharedInstance] registerForRemoteNotificationForBelowiOS10WithCategories:nil];
        }
    }
}

-(void)setMoEngageRegion:(NSString*)region{
    region = region.uppercaseString;
    if ([region isEqualToString:@"EU"]){
        [MoEngage redirectDataToRegion:MOE_REGION_EU];
    }
    else if ([region isEqualToString:@"SERV3"]){
        [MoEngage redirectDataToRegion:MOE_REGION_SERV3];
    }
    else{
        [MoEngage redirectDataToRegion:MOE_REGION_DEFAULT];
    }
}

-(NSString*)getAppGroupID{
    NSString *parentBundleIdentifier = [MOCoreUtils bundleIdentifier];
    NSString *appGroupID = [NSString stringWithFormat:@"group.%@.moengage",parentBundleIdentifier];
    return appGroupID;
}

#pragma mark- Add Push Observers

-(void)addObserversForPushCallbacks{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(notificationClickedCallback:) name:MoEngage_Notification_Received_Notification object:nil];
}

-(void)notificationClickedCallback:(NSNotification*)notification{
    NSDictionary* pushPayload = notification.userInfo;
    if (pushPayload) {
        NSMutableDictionary* notifPayload = [[NSMutableDictionary alloc] init];
        notifPayload[@"payload"] = pushPayload;
        MoEUnityMessage* pushClick = [[MoEUnityMessage alloc] initWithMethodName:kMOEventNamePushClicked andInfoDict:notifPayload];
        [[MoEUnityMessageQueueHandler sharedInstance] sendMessage:pushClick];
    }
}

#pragma mark- iOS10 UserNotification Framework delegate methods

- (void)userNotificationCenter:(UNUserNotificationCenter *)center
didReceiveNotificationResponse:(UNNotificationResponse *)response
         withCompletionHandler:(void (^)(void))completionHandler API_AVAILABLE(ios(10.0)){
    [[MoEngage sharedInstance] userNotificationCenter:center didReceiveNotificationResponse:response];
    completionHandler();
}

-(void)userNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(UNNotificationPresentationOptions options))completionHandler API_AVAILABLE(ios(10.0)){
    completionHandler((UNNotificationPresentationOptionSound
                       | UNNotificationPresentationOptionAlert ));
}

#pragma mark - MOInAppNativDelegate - methods

-(void)inAppShownWithCampaignInfo:(MOInAppCampaign*)inappCampaign {
    NSLog(@"InApp Shown with Campaign ID %@",inappCampaign.campaign_id);
    NSMutableDictionary* inAppPayload = [[NSMutableDictionary alloc] init];
    inAppPayload[@"campaignId"] = inappCampaign.campaign_id;
    inAppPayload[@"campaignName"] = inappCampaign.campaign_name;
    
    MoEUnityMessage* inAppShownMsg = [[MoEUnityMessage alloc] initWithMethodName:kMOEventNameInAppShown  andInfoDict:inAppPayload];
    [[MoEUnityMessageQueueHandler sharedInstance] sendMessage:inAppShownMsg];
}

-(void)inAppDismissedWithCampaignInfo:(MOInAppCampaign *)inappCampaign{
    NSLog(@"InApp Dismissed with Campaign ID %@",inappCampaign.campaign_id);

    NSMutableDictionary* inAppPayload = [[NSMutableDictionary alloc] init];
    inAppPayload[@"campaignId"] = inappCampaign.campaign_id;
    inAppPayload[@"campaignName"] = inappCampaign.campaign_name;
    
    MoEUnityMessage* inAppDismissedMsg = [[MoEUnityMessage alloc] initWithMethodName:kMOEventNameInAppDismissed andInfoDict:inAppPayload];
    [[MoEUnityMessageQueueHandler sharedInstance] sendMessage:inAppDismissedMsg];
}

-(void)inAppClickedWithCampaignInfo:(MOInAppCampaign*)inappCampaign andNavigationActionInfo:(MOInAppAction*)navigationAction {
    [self sendInAppClickWithWithCampaignInfo:inappCampaign andAction:navigationAction];
}

-(void)inAppClickedWithCampaignInfo:(MOInAppCampaign*)inappCampaign andCustomActionInfo:(MOInAppAction*)customAction {
    [self sendInAppClickWithWithCampaignInfo:inappCampaign andAction: customAction];
}

-(void)selfHandledInAppTriggeredWithInfo:(MOInAppSelfHandledCampaign*)inappCampaign {
    NSMutableDictionary* selfHandledContent = [[NSMutableDictionary alloc] init];
    selfHandledContent[@"payload"] = inappCampaign.campaignContent;
    selfHandledContent[@"dismissInterval"] = [NSNumber numberWithLong:inappCampaign.autoDismissInterval];
    
    NSMutableDictionary* inAppPayload = [[NSMutableDictionary alloc] init];
    inAppPayload[@"campaignId"] = inappCampaign.campaign_id;
    inAppPayload[@"campaignName"] = inappCampaign.campaign_name;
    inAppPayload[@"selfHandled"] = selfHandledContent;
    
    MoEUnityMessage* inAppTriggerMsg = [[MoEUnityMessage alloc] initWithMethodName:kMOEventNameInAppSelfHandled andInfoDict:inAppPayload];
    [[MoEUnityMessageQueueHandler sharedInstance] sendMessage:inAppTriggerMsg];
}

- (void)sendInAppClickWithWithCampaignInfo:(MOInAppCampaign*)inappCampaign andAction:(MOInAppAction *)action{

    NSMutableDictionary* inAppPayload = [[NSMutableDictionary alloc] init];
    inAppPayload[@"campaignId"] = inappCampaign.campaign_id;
    inAppPayload[@"campaignName"] = inappCampaign.campaign_name;
    
    NSMutableDictionary* actionContent = [[NSMutableDictionary alloc] init];
    actionContent[@"kvPair"] = action.keyValuePairs;
    
    NSString* clickedEventName = kMOEventNameInAppClicked;
    if (action.actionType == NavigationAction) {
        actionContent[@"value"] = action.screenName;
        actionContent[@"navigationType"] = @"screen";
        inAppPayload[@"navigation"] = actionContent;
    }
    else if(action.actionType == CustomAction){
        clickedEventName = kMOEventNameInAppCustomAction;
        inAppPayload[@"customAction"] = actionContent;
    }
    
    MoEUnityMessage* inAppClickedMsg = [[MoEUnityMessage alloc] initWithMethodName:clickedEventName andInfoDict:inAppPayload];
    [[MoEUnityMessageQueueHandler sharedInstance] sendMessage:inAppClickedMsg];
}

@end
