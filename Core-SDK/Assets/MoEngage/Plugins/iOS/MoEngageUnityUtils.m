//
//  MoEngageUnityUtils.m
//  UnityFramework
//
//  Created by Rakshitha on 28/06/23.
//

#import <Foundation/Foundation.h>
#import "MoEngageUnityUtils.h"
#import "MoEngageConfiguration.h"

@implementation MoEngageUnityUtils

+ (MoEngageDataCenter)fetchDataCenter {
#ifdef kMoEngageRegion
    NSString* dataCenter = kMoEngageRegion;
    dataCenter = dataCenter.uppercaseString;
    if ([dataCenter isEqualToString:@"DATA_CENTER_02"]){
        return MoEngageDataCenterData_center_02;
    } else if ([dataCenter isEqualToString:@"DATA_CENTER_03"]){
        return  MoEngageDataCenterData_center_03;
    } else if ([dataCenter isEqualToString:@"DATA_CENTER_04"]){
        return  MoEngageDataCenterData_center_04;
    } else if ([dataCenter isEqualToString:@"DATA_CENTER_05"]){
        return  MoEngageDataCenterData_center_05;
    }
#endif
    return MoEngageDataCenterData_center_01;
}

+ (BOOL)isPeriodicFlushDisabled {
    BOOL disablePeriodicFlush = false;
#ifdef kMoEngageAnalyticsDisablePeriodicFlush
    disablePeriodicFlush = kMoEngageAnalyticsDisablePeriodicFlush;
#endif
    return disablePeriodicFlush;
}

+(NSInteger)fetchPeriodicFlushDuration {
    NSInteger* duration = 60;
#ifdef kMoEngageAnalyticsPeriodicFlushDuration
    duration = kMoEngageAnalyticsPeriodicFlushDuration;
#endif
    return duration;
}

+ (BOOL)isLogsEnabled {
    BOOL logsEnabled = false;
#ifdef kMoEngageLogsEnabled
    logsEnabled = kMoEngageLogsEnabled;
#endif
    return logsEnabled;
}

+ (BOOL)isStorageEncryptionEnabled {
    BOOL storageEncryption = false;
#ifdef kMoEngageStorageEncryption
    storageEncryption = kMoEngageStorageEncryption;
#endif
    return storageEncryption;
}

+ (BOOL)isJWTEnabled {
    BOOL jwtEnabled = false;
#ifdef kMoEngageDataSecurity
    jwtEnabled = kMoEngageDataSecurity;
#endif
    return jwtEnabled;
}

+ (BOOL)isApiEncryptionEnabled {
    BOOL apiEncryption = false;
#ifdef kMoEngageNetworkEncryption
    apiEncryption = kMoEngageNetworkEncryption;
#endif
    return apiEncryption;
}

+ (NSString *)fetchDebugNetworkApiKey {
    NSString* debugKey = NULL;
#ifdef kMoEngageNetworkNetworkDebugKey
    debugKey = kMoEngageNetworkNetworkDebugKey;
#endif
    return debugKey;
}

+ (NSString *)fetchReleaseNetworkApiKey {
    NSString* ReleaseKey = NULL;
#ifdef kMoEngageNetworkNetworkReleaseKey
    ReleaseKey = kMoEngageNetworkNetworkReleaseKey;
#endif
    return ReleaseKey;
}

+ (NSString *)fetchKeyChainGroupName {
    NSString* groupName = NULL;
#ifdef KeyChainGroupName
    groupName = KeyChainGroupName;
#endif
    return groupName;
}


+(BOOL)isUnityAppControllerSwizzlingEnabled{
    BOOL swizzleUnityAppController = false;
#ifdef kMoEngageUnityControllerSwizzlingEnabled
    swizzleUnityAppController = kMoEngageUnityControllerSwizzlingEnabled;
#endif
    return swizzleUnityAppController;
}

+(NSString *)dictToJson:(NSDictionary *)dict {
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
    if(err != nil) {
        return nil;
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

@end

