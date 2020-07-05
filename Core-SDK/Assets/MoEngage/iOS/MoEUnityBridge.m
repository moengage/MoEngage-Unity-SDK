
//
//  MoEUnityBridge.mm
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityBridge.h"
#import <MoEngage/MoEngage.h>
#import <UserNotifications/UserNotifications.h>
#if __has_include(<MOGeofence/MOGeofence.h>)
#import <MOGeofence/MOGeofence.h>
#endif

#import "MoEUnityConstants.h"
#import "MoEUnityInitializer.h"
#import "MoEUnityMessageQueueHandler.h"

@implementation MoEUnityBridge

#pragma mark- Initializer

+ (MoEUnityBridge*)sharedInstance {
    static MoEUnityBridge *sharedInstance = nil;
    
    if(!sharedInstance) {
        sharedInstance = [[MoEUnityBridge alloc] init];
        [sharedInstance setUnitySDKVersion:kMoEUnitySDKVersion];
    }
    
    return sharedInstance;
}

- (void)setUnitySDKVersion:(NSString*)unityVer{
    if (unityVer != nil) {
        [[NSUserDefaults standardUserDefaults] setObject:unityVer forKey:MoEngage_Unity_SDK_Version];
    }
}

- (void)initializeWithGameObject:(NSString*)gameObjectName{
    [[MoEUnityInitializer sharedInstance] setupSDKWithGameObject:gameObjectName];
}

#pragma mark- App Status

- (void)setAppStatus:(NSString*)appStatus{
    appStatus = [appStatus uppercaseString];
    if ([appStatus isEqualToString:@"UPDATE"]) {
        [[MoEngage sharedInstance] appStatus:UPDATE];
    }
    else if ([appStatus isEqualToString:@"INSTALL"]){
        [[MoEngage sharedInstance] appStatus:INSTALL];
    }
}

#pragma mark- User Attributes Methods

- (void)setAlias:(id)updatedUniqueID{
    [[MoEngage sharedInstance] setAlias:updatedUniqueID];
}

- (void)setUserAttribute:(id)userAttributeVal forKey:(NSString *)userAttributeName{
    [[MoEngage sharedInstance] setUserAttribute:userAttributeVal forKey:userAttributeName];
}

- (void)setUserAttributeDateTimeWithISOString:(NSString *)isoString forKey:(NSString *)userAttributeName{
    [[MoEngage sharedInstance] setUserAttributeISODateString:isoString forKey:userAttributeName];
}

- (void)setUserAttributeLocationLatitude:(double)lat longitude:(double)lng forKey:(NSString *)userAttributeName{
    [[MoEngage sharedInstance] setUserAttributeLocationLatitude:lat longitude:lng forKey:userAttributeName];
}

#pragma mark- Event Tracking

- (void)trackEventWithPayload:(NSDictionary*)payloadDict{
    NSString* eventName = [payloadDict validObjectForKey:@"eventName"];
    if (eventName && eventName.length > 0) {
        MOProperties* properties = nil;
        NSDictionary* eventAttrsDict = [payloadDict validObjectForKey:@"eventAttributes"];
        
        NSDictionary* generalAttrDict = [eventAttrsDict validObjectForKey:@"generalAttributes"];
        if (generalAttrDict) {
            properties = [[MOProperties alloc] initWithAttributes:[generalAttrDict mutableCopy]];
        }
        else{
            properties = [[MOProperties alloc] init];
        }
        
        NSDictionary* dateTimeAttrsDict = [eventAttrsDict validObjectForKey:@"dateTimeAttributes"];
        if (dateTimeAttrsDict && [dateTimeAttrsDict allKeys].count > 0) {
            for (NSString* key in [dateTimeAttrsDict allKeys]){
                NSString* isoDate = [dateTimeAttrsDict validObjectForKey:key];
                if (isoDate) {
                    [properties addDateISOStringAttribute:isoDate withName:key];
                }
            }
        }
        
        NSDictionary* locationAttrsDict = [eventAttrsDict validObjectForKey:@"locationAttributes"];
        if (locationAttrsDict && [locationAttrsDict allKeys].count > 0) {
            for (NSString* key in [locationAttrsDict allKeys]){
                NSDictionary* locationDict = [locationAttrsDict validObjectForKey:key];
                if (locationDict) {
                    double lat = [[locationDict validObjectForKey:@"latitude"] doubleValue];
                    double lng = [[locationDict validObjectForKey:@"longitude"] doubleValue];
                    MOGeoLocation *loc = [[MOGeoLocation alloc] initWithLatitude:lat andLongitude:lng];
                    [properties addLocationAttribute:loc withName:key];
                }
            }
        }
        
        BOOL isNonInteractive = [[eventAttrsDict validObjectForKey:@"isNonInteractive"] boolValue];
        if (isNonInteractive) {
            [properties setNonInteractive];
        }
        [[MoEngage sharedInstance] trackEvent:eventName withProperties:properties];
    }
}

#pragma mark- Push Notification

- (void)registerForPush{
    if (@available(iOS 10, *)) {
        [[MoEngage sharedInstance] registerForRemoteNotificationWithCategories:nil withUserNotificationCenterDelegate:[UNUserNotificationCenter currentNotificationCenter].delegate];
    }
    else{
         [[MoEngage sharedInstance] registerForRemoteNotificationForBelowiOS10WithCategories:nil];
    }
}

- (void)showInApp{
    [[MOInApp sharedInstance] showInApp];
}

- (void)setInAppContexts:(NSArray*)contextsArr{
    if (contextsArr != nil) {
        [[MOInApp sharedInstance] setCurrentInAppContexts:contextsArr];
    }
}

- (void)invalidateInAppContexts{
    [[MOInApp sharedInstance] invalidateInAppContexts];
}

- (void)getSelfHandledInApp{
    [[MOInApp sharedInstance] getSelfHandledInAppWithCompletionBlock:^(MOInAppSelfHandledCampaign * _Nullable inappCampaign) {
        if (inappCampaign) {
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
    }];
}

- (void)selfHandledCampaignShown:(NSMutableDictionary*)selfHandledCampaignDict{
    MOInAppSelfHandledCampaign *selfHandledCmp = [self getSelfHandledCampaignFromDict:selfHandledCampaignDict];
    if (selfHandledCmp) {
        [[MOInApp sharedInstance] selfHandledShownWithCampaignInfo:selfHandledCmp];
    }
}

- (void)selfHandledCampaignClicked:(NSMutableDictionary*)selfHandledCampaignDict{
    MOInAppSelfHandledCampaign *selfHandledCmp = [self getSelfHandledCampaignFromDict:selfHandledCampaignDict];
    if (selfHandledCmp) {
        [[MOInApp sharedInstance] selfHandledClickedWithCampaignInfo:selfHandledCmp];
    }
}

- (void)selfHandledCampaignDismissed:(NSMutableDictionary*)selfHandledCampaignDict{
    MOInAppSelfHandledCampaign *selfHandledCmp = [self getSelfHandledCampaignFromDict:selfHandledCampaignDict];
    if (selfHandledCmp) {
        [[MOInApp sharedInstance] selfHandledDismissedWithCampaignInfo:selfHandledCmp];
    }
}

-(MOInAppSelfHandledCampaign*)getSelfHandledCampaignFromDict:(NSMutableDictionary*)selfHandledCampaignDict{
    if (selfHandledCampaignDict) {
        MOInAppSelfHandledCampaign *selfHandledCmp = [[MOInAppSelfHandledCampaign alloc] init];
        selfHandledCmp.campaign_id = [selfHandledCampaignDict validObjectForKey:@"campaignId"];
        selfHandledCmp.campaign_name = [selfHandledCampaignDict validObjectForKey:@"campaignName"];
        
        NSDictionary *selfHandledContent = [selfHandledCampaignDict validObjectForKey:@"selfHandled"];
        if (selfHandledContent) {
            selfHandledCmp.campaignContent = [selfHandledContent validObjectForKey:@"payload"];
            selfHandledCmp.autoDismissInterval = [[selfHandledContent validObjectForKey:@"dismissInterval"] integerValue];
        }
        return selfHandledCmp;
    }
    return nil;
}

- (void)startGeofenceMonitoring{
    // Init Geofence if included
    Class   geofenceHandlerClass    = nil;
    id      geofenceHandler         = nil;
    geofenceHandlerClass = NSClassFromString(@"MOGeofenceHandler");
    if (geofenceHandlerClass != NULL){
        geofenceHandler = [geofenceHandlerClass sharedInstance];
        [geofenceHandler startGeofenceMonitoring];
    }else {
        MOLog(@"MOGeofence Framework unavailable");
    }
}

#pragma mark- Reset User

- (void)resetUser{
    [[MoEngage sharedInstance] resetUser];
}

#pragma mark- Misc methods

- (void)enableLogs:(BOOL)shouldEnable{
    if (shouldEnable) {
        [MoEngage debug:LOG_ALL];
    }
    else{
        [MoEngage debug:LOG_NONE];
    }
}

@end

