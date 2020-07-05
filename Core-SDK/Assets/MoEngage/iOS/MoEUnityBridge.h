
//
//  MoEUnityBridge.h
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//


#import <Foundation/Foundation.h>
@interface MoEUnityBridge : NSObject

@property(nonatomic, strong) NSString* moeGameObjectName;

+ (MoEUnityBridge *)sharedInstance;

- (void)initializeWithGameObject:(NSString*)gameObjectName;

- (void)setAppStatus:(NSString*)appStatus;

- (void)setAlias:(id)updatedUniqueID;
- (void)setUserAttribute:(id)userAttributeVal forKey:(NSString *)userAttributeName;
- (void)setUserAttributeDateTimeWithISOString:(NSString *)isoString forKey:(NSString *)userAttributeName;
- (void)setUserAttributeLocationLatitude:(double)lat longitude:(double)lng forKey:(NSString *)userAttributeName;

- (void)trackEventWithPayload:(NSDictionary*)payloadDict;


- (void)registerForPush;

- (void)showInApp;
- (void)setInAppContexts:(NSArray*) contextsArr;
- (void)invalidateInAppContexts;
- (void)getSelfHandledInApp;
- (void)selfHandledCampaignShown:(NSMutableDictionary*)selfHandledCampaignDict;
- (void)selfHandledCampaignClicked:(NSMutableDictionary*)selfHandledCampaignDict;
- (void)selfHandledCampaignDismissed:(NSMutableDictionary*)selfHandledCampaignDict;

- (void)startGeofenceMonitoring;

- (void)enableLogs:(BOOL)shouldEnable;
- (void)resetUser;
@end
