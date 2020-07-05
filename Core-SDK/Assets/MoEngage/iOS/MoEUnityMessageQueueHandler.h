//
//  MoEUnityMessageQueueHandler.h
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "MoEUnityMessage.h"

@interface MoEUnityMessageQueueHandler : NSObject
@property(nonatomic, assign) BOOL isSDKInitialized;
@property(nonatomic, strong) NSString* gameObjectName;

+(instancetype)sharedInstance;

-(void)sendMessage:(MoEUnityMessage*)message;
-(void)flushMessageQueue;

@end

