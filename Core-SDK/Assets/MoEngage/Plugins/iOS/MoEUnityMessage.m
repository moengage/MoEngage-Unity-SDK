//
//  MoEUnityMessage.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityMessage.h"

@implementation MoEUnityMessage

-(instancetype)initWithMethodName:(NSString*)methodName andInfoDict:(NSDictionary*)infoDict;{
    self = [super init];
    if (self) {
        self.msgMethodName = methodName;
        NSMutableDictionary* updatedInfoDict = [NSMutableDictionary dictionary];
        updatedInfoDict[@"platform"] = @"ios";
        if (infoDict) {
            [updatedInfoDict addEntriesFromDictionary:infoDict];
        }
        self.msgInfoDict = updatedInfoDict;
    }
    return self;
}
@end
