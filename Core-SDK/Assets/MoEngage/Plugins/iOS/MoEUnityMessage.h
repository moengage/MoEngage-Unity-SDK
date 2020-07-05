//
//  MoEUnityMessage.h
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MoEUnityMessage : NSObject
@property(nonatomic, strong) NSString* msgMethodName;
@property(nonatomic, strong) NSDictionary* msgInfoDict;

-(instancetype)initWithMethodName:(NSString*)methodName andInfoDict:(NSDictionary*)infoDict;

@end
