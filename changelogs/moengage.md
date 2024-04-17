# MoEngage

# Next Release

## Next version

- iOS
  - Updated the MoEngage-iOS-SDK dependency to `9.17.0` and above.
  
# 20-12-2023

## 3.1.0

- Android
  - Support for SDK version `12.10.02`.
  - Google Policy - API `DeleteUser()` to delete User details.
- iOS
  - Updated the MoEngage-iOS-SDK dependency to `9.14.0` and above.

# 17-08-2023

## 3.0.0

- Support for Android SDK version `12.8.02`.
- Support for iOS SDK version `9.10.0`.
- Breaking Changes
  | Then | Now |
  |---------------------------|-----------------------|
  | InvalidateInAppContexts() | resetInAppContexts |
  | OptOutDataTracking(true) | EnableDataTracking() |
  | OptOutDataTracking(false) | DisableDataTracking() |

- Removed APIs
  | Removed APIs |
  |-----------------------------|
  | EnableSDKLogs() |
  | OptOutPushTracking() |
  | OptOutInAppTracking() |
  | SelfHandledPrimaryClicked() |
  | StartGeofenceMonitoring() |

- Android

  - Support for Android 13 notification permission.
  - Android Gradle Plugin version updated to 7.3.1
  - Gradle version updated to 7.4
  - Build Configuration Updates
  - Compile SDK Version 33
  - Target SDK version 33
  - Support for Android SDK version `12.8.02`
  - InApp `6.7.2`
  - Removed and replaced APIs
    | Then | Now |
    |-----------------------------------------------------------------------------|-----------------------------------------------------------------------------------------|
    | initialize(Context context, MoEngage.Builder builder) | initialiseDefaultInstance(Context context, MoEngage.Builder builder) |
    | initialize(Context context, MoEngage.Builder builder, boolean isSdkEnabled) | initialiseDefaultInstance(Context context, MoEngage.Builder builder, SdkState sdkState) |

- iOS
  - Removed and replaced APIs
    | Then | Now |
    |-----------------------------------------------------------------------------|-----------------------------------------------------------------------------------------|
    |- (void)intializeSDKWithLaunchOptions:(NSDictionary*)launchOptions andSDKState:(BOOL)isSDKEnabled; | - (void)initializeSDKWithLaunchOptions:(NSDictionary*)launchOptions withSDKState:(MoEngageSDKState)sdkState;|

### 2.3.0 (25-07-2022)

- Android
  - Native SDK updated to support `11.6.02`.
- iOS
  - Native dependencies updated to support `MoEngage-iOS-SDK` version `7.2.0`.

### 2.2.0 (02-09-2021)

- Android
  - Native SDK updated to support `11.4.00`
- iOS
  - Native dependencies updated to support `MoEngage-iOS-SDK` version `7.1.0` and above.

### 2.1.0 (11-05-2021)

- Android SDK updated to `11.2.00`

### 2.0.0 (01-03-2021)

- iOS
  - Dropped support for iOS 9.\*, plugin now supports iOS version `10.0` and above.
  - Native dependencies updated to support `MoEngage-iOS-SDK` version `7.0.0` and above.
- Android
  - Android Native SDK updated to support version `11.0.04`.
- Removed APIs

|                            Then                             |                              Now                               |
| :---------------------------------------------------------: | :------------------------------------------------------------: |
| MoEngageClient#PassPushPayload(IDictionary<string, string>) | MoEngageClient#PassFcmPushPayload(IDictionary<string, string>) |
|            MoEngageClient#PassPushToken(string)             |            MoEngageClient#PassFcmPushToken(string)             |

### 1.3.1 (16-02-2021)

- Android artifacts use manven central instead of Jcenter.

### 1.3.0 (11-01-2021)

- iOS: UnityAppController Swizzling implementation added since UnityAppController subclass Implementation was not working in case of multiple subclasses.

### 1.2.1 (29-12-2020)

- iOS: Weak link AppTrackingTransparency framework in Build Settings

### 1.2.0 (18-12-2020)

- Disable/Enable Methods added to block/unblock the SDK features.
- Push Token Callback added

### 1.1.1 (23-10-2020)

- Bugfix
  - Events not marked as non-interactive.

### 1.1.0 (30-09-2020)

- Added support for Push Templates
- Provided GDPR OptOut APIs
- iOS Native SDK Dependency set to MoEPluginBase 1.0.0
- Android SDK moved to `androidx` namespace

### 1.0.1 (07-07-2020)

- Android SDK Version update

### 1.0.0 (05-07-2020)

- Initial Release
