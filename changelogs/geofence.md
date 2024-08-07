# 07-08-2024
## 2.0.0

- iOS
  - Updated the MoEngageGeofence dependency to `5.17.1` and above.
  
# 26-04-2024

## 1.2.0

- iOS
  - Updated the MoEngageGeofence dependency to `5.16.0` and above.

# 20-12-2023

## 1.1.0

- Android
  - Support for native Geofence SDK version `3.4.0`
- iOS
  - Updated the MoEngageGeofence dependency to `5.13.0` and above.

# 17-08-2023

## 1.0.0

- Support for Android Geofence SDK version `3.3.0`
- Support for iOS MoEngageGeofence SDK version `5.10.0` and above.
- Added support for `StopGeofenceMonitoring` API.

- iOS
  - Breaking Changes
    | Then | Now |
    |-----------------------------------------------------------------------------|-----------------------------------------------------------------------------------------|
    | MoEngageClient.StartGeofenceMonitoring(); | MoEngageGeofenceClient.StartGeofenceMonitoring(); |
