
//
//  MoEUnityBinding.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityBridge.h"
#import <MoEngage/MoEngage.h>

extern "C"{

#pragma mark- Utils Methods
void enableLogs() {
    [[MoEUnityBridge sharedInstance] enableLogs:true];
}

NSString* getNSStringFromChar(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString stringWithUTF8String:""];
}

NSString* getJSONString(id val) {
    NSString *jsonString;
    
    if (val == nil) {
        return nil;
    }
    
    if ([val isKindOfClass:[NSArray class]] || [val isKindOfClass:[NSDictionary class]]) {
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:val options:NSJSONWritingPrettyPrinted error:&error];
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        
        if (error != nil) {
            jsonString = nil;
        }
    } else {
        jsonString = [NSString stringWithFormat:@"%@", val];
    }
    
    return jsonString;
}

NSMutableArray* getNSArrayFromArray(const char* array[], int size) {
    
    NSMutableArray *values = [NSMutableArray arrayWithCapacity:size];
    for (int i = 0; i < size; i ++) {
        NSString *value = getNSStringFromChar(array[i]);
        [values addObject:value];
    }
    
    return values;
}

NSMutableDictionary* getDictionaryFromJSON(const char* jsonString) {
    
    NSMutableDictionary *dict = [NSMutableDictionary dictionaryWithCapacity:1];
    
    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [getNSStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        dict = [NSJSONSerialization JSONObjectWithData:objectData
                                               options:NSJSONReadingMutableContainers
                                                 error:&jsonError];
    }
    
    return dict;
}

NSMutableArray* moe_NSArrayFromJsonString(const char* jsonString) {
    NSMutableArray *arr = [NSMutableArray arrayWithCapacity:1];
    
    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [getNSStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        arr = [NSJSONSerialization JSONObjectWithData:objectData
                                              options:NSJSONReadingMutableContainers
                                                error:&jsonError];
    }
    
    return arr;
}


char* moe_cStringCopy(const char* string) {
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}


#pragma mark- Unity Init

void initialize(const char* gameObjPayload){
    NSMutableDictionary *gameObjDict = getDictionaryFromJSON(gameObjPayload);
    NSString* gameObjectName = [gameObjDict validObjectForKey:@"gameObjectName"];
    [[MoEUnityBridge sharedInstance] initializeWithGameObject:gameObjectName];
}

#pragma mark- INSTALL/UPDATE Tracking

void setAppStatus(const char* appStatusPayload){
    NSMutableDictionary *appStatusDict = getDictionaryFromJSON(appStatusPayload);
    NSString* appStatus = [appStatusDict validObjectForKey:@"appStatus"];
    if (appStatus) {
        [[MoEUnityBridge sharedInstance] setAppStatus:appStatus];
    }
}

#pragma mark- User Attributes

void setUserAttribute(const char* userAttrPayload){
    NSMutableDictionary *userAttrDict = getDictionaryFromJSON(userAttrPayload);
    NSString* attributeType = [userAttrDict validObjectForKey:@"type"];
    NSString* attributeName = [userAttrDict validObjectForKey:@"attributeName"];
    if (attributeType) {
        if ([attributeType isEqualToString:@"general"]) {
            id attributeValue = [userAttrDict validObjectForKey:@"attributeValue"];
            [[MoEUnityBridge sharedInstance] setUserAttribute:attributeValue forKey:attributeName];
        }
        else if ([attributeType isEqualToString:@"timestamp"]){
            NSString* attributeValue = [userAttrDict validObjectForKey:@"attributeValue"];
            [[MoEUnityBridge sharedInstance] setUserAttributeDateTimeWithISOString:attributeValue forKey:attributeName];
        }
        else if ([attributeType isEqualToString:@"location"]){
            NSDictionary* locationDict = [userAttrDict validObjectForKey:@"locationAttribute"];
            if (locationDict) {
                double lat = [[locationDict validObjectForKey:@"latitude"] doubleValue];
                double lng = [[locationDict validObjectForKey:@"longitude"] doubleValue];
                [[MoEUnityBridge sharedInstance] setUserAttributeLocationLatitude:lat longitude:lng forKey:attributeName];
            }
        }
    }
}

void setAlias(const char* aliasPayload){
    NSMutableDictionary *aliasDict = getDictionaryFromJSON(aliasPayload);
    NSString* alias = [aliasDict validObjectForKey:@"alias"];
    if (alias) {
        [[MoEUnityBridge sharedInstance] setAlias:alias];
    }
}

#pragma mark- Track Event

void trackEvent(const char* eventPayload) {
    NSMutableDictionary *eventPayloadDict = getDictionaryFromJSON(eventPayload);
    [[MoEUnityBridge sharedInstance] trackEventWithPayload:eventPayloadDict];
}

#pragma mark- Push Notification

void registerForPush() {
    [[MoEUnityBridge sharedInstance] registerForPush];
}

#pragma mark- InApp Nativ
void showInApp() {
    [[MoEUnityBridge sharedInstance] showInApp];
}

void setInAppContexts(const char* contextsPayload){
    NSMutableDictionary *contextsPayloadDict = getDictionaryFromJSON(contextsPayload);
    NSArray* contexts = [contextsPayloadDict validObjectForKey:@"contexts"];
    [[MoEUnityBridge sharedInstance] setInAppContexts:contexts];
}

void invalidateInAppContexts(){
    [[MoEUnityBridge sharedInstance] invalidateInAppContexts];
}

void getSelfHandledInApp() {
    [[MoEUnityBridge sharedInstance] getSelfHandledInApp];
}

void selfHandledCampaignShown(const char* selfHandledPayload){
    NSMutableDictionary *selfHandledCampaignDict = getDictionaryFromJSON(selfHandledPayload);
    [[MoEUnityBridge sharedInstance] selfHandledCampaignShown:selfHandledCampaignDict];
}

void selfHandledCampaignClicked(const char* selfHandledPayload){
    NSMutableDictionary *selfHandledCampaignDict = getDictionaryFromJSON(selfHandledPayload);
    [[MoEUnityBridge sharedInstance] selfHandledCampaignClicked:selfHandledCampaignDict];
}

void selfHandledCampaignDismissed(const char* selfHandledPayload){
    NSMutableDictionary *selfHandledCampaignDict = getDictionaryFromJSON(selfHandledPayload);
    [[MoEUnityBridge sharedInstance] selfHandledCampaignDismissed:selfHandledCampaignDict];
}

#pragma mark- Geofence
void startGeofenceMonitoring() {
    [[MoEUnityBridge sharedInstance] startGeofenceMonitoring];
}

#pragma mark- OptOuts

void optOutOfIDFATracking(const char* optOutPayload) {
    NSMutableDictionary *optOutDict = getDictionaryFromJSON(optOutPayload);
    id optOutVal = [optOutDict validObjectForKey:@"isOptedOut"];
    if (optOutVal) {
        [[MOAnalytics sharedInstance]optOutOfIDFATracking:[optOutVal boolValue]];
    }
}

void optOutOfIDFVTracking(const char* optOutPayload) {
    NSMutableDictionary *optOutDict = getDictionaryFromJSON(optOutPayload);
    id optOutVal = [optOutDict validObjectForKey:@"isOptedOut"];
    if (optOutVal) {
        [[MOAnalytics sharedInstance]optOutOfIDFVTracking:[optOutVal boolValue]];
    }
}

#pragma mark- Reset User
void resetUser(){
    [[MoEUnityBridge sharedInstance] resetUser];
}

}

