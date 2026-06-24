//
//  MoEngageUnityUtils.m
//  UnityFramework
//
//  Created by Rakshitha on 28/06/23.
//

#import <Foundation/Foundation.h>
#import "MoEngageUnityUtils.h"

@implementation MoEngageUnityUtils

+(NSString *)dictToJson:(NSDictionary *)dict {
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
    if(err != nil) {
        return nil;
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

+(NSDictionary* _Nullable)fetchInfoPlistConfig {
    NSDictionary *infoPlist = [[NSBundle mainBundle] infoDictionary];
    NSDictionary *moeConfig = [infoPlist objectForKey:@"MoEngage"];
    if (![moeConfig isKindOfClass:[NSDictionary class]]) {
        return nil;
    }
    return moeConfig;
}

@end
