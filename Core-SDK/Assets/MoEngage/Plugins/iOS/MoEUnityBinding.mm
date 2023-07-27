
//
//  MoEUnityBinding.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//
@protocol SFSafariViewControllerDelegate;

#import <MoEngageSDK/MoEngageSDK.h>
#import "MoEUnityInitializer.h"
#import <MoEngagePluginBase/MoEngagePluginBase-Swift.h>

extern "C"{


NSString* getNSStringFromChar(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString stringWithUTF8String:""];
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

#pragma mark- Unity Init

void initialize(const char* gameObjPayload){
    NSMutableDictionary *gameObjDict = getDictionaryFromJSON(gameObjPayload);
    [[MoEUnityInitializer sharedInstance] setupSDKWithInitializePayload:gameObjDict];
}

#pragma mark- INSTALL/UPDATE Tracking

void setAppStatus(const char* appStatusPayload){
    NSMutableDictionary *appStatusDict = getDictionaryFromJSON(appStatusPayload);
    [[MoEngagePluginBridge sharedInstance] setAppStatus:appStatusDict];
}

#pragma mark- User Attributes

void setUserAttribute(const char* userAttrPayload){
    NSMutableDictionary *userAttrDict = getDictionaryFromJSON(userAttrPayload);
    [[MoEngagePluginBridge sharedInstance] setUserAttribute:userAttrDict];
}

void setAlias(const char* aliasPayload){
    NSMutableDictionary *aliasDict = getDictionaryFromJSON(aliasPayload);
    [[MoEngagePluginBridge sharedInstance] setAlias:aliasDict];
}

#pragma mark- Track Event

void trackEvent(const char* eventPayload) {
    NSMutableDictionary *eventPayloadDict = getDictionaryFromJSON(eventPayload);
    [[MoEngagePluginBridge sharedInstance] trackEvent:eventPayloadDict];
}

#pragma mark- Push Notification

void registerForPush() {
    [[MoEngagePluginBridge sharedInstance] registerForPush];
}

#pragma mark- InApp Nativ
void showInApp(const char* inappPayload) {
    NSMutableDictionary *inappDict = getDictionaryFromJSON(inappPayload);
    [[MoEngagePluginBridge sharedInstance] showInApp:inappDict];
}

void setInAppContexts(const char* contextsPayload){
    NSMutableDictionary *contextsPayloadDict = getDictionaryFromJSON(contextsPayload);
    [[MoEngagePluginBridge sharedInstance] setInAppContext:contextsPayloadDict];
}

void invalidateInAppContexts(const char* inappPayload){
    NSMutableDictionary *inappDict = getDictionaryFromJSON(inappPayload);
    [[MoEngagePluginBridge sharedInstance] resetInAppContext: inappDict];
}

void getSelfHandledInApp(const char* inappPayload) {
    NSMutableDictionary *inappDict = getDictionaryFromJSON(inappPayload);
    [[MoEngagePluginBridge sharedInstance] getSelfHandledInApp:inappDict];
}

void updateSelfHandledInAppStatusWithPayload(const char* selfHandledPayload){
    NSMutableDictionary *selfHandledCampaignDict = getDictionaryFromJSON(selfHandledPayload);
    [[MoEngagePluginBridge sharedInstance] updateSelfHandledImpression:selfHandledCampaignDict];
}

#pragma mark- OptOuts

void optOutGDPRTracking(const char* optOutPayload) {
    NSMutableDictionary *optOutDict = getDictionaryFromJSON(optOutPayload);
    [[MoEngagePluginBridge sharedInstance] optOutDataTracking:optOutDict];
}

#pragma mark- Reset User
void resetUser(const char* resetPayload){
    NSMutableDictionary *resetDict = getDictionaryFromJSON(resetPayload);
    [[MoEngagePluginBridge sharedInstance] resetUser:resetDict];
}

#pragma mark- Update SDK State
void updateSdkState(const char* sdkStatePayload){
    NSMutableDictionary *sdkStateDict = getDictionaryFromJSON(sdkStatePayload);
    if (sdkStateDict) {
        [[MoEngagePluginBridge sharedInstance] updateSDKState:sdkStateDict];
    }
}
}


