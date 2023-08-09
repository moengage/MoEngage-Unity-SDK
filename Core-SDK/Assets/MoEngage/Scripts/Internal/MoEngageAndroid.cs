/*
 * Copyright (c) 2014-2020 MoEngage Inc.
 *
 * All rights reserved.
 *
 *  Use of source code or binaries contained within MoEngage SDK is permitted only to enable use of the MoEngage platform by customers of MoEngage.
 *  Modification of source code and inclusion in mobile apps is explicitly allowed provided that all other conditions are met.
 *  Neither the name of MoEngage nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *  Redistribution of source code or binaries is disallowed except with specific prior written permission. Any such redistribution must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoEMiniJSON;

namespace MoEngage {

  #if UNITY_ANDROID
  public class MoEngageAndroid: MoEngageUnityPlatform {
    
    private const string TAG = "MoEngageAndroid";
    private static AndroidJavaClass moengageAndroidClass = new AndroidJavaClass("com.moengage.unity.wrapper.MoEAndroidWrapper");
    private static AndroidJavaObject moengageAndroid = moengageAndroidClass.CallStatic < AndroidJavaObject > ("getInstance");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    public void Initialize(string gameObjectPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("initialize", gameObjectPayload);
      #endif
    }

    public void SetAppStatus(string appStatusPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("setAppStatus", appStatusPayload);
      #endif
    }

    public void SetAlias(string aliasPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("setAlias", aliasPayload);
      #endif
    }

    public void SetUserAttribute(string userAttributesPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("setUserAttribute", userAttributesPayload);
      #endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="attributes"></param>
    public void TrackEvent(string eventPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("trackEvent", eventPayload);
      #endif
    }

    public void EnableSDKLogs() {
      #if!UNITY_EDITOR
      Debug.Log(TAG + ": EnableSDKLogs::");
      moengageAndroid.Call("enableSDKLogs");
      #endif
    }

    public void Logout(string accountPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("logout", accountPayload);
      #endif
    }

    public void GetSelfHandledInApp(string accountPayload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + ": GetSelfHandledInApp::");
      moengageAndroid.Call("getSelfHandledInApp", accountPayload);
      #endif
    }

    public void ShowInApp(string accountPayload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + ": ShowInApp::");
      moengageAndroid.Call("showInApp", accountPayload);
      #endif
    }

    public static void PassFcmPushPayload(string pushPayload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + ": PassFcmPushPayload:: pushPayload: " + pushPayload);
      moengageAndroid.Call("passPushPayload", pushPayload);
      #endif
    }

    public static void PassFcmPushToken(string pushTokenPayload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + ": PassFcmPushToken:: pushToken: " + pushTokenPayload);
      moengageAndroid.Call("passPushToken", pushTokenPayload);
      #endif
    }

    public void SelfHandledShown(string selfHandledPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("selfHandledCallback", selfHandledPayload);
      #endif
    }

    public void SelfHandledClicked(string selfHandledPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("selfHandledCallback", selfHandledPayload);
      #endif
    }

    public void SelfHandledDismissed(string selfHandledPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("selfHandledCallback", selfHandledPayload);
      #endif
    }

    public void SetInAppContexts(string contextPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("setAppContext", contextPayload);
      #endif
    }

    public void ResetInAppContexts(string accountPayload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " resetInAppContexts:: ");
      moengageAndroid.Call("resetContext", accountPayload);
      #endif
    }

    public void optOutDataTracking(string optOutPayload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("optOutTracking", optOutPayload);
      #endif
    }

    public void UpdateSdkState(string payload) {
      #if!UNITY_EDITOR
      moengageAndroid.Call("updateSdkState", payload);
      #endif
    }

    public static void OnOrientationChanged() {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " OnOrientationChanged::");
      moengageAndroid.Call("onOrientationChanged");
      #endif
    }

    public static void SetupNotificationChannelsAndroid() {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " SetupNotificationChannelsAndroid::");
      moengageAndroid.Call("setUpNotificationChannels");
      #endif
    }

    public static void PushPermissionResponseAndroid(string payload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " PushPermissionResponseAndroid:: payload: " + payload);
      moengageAndroid.Call("permissionResponse", payload);
      #endif
    }

    public static void NavigateToSettingsAndroid() {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " NavigateToSettingsAndroid:: ");
      moengageAndroid.Call("navigateToSettings");
      #endif
    }

    public static void RequestPushPermissionAndroid() {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " RequestPushPermissionAndroid:: ");
      moengageAndroid.Call("requestPushPermission");
      #endif
    }

    public static void UpdatePushPermissionRequestCountAndroid(string payload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " UpdatePushPermissionRequestCountAndroid:: payload: " + payload);
      moengageAndroid.Call("updatePushPermissionRequestCount", payload);
      #endif
    }

    public static void UpdateDeviceIdentifierTrackingStatus(string payload) {
      #if!UNITY_EDITOR
      Debug.Log(TAG + " UpdateDeviceIdentifierTrackingStatus:: payload: " + payload);
      moengageAndroid.Call("deviceIdentifierTrackingStatusUpdate", payload);
      #endif
    }
  }

  #endif
}