using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage {
  public interface MoEGeofenceUnityPlatform {
    void StartGeofenceMonitoring(string payload);
    void StopGeofenceMonitoring(string payload);

  }
}