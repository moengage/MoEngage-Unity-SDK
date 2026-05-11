//
//  MoEGeofenceBridgeHelper.m
//  MoEngage
//

#import "MoEGeofenceBridgeHelper.h"
@import MoEngagePluginGeofence;

@implementation MoEGeofenceBridgeHelper

+ (void)startGeofenceMonitoring:(NSDictionary *)payload {
    [[MoEngagePluginGeofenceBridge sharedInstance] startGeofenceMonitoring:(NSMutableDictionary *)payload];
}

+ (void)stopGeofenceMonitoring:(NSDictionary *)payload {
    [[MoEngagePluginGeofenceBridge sharedInstance] stopGeofenceMonitoring:(NSMutableDictionary *)payload];
}

@end
