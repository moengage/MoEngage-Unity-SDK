//
//  NotificationService.m
//  MoENotificationServiceExtension
//
//  Created by Chengappa C D on 04/04/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

#import "NotificationService.h"
#import <UIKit/UIKit.h>
#import <MoEngageRichNotification/MoEngageRichNotification-Swift.h>

@interface NotificationService ()
@property (nonatomic, strong) void (^contentHandler)(UNNotificationContent *contentToDeliver);
@property (nonatomic, strong) UNMutableNotificationContent *bestAttemptContent;
@end

@implementation NotificationService

- (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent * _Nonnull))contentHandler {
    
    NSString* appGroupId = [self getAppGroupID];
    [MoEngageSDKRichNotification setAppGroupID:appGroupId];
    
    self.contentHandler = contentHandler;
    self.bestAttemptContent = [request.content mutableCopy];
    
    // Handle Rich Notification
    [MoEngageSDKRichNotification handleWithRichNotificationRequest:request withContentHandler:contentHandler];
    
}


/// Save the image to disk
- (void)serviceExtensionTimeWillExpire {
    // Called just before the extension will be terminated by the system.
    // Use this as an opportunity to deliver your "best attempt" at modified content, otherwise the original push payload will be used.
    self.contentHandler(self.bestAttemptContent);
}

-(NSString*)getAppGroupID{
    NSDictionary *moeConfig = [[NSBundle mainBundle].infoDictionary objectForKey:@"MoEngage"];
    return [moeConfig objectForKey:@"AppGroupName"];
}

@end


