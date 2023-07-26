using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage {

  #if UNITY_ANDROID
  public class MoEngageGeofenceAndroid: MoEGeofenceUnityPlatform {
    private
    const string TAG = "MoEngagMoEngageGeofenceAndroideAndroid";

    private static AndroidJavaClass moengageAndroidClass = new AndroidJavaClass("com.moengage.unity.wrapper.MoEAndroidWrapper");
    private static AndroidJavaObject moengageAndroid = moengageAndroidClass.CallStatic < AndroidJavaObject > ("getInstance");

    public void StartGeofenceMonitoring(string payload) {

    }

    public void StopGeofenceMonitoring(string payload) {

    }

  }

  #endif
}