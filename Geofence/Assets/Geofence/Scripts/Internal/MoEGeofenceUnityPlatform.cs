using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage {
  /// <summary>
  /// Common geofence interface class for iOS and android
  /// </summary>
  public interface MoEGeofenceUnityPlatform {
    void StartGeofenceMonitoring(string payload);
    void StopGeofenceMonitoring(string payload);

  }
}