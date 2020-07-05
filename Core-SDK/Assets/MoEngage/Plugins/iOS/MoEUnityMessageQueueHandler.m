//
//  MoEUnityMessageQueueHandler.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "MoEUnityMessageQueueHandler.h"

@interface MoEUnityMessageQueueHandler()
@property(nonatomic,strong) NSMutableArray* messageQueue;
@end

@implementation MoEUnityMessageQueueHandler

#pragma mark- Initialization

+(instancetype)sharedInstance{
    static dispatch_once_t onceToken;
    static MoEUnityMessageQueueHandler *instance;
    dispatch_once(&onceToken, ^{
        instance = [[MoEUnityMessageQueueHandler alloc] init];
    });
    return instance;
}


-(instancetype)init{
    self = [super init];
    if (self) {
        self.messageQueue = [NSMutableArray array];
    }
    return self;
}

#pragma mark- Send Message

-(void)sendMessage:(MoEUnityMessage*)message{
    if (message == nil || message.msgMethodName == nil) {
        return;
    }
    
    if (self.isSDKInitialized) {
        [self sendCallbackToUnityForMethod:message.msgMethodName withMessage:message.msgInfoDict];
    }
    else{
        if (self.messageQueue == nil) {
            self.messageQueue = [NSMutableArray array];
        }
        [self.messageQueue addObject:message];
    }
}

#pragma mark- Flush Message Queue

-(void)flushMessageQueue{
    self.isSDKInitialized = true;
    if (self.messageQueue != nil && self.messageQueue.count > 0) {
        for (MoEUnityMessage* message in self.messageQueue) {
            [self sendCallbackToUnityForMethod:message.msgMethodName withMessage:message.msgInfoDict];
        }
        [self.messageQueue removeAllObjects];
    }
}

#pragma mark- Native to Unity Callbacks

-(void)sendCallbackToUnityForMethod:(NSString *)method withMessage:(NSDictionary *)messageDict {
    if (self.gameObjectName != nil) {
        NSString* objectName = self.gameObjectName;
        NSString* message = [self dictToJson:messageDict];
        UnitySendMessage([objectName UTF8String], [method UTF8String], [message UTF8String]);
    }
}

-(NSString *)dictToJson:(NSDictionary *)dict {
    NSError *err;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&err];
    if(err != nil) {
        return nil;
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

@end
