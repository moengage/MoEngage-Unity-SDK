//
//  MoEngageUnityUtils.h
//  Unity-iPhone
//
//  Created by Rakshitha on 28/06/23.
//
@protocol SFSafariViewControllerDelegate;

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MoEngageSDK/MoEngageSDK.h>

@interface MoEngageUnityUtils : NSObject

+(MoEngageDataCenter)fetchDataCenter;
+(BOOL)isUnityAppControllerSwizzlingEnabled;

+(NSInteger)fetchPeriodicFlushDuration;
+(BOOL)isPeriodicFlushDisabled;
+(BOOL)isLogsEnabled;


+(BOOL)isStorageEncryptionEnabled;
+(BOOL)isJWTEnabled;
+(BOOL)isApiEncryptionEnabled;

+(NSString* _Nullable)fetchDebugNetworkApiKey;
+(NSString* _Nullable)fetchReleaseNetworkApiKey;
+(NSString* _Nullable)fetchKeyChainGroupName;

+(NSString* _Nullable)dictToJson:(NSDictionary *_Nullable)dict;
@end
