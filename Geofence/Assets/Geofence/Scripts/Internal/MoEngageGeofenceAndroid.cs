using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage {

  #if UNITY_ANDROID
  public class MoEngageGeofenceAndroid: MoEGeofenceUnityPlatform {
    private
    const string TAG = "MoEngageGeofenceAndroid";

    private static AndroidJavaClass moengageAndroidClass = new AndroidJavaClass("com.moengage.unity.wrapper.geofence.MoEGeofenceWrapper");
    private static AndroidJavaObject moengageAndroid = moengageAndroidClass.CallStatic <AndroidJavaObject> ("getInstance");

    public void StartGeofenceMonitoring(string payload) {
     #if !UNITY_EDITOR
     			moengageAndroid.Call("startGeofenceMonitoring", payload);
     #endif
    }

    public void StopGeofenceMonitoring(string payload) {
    #if !UNITY_EDITOR
        moengageAndroid.Call("stopGeofenceMonitoring", payload);
    #endif
    }

  }

  #endif
}