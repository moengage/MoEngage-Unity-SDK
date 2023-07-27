
//
//  MoEUnityBinding.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <MoEngagePluginGeofence/MoEngagePluginGeofence-Swift.h>



extern "C"{


NSString* getStringFromChar(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString stringWithUTF8String:""];
}

NSMutableDictionary* getDictFromJSON(const char* jsonString) {

    NSMutableDictionary *dict = [NSMutableDictionary dictionaryWithCapacity:1];

    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [getStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        dict = [NSJSONSerialization JSONObjectWithData:objectData
                                               options:NSJSONReadingMutableContainers
                                                 error:&jsonError];
    }

    return dict;
}

#pragma mark- Geofence

    void startGeofenceMonitoring(const char* accountPayload) {
        NSMutableDictionary *accountDict = getDictFromJSON(accountPayload);
        [[MoEngagePluginGeofenceBridge sharedInstance] startGeofenceMonitoring:accountDict];
    }
    
    void stopGeofenceMonitoring(const char* accountPayload) {
        NSMutableDictionary *accountDict = getDictFromJSON(accountPayload);
        [[MoEngagePluginGeofenceBridge sharedInstance] stopGeofenceMonitoring:accountDict];
    }
}


