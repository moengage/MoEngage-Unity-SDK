### 2.0.0 (01-03-2021)
- iOS 
    - Dropped support for iOS 9.*, plugin now supports iOS version `10.0` and above.
    - Native dependencies updated to support `MoEngage-iOS-SDK` version `7.0.0` and above.
- Android 
    - Android Native SDK updated to support version `11.0.04`.
- Removed APIs

|                             Then                            	|                               Now                              	|
|:-----------------------------------------------------------:	|:--------------------------------------------------------------:	|
| MoEngageClient#PassPushPayload(IDictionary<string, string>) 	| MoEngageClient#PassFcmPushPayload(IDictionary<string, string>) 	|
|             MoEngageClient#PassPushToken(string)            	|             MoEngageClient#PassFcmPushToken(string)            	|

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
