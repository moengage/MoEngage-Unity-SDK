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

namespace MoEngage
{

#if UNITY_ANDROID
	public class MoEngageAndroid: MoEngageUnityPlatform
	{
		private const string TAG = "MoEngageAndroid";

		private static AndroidJavaClass moengageAndroidClass = new AndroidJavaClass("com.moengage.unity.wrapper.MoEAndroidWrapper");
		private static AndroidJavaObject moengageAndroid = moengageAndroidClass.CallStatic<AndroidJavaObject>("getInstance");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameObject"></param>
		public void Initialize(string gameObjectPayload) 
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("initialize", gameObjPayload);
#endif
	    }

    public void SetAppStatus(string appStatusPayload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("setAppStatus", appStatusPayload);
#endif
	    }

		public void SetAlias(string aliasPayload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("setAlias", aliasPayload);
#endif
	    }

		public void SetUserAttribute(string userAttributesPayload) 
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("setUserAttribute", userAttributesPayload);
#endif
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="attributes"></param>
	    public void TrackEvent(string eventPayload) 
	    {
#if !UNITY_EDITOR
			moengageAndroid.Call("trackEvent", eventPayload);
#endif
	    }

	  public void EnableSDKLogs()
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": EnableSDKLogs::");
			moengageAndroid.Call("enableSDKLogs");
#endif
		}

		public void Logout(string accountPayload) 
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("logout");
#endif
		}

	    public void GetSelfHandledInApp(string accountPayload) 
		{	
#if !UNITY_EDITOR
			Debug.Log(TAG + ": GetSelfHandledInApp::");
			moengageAndroid.Call("getSelfHandledInApp");
#endif
		}

		public void ShowInApp(string accountPayload) 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": ShowInApp::");
			moengageAndroid.Call("showInApp");
#endif
		}

		public static void PassFcmPushPayload(IDictionary<string, string> pushPayloadDict) 
	    {
#if !UNITY_EDITOR
			Debug.Log(TAG + ": PassFcmPushPayload::");
			
			string pushPayload = MoEUtils.GetPushPayload(new Dictionary<string, string> (pushPayloadDict), MoEConstants.PUSH_SERVICE_TYPE_FCM);
			Debug.Log(TAG + ": PassFcmPushPayload:: pushPayload: " + pushPayload);
			moengageAndroid.Call("passPushPayload", pushPayload);
#endif
	    }

	    public static void PassFcmPushToken(string pushToken) 
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + ": PassFcmPushToken:: ");
			string pushTokenPayload = MoEUtils.GetPushTokenPayload(pushToken, MoEConstants.PUSH_SERVICE_TYPE_FCM);
			Debug.Log(TAG + ": PassFcmPushToken:: pushToken: " + pushTokenPayload);
			moengageAndroid.Call("passPushToken", pushTokenPayload);
#endif
	    }

		public void SelfHandledShown(string selfHandledPayload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("selfHandledCallback", selfHandledPayload);
#endif
		}

		public  SelfHandledClicked(string selfHandledPayload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("selfHandledCallback", selfHandledPayload);
#endif
		}

		public void SelfHandledDismissed(string selfHandledPayload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("selfHandledCallback", selfHandledPayload);
#endif
		}

		public void SetAppContext(string contextPayload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("setAppContext", contextPayload);
#endif
		}

		public void InvalidateAppContext(string accountPayload)
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " InvalidateAppContext:: " );
			moengageAndroid.Call("resetContext");
#endif
		}

		public  void optOutDataTracking(string optOutPayload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("optOutTracking", payload);
#endif
		}

		public void UpdateSdkState(string payload)
		{
#if !UNITY_EDITOR
			moengageAndroid.Call("updateSdkState", payload);
#endif
		}

		public  void OnOrientationChanged()
		{
#if !UNITY_EDITOR
			Debug.Log(TAG + " OnOrientationChanged::");
			moengageAndroid.Call("onOrientationChanged");
#endif
		}

        public static void EnableAdIdTracking()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + " EnableAdIdTracking::");
			string payload = MoEUtils.GetAdIdTrackingStatus(true);
			Debug.Log(TAG + " EnableAdIdTracking:: payload " + payload);
            moengageAndroid.Call("deviceIdentifierTrackingStatusUpdate", payload);
#endif
        }

        public static void DisableAdIdTracking()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + " DisableAdIdTracking::");
			string payload = MoEUtils.GetAdIdTrackingStatus(false);
			Debug.Log(TAG + " DisableAdIdTracking:: payload " + payload);
            moengageAndroid.Call("deviceIdentifierTrackingStatusUpdate", payload);
#endif
        }

        public static void EnableAndroidIdTracking()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + " EnableAndroidIdTracking::");
			string payload = MoEUtils.GetAndroidIdTrackingStatus(true);
			Debug.Log(TAG + " EnableAndroidIdTracking:: payload " + payload);
            moengageAndroid.Call("deviceIdentifierTrackingStatusUpdate", payload);
#endif
        }

        public static void DisableAndroidIdTracking()
        {
#if !UNITY_EDITOR
			Debug.Log(TAG + " DisableAndroidIdTracking::");
			string payload = MoEUtils.GetAndroidIdTrackingStatus(false);
			Debug.Log(TAG + " DisableAndroidIdTracking:: payload " + payload);
            moengageAndroid.Call("deviceIdentifierTrackingStatusUpdate", payload);
#endif
        }

		public void RegisterForPush()
        {
#if !UNITY_EDITOR
			Debug.Log("Not supported");
#endif
        }

	}

#endif
}