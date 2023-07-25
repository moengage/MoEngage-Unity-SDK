//
//  NotificationViewController.m
//  MoEPushTemplateExtension
//
//  Created by Chengappa C D on 14/07/20.
//  Copyright © 2020 MoEngage Inc. All rights reserved.
//


#import <UIKit/UIKit.h>
#import "NotificationViewController.h"
#import <UserNotifications/UserNotifications.h>
#import <UserNotificationsUI/UserNotificationsUI.h>
#import <WebKit/WebKit.h>
#import <MoEngageRichNotification/MoEngageRichNotification-Swift.h>

@interface NotificationViewController () <UNNotificationContentExtension>

@end

@implementation NotificationViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Set App Group ID
    [MoEngageSDKRichNotification setAppGroupID:[self getAppGroupID]];
}

- (void)didReceiveNotification:(UNNotification *)notification {
    // Method to add template to UI
    [MoEngageSDKRichNotification addPushTemplateToController: self withNotification:notification];
}

-(NSString*)getAppGroupID{
    NSString *extbundleIdentifier = [[NSBundle bundleForClass:[self class]] bundleIdentifier];
    NSString *parentBundleIdentifier = [extbundleIdentifier stringByReplacingOccurrencesOfString:@".MoEPushTemplateExtension" withString:@""];
    NSString *appGroupID = [NSString stringWithFormat:@"group.%@.moengage",parentBundleIdentifier];
    return appGroupID;
}


@end
