//
//  MoEUnityConstants.m
//  MoEngage
//
//  Created by Chengappa C D on 03/07/20.
//

#import "MoEUnityConstants.h"

NSString* const kUnityPluginVersion = @"3.0.0";

// Callbacks Method Names
NSString* const kUnityMethodNamePushTokenGenerated = @"PushToken";
NSString* const kUnityMethodNamePushClicked = @"PushClicked";
NSString* const kUnityMethodNameInAppShown = @"InAppCampaignShown";
NSString* const kUnityMethodNameInAppClicked = @"InAppCampaignClicked";
NSString* const kUnityMethodNameInAppDismissed = @"InAppCampaignDismissed";
NSString* const kUnityMethodNameInAppSelfHandled = @"InAppCampaignSelfHandled";
NSString* const kUnityMethodNameInAppCustomAction = @"InAppCampaignCustomAction";

// PluginBase Callbacks Method Names
NSString* const kPushTokenGenerated = @"MoEPushTokenGenerated";
NSString* const kPushClicked = @"MoEPushClicked";
NSString* const kInAppShown = @"MoEInAppCampaignShown";
NSString* const kInAppClicked = @"MoEInAppCampaignClicked";
NSString* const kInAppDismissed = @"MoEInAppCampaignDismissed";
NSString* const kInAppSelfHandled = @"MoEInAppCampaignSelfHandled";
NSString* const kInAppCustomAction = @"MoEInAppCampaignCustomAction";

// Platform
NSString* const kUnity = @"unity";
