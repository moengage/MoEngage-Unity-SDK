
//
//  MoEngageConfiguration.m
//  MoEngage
//
//  Created by Chengappa on 28/06/20.
//  Copyright © 2020 MoEngage. All rights reserved.
//

/// MoEngage Account Identifier
#define kMoEngageAppID @"YOUR APPID"

/// Set DataCenter
#define kMoEngageRegion NULL// DATA_CENTER_01/DATA_CENTER_02/DATA_CENTER_03/DATA_CENTER_04/DATA_CENTER_05

/// Set Xcode console logs
#define kMoEngageLogsEnabled YES // YES/NO

/// Enable UnityController Swizzling.
#define kMoEngageUnityControllerSwizzlingEnabled YES // YES/NO

/// Interval at which data must be send to MoEngage
#define kMoEngageAnalyticsPeriodicFlushDuration 60 // Any value greater than 60

///  Bool to disable to periodic flush of events.
#define kMoEngageAnalyticsDisablePeriodicFlush NO // YES/NO

/// Inset value for nudge placement. Currently Nudge is not supported in Unity.
#define kMoEngageInAppSafeArea 0

/// Bool to enable Storage Encryption
#define kMoEngageStorageEncryption NO  // YES/NO

/// Set the keychain group name to save the encryption key in keychain
#define KeyChainGroupName NULL

/// If true sdk will encrypt all data in the API Request.
#define kMoEngageNetworkEncryption NO // YES/NO

/// Encryption Key of type String which will be use to encrypt/decrypt data in Debug mode.
#define kMoEngageNetworkNetworkDebugKey NULL

/// Encryption Key of type String which will be use to encrypt/decrypt data in Release mode
#define kMoEngageNetworkNetworkReleaseKey NULL

/// Jwt Configuration, if true all Network Request will be authenticated with jwt token
#define kMoEngageDataSecurity NO // YES/NO

