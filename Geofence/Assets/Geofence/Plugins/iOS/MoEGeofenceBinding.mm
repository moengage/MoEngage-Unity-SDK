
//
//  MoEGeofenceBinding.mm
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "MoEGeofenceBridgeHelper.h"

extern "C" {

static NSString* getStringFromChar(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString string];
}

static NSDictionary* getDictFromJSON(const char* jsonString) {
    if (jsonString == NULL) return @{};
    NSData *data = [getStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
    NSError *error;
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data
                                                        options:NSJSONReadingMutableContainers
                                                          error:&error];
    return dict ?: @{};
}

#pragma mark - Geofence

void startGeofenceMonitoring(const char* accountPayload) {
    [MoEGeofenceBridgeHelper startGeofenceMonitoring:getDictFromJSON(accountPayload)];
}

void stopGeofenceMonitoring(const char* accountPayload) {
    [MoEGeofenceBridgeHelper stopGeofenceMonitoring:getDictFromJSON(accountPayload)];
}

} // extern "C"
