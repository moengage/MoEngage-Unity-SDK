
//
//  MoEUnityBinding.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "MoEUnityBridgeHelper.h"

extern "C" {

static NSString* getNSStringFromChar(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString string];
}

static NSDictionary* getDictionaryFromJSON(const char* jsonString) {
    if (jsonString == NULL) return @{};
    NSData *data = [getNSStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
    NSError *error;
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data
                                                        options:NSJSONReadingMutableContainers
                                                          error:&error];
    return dict ?: @{};
}

#pragma mark - Unity Init

void initialize(const char* gameObjPayload) {
    [MoEUnityBridgeHelper initialize:getDictionaryFromJSON(gameObjPayload)];
  //  [[MoEUnityInitializer sharedInstance] setupSDKWithInitializePayload:gameObjDict];
}

#pragma mark - INSTALL/UPDATE Tracking

void setAppStatus(const char* appStatusPayload) {
    [MoEUnityBridgeHelper setAppStatus:getDictionaryFromJSON(appStatusPayload)];
}

#pragma mark - User Attributes

void setUserAttribute(const char* userAttrPayload) {
    [MoEUnityBridgeHelper setUserAttribute:getDictionaryFromJSON(userAttrPayload)];
}

void setAlias(const char* aliasPayload) {
    [MoEUnityBridgeHelper setAlias:getDictionaryFromJSON(aliasPayload)];
}

void identifyUser(const char* identifyPayload) {
    [MoEUnityBridgeHelper identifyUser:getDictionaryFromJSON(identifyPayload)];
}

void getUserIdentities(const char* accountPayload) {
    [MoEUnityBridgeHelper getUserIdentities:getDictionaryFromJSON(accountPayload)];
}

#pragma mark - Track Event

void trackEvent(const char* eventPayload) {
    [MoEUnityBridgeHelper trackEvent:getDictionaryFromJSON(eventPayload)];
}

#pragma mark - Push Notification

void registerForPush() {
    [MoEUnityBridgeHelper registerForPush];
}

void registerForProvisionalPush() {
    [MoEUnityBridgeHelper registerForProvisionalPush];
}

#pragma mark - InApp Native

void showInApp(const char* inappPayload) {
    [MoEUnityBridgeHelper showInApp:getDictionaryFromJSON(inappPayload)];
}

void setInAppContexts(const char* contextsPayload) {
    [MoEUnityBridgeHelper setInAppContexts:getDictionaryFromJSON(contextsPayload)];
}

void invalidateInAppContexts(const char* inappPayload) {
    [MoEUnityBridgeHelper invalidateInAppContexts:getDictionaryFromJSON(inappPayload)];
}

void getSelfHandledInApp(const char* inappPayload) {
    [MoEUnityBridgeHelper getSelfHandledInApp:getDictionaryFromJSON(inappPayload)];
}

void getSelfHandledInApps(const char* inappPayload) {
    [MoEUnityBridgeHelper getSelfHandledInApps:getDictionaryFromJSON(inappPayload)];
}

void showNudge(const char* nudgePayload) {
    [MoEUnityBridgeHelper showNudge:getDictionaryFromJSON(nudgePayload)];
}

void updateSelfHandledInAppStatusWithPayload(const char* selfHandledPayload) {
    [MoEUnityBridgeHelper updateSelfHandledInAppStatus:getDictionaryFromJSON(selfHandledPayload)];
}

#pragma mark - OptOuts

void optOutGDPRTracking(const char* optOutPayload) {
    [MoEUnityBridgeHelper optOutGDPRTracking:getDictionaryFromJSON(optOutPayload)];
}

#pragma mark - Reset User

void resetUser(const char* resetPayload) {
    [MoEUnityBridgeHelper resetUser:getDictionaryFromJSON(resetPayload)];
}

#pragma mark - Update SDK State

void updateSdkState(const char* sdkStatePayload) {
    [MoEUnityBridgeHelper updateSdkState:getDictionaryFromJSON(sdkStatePayload)];
}

} // extern "C"
