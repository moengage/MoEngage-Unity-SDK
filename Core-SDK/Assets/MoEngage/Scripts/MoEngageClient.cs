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
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage
{
    public class MoEngageClient : MonoBehaviour
    {
       private static string appId;

       private static MoEngageUnityPlatform _moengageHandler;
       private static MoEngageUnityPlatform moengageHandler {
        get {
           if (_moengageHandler != null) {
                return _moengageHandler;
            }
            #if UNITY_ANDROID
              _moengageHandler = new MoEngageAndroid();
            #elif UNITY_IOS
              _moengageHandler = new MoEngageiOS();
             #endif
             return _moengageHandler;
           }
        }

        private static Boolean isPluginInitialized() {
            return _moengageHandler != null;
        }
        
        void Awake()
        {
#if (UNITY_IPHONE || UNITY_ANDROID)
           DontDestroyOnLoad(gameObject);
#endif
        }

        #region Initialize
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject">Instance of Game Object</param>
        /// <param name="appId">Account Identifier</param>
        public static void Initialize(GameObject gameObject, string appId)
        {
          MoEngageClient.appId = appId;
          string gameObjPayload = MoEUtils.GetInitializePayload(gameObject ? gameObject.name : null, appId);
          Debug.Log(": Initialize:: payload: " + gameObjPayload);

          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.Initialize(gameObjPayload);
          #endif
        }
        #endregion

        #region AppStatus Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appStatus">Instance of MoEAppStatus</param>
        public static void SetAppStatus(MoEAppStatus appStatus)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetAppStatus:: appStatus: " + appStatus);
          string appStatusPayload = MoEUtils.GetAppStatusPayload(appStatus, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
           moengageHandler.SetAppStatus(appStatusPayload);
          #endif
        }

        #endregion

        #region UserAttribute Tracking Methods

        /// <summary>
        /// Updates the already set unique identifier, sets a unique identifier if not set already.
        /// </summary>
        /// <param name="alias"></param>
        public static void SetAlias(string alias)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetAlias:: alias: " + alias);
          string aliasPayload = MoEUtils.GetAliasPayload(alias, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
             moengageHandler.SetAlias(aliasPayload);
          #endif
        }


        /// <summary>
        /// Tracks a unique identifier for the user.
        /// </summary>
        /// <param name="uniqueId"></param>
        public static void SetUniqueId(string uniqueId)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetUserAttribute:: attributeName: " + MoEConstants.USER_ATTRIBUTE_UNIQUE_ID + " : attributeValue: " + uniqueId);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(MoEConstants.USER_ATTRIBUTE_UNIQUE_ID, MoEConstants.ATTRIBUTE_TYPE_GENERAL, uniqueId, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                moengageHandler.SetUserAttribute(userAttributesPayload);
          #endif
        }

        /// <summary>
        /// Track user's first name.
        /// </summary>
        /// <param name="firstName"></param>
        public static void SetFirstName(string firstName)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetUserAttribute:: attributeName: " + MoEConstants.USER_ATTRIBUTE_USER_FIRST_NAME + " : attributeValue: " + firstName);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(MoEConstants.USER_ATTRIBUTE_USER_FIRST_NAME, MoEConstants.ATTRIBUTE_TYPE_GENERAL, firstName, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
             moengageHandler.SetUserAttribute(userAttributesPayload);
           #endif
        }

        /// <summary>
        /// Track user's last name.
        /// </summary>
        /// <param name="lastName"></param>
        public static void SetLastName(string lastName)
        {
           if (!isPluginInitialized()) return;
           Debug.Log(": SetUserAttribute:: attributeName: " + MoEConstants.USER_ATTRIBUTE_USER_LAST_NAME + " : attributeValue: " + lastName);
           string userAttributesPayload = MoEUtils.GetUserAttributePayload(MoEConstants.USER_ATTRIBUTE_USER_LAST_NAME, MoEConstants.ATTRIBUTE_TYPE_GENERAL, lastName, appId);
           #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                 moengageHandler.SetUserAttribute(userAttributesPayload);
           #endif
        }

        /// <summary>
        /// Track user's email-id.
        /// </summary>
        /// <param name="emailId"></param>
        public static void SetEmail(string emailId)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetUserAttribute:: attributeName: " + MoEConstants.USER_ATTRIBUTE_USER_EMAIL + " : attributeValue: " + emailId);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(MoEConstants.USER_ATTRIBUTE_USER_EMAIL, MoEConstants.ATTRIBUTE_TYPE_GENERAL, emailId, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
             moengageHandler.SetUserAttribute(userAttributesPayload);
          #endif
        }

        /// <summary>
        /// Track user's phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static void SetPhoneNumber(string phoneNumber)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetUserAttribute:: attributeName: " + MoEConstants.USER_ATTRIBUTE_USER_MOBILE + " : attributeValue: " + phoneNumber);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(MoEConstants.USER_ATTRIBUTE_USER_MOBILE, MoEConstants.ATTRIBUTE_TYPE_GENERAL, phoneNumber, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
              moengageHandler.SetUserAttribute(userAttributesPayload);
          #endif
        }

        /// <summary>
        /// Track user's gender.
        /// </summary>
        /// <param name="gender"></param>
        public static void SetGender(MoEUserGender gender)
        {
          if (!isPluginInitialized()) return;

          string genderVal = gender.ToString().ToLower();

          Debug.Log(": SetUserAttribute:: attributeName: " + MoEConstants.USER_ATTRIBUTE_USER_GENDER + " : attributeValue: " + genderVal);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(MoEConstants.USER_ATTRIBUTE_USER_GENDER, MoEConstants.ATTRIBUTE_TYPE_GENERAL, genderVal, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
               moengageHandler.SetUserAttribute(userAttributesPayload);
          #endif
        }


        /// <summary>
        /// Tracks birthdate as user attribute.
        /// </summary>
        /// <param name="time" - Supported format - [yyyy-MM-dd|yyyyMMdd][T(hh:mm[:ss[.sss]]|hhmm[ss[.sss]])]?[Z|[+-]hh:mm]]></param>
        public static void SetBirthdate(string time)
        {
           if (!isPluginInitialized()) return;
           Debug.Log(": SetUserAttributeISODate:: attributeName: " + MoEConstants.USER_ATTRIBUTE_USER_BDAY + " : isoDate: " + time);
           string userAttributesPayload = MoEUtils.GetUserAttributePayload(MoEConstants.USER_ATTRIBUTE_USER_BDAY, MoEConstants.ATTRIBUTE_TYPE_TIMESTAMP, time, appId);
           #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
             moengageHandler.SetUserAttribute(userAttributesPayload);
           #endif
        }

        /// <summary>
        /// Tracks user's location as attribute.
        /// </summary>
        /// <param name="location"></param>
        public static void SetUserLocation(GeoLocation location)
        {
          if (!isPluginInitialized()) return;
          Dictionary < string, double > locationDict = location.ToDictionary();

          string attributeName = default;

          #if UNITY_ANDROID
                attributeName = MoEConstants.USER_ATTRIBUTE_USER_LOCATION_ANDROID;
          #elif UNITY_IOS
                attributeName = MoEConstants.USER_ATTRIBUTE_USER_LOCATION_IOS;
          #endif

          Debug.Log(": SetUserAttributeLocation:: attributeName: " + attributeName + " : locationDict: " + locationDict);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_LOCATION, locationDict, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
               moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif 
       }


        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, int attributeValue)
        {
         if (!isPluginInitialized()) return;
         Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
         string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
         moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif
        }


        /// <summary>
        /// Tracks array of user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, int[] attributeValue)
        {
         if (!isPluginInitialized()) return;
         Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
         string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
          moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, double attributeValue)
        {
         if (!isPluginInitialized()) return;
         Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
         string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
          moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif
        }

        /// <summary>
        /// Tracks array of user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, double[] attributeValue) 
        {
           if (!isPluginInitialized()) return;
           Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
           string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
           #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                moengageHandler.SetUserAttribute(userAttributesPayload);
           #endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, float attributeValue)
        {
         if (!isPluginInitialized()) return;
         Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
         string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
          moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif
        }

        /// <summary>
        /// Tracks array of user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, float[] attributeValue) 
        {
         if (!isPluginInitialized()) return;
         Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
         string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
           moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, bool attributeValue)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.SetUserAttribute(userAttributesPayload);
          #endif
        }

        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, long attributeValue)
        {
            if (!isPluginInitialized()) return;
            Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
            string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
            #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                moengageHandler.SetUserAttribute(userAttributesPayload);
            #endif
        }

        /// <summary>
        /// Tracks array of user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, long[] attributeValue)
        {
                if (!isPluginInitialized()) return;
                Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
                string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
                #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                moengageHandler.SetUserAttribute(userAttributesPayload);
                #endif
        }


        /// <summary>
        /// Tracks a user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, string attributeValue)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.SetUserAttribute(userAttributesPayload);
          #endif
        }

        /// <summary>
        /// Tracks array of user attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetUserAttribute(string attributeName, string[] attributeValue) 
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": SetUserAttribute:: attributeName: " + attributeName + " : attributeValue: " + attributeValue);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_GENERAL, attributeValue, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.SetUserAttribute(userAttributesPayload);
          #endif
        }

        /// <summary>
        /// Tracks a user date attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="isoDate" - Supported format - [yyyy-MM-dd|yyyyMMdd][T(hh:mm[:ss[.sss]]|hhmm[ss[.sss]])]?[Z|[+-]hh:mm]]></param>
        public static void SetUserAttributeISODate(string attributeName, string isoDate)
        {
         if (!isPluginInitialized()) return;
         Debug.Log(": SetUserAttributeISODate:: attributeName: " + attributeName + " : isoDate: " + isoDate);
         string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_TIMESTAMP, isoDate, appId);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif
        }

        /// <summary>
        /// Tracks a user location attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="location"></param>
        public static void SetUserAttributeLocation(string attributeName, GeoLocation location)
        {
          if (!isPluginInitialized()) return;
          Dictionary < string, double > locationDict = location.ToDictionary();
          Debug.Log(": SetUserAttributeLocation:: attributeName: " + attributeName + ": locationDict: " + locationDict);
          string userAttributesPayload = MoEUtils.GetUserAttributePayload(attributeName, MoEConstants.ATTRIBUTE_TYPE_LOCATION, locationDict, appId);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
               moengageHandler.SetUserAttribute(userAttributesPayload);
         #endif
        }
        #endregion

        #region User Reset

        /// <summary>
        /// Invalidates the existing user and session. A new user and session is created.
        /// </summary>
        public static void Logout()
        {
         if (!isPluginInitialized()) return;
         string accountPayload = MoEUtils.GetAccountPayload(appId);
         Debug.Log(": LogOut:: payload: " + accountPayload);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.Logout(accountPayload);
         #endif
        }

        #endregion

        #region Track Event

        /// <summary>
        /// Tracks an event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="properties">Event Attributes.</param>
        public static void TrackEvent(string eventName, Properties properties)
        {
          if (!isPluginInitialized()) return;
          Debug.Log(": TrackEvent:: eventName: " + eventName + "\n properties: " + properties);
          string eventPayload = MoEUtils.GetEventPayload(eventName, properties, appId);
          Debug.Log(": TrackEvent:: eventPayload: " + eventPayload);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
               moengageHandler.TrackEvent(eventPayload);
         #endif
        }

        #endregion

        #region InApp Methods
        /// <summary>
        /// 
        /// </summary>
        public static void ShowInApp()
        {
          if (!isPluginInitialized()) return;
          string accountPayload = MoEUtils.GetAccountPayload(appId);
          Debug.Log(": ShowInApp:: payload: " + accountPayload);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
               moengageHandler.ShowInApp(accountPayload);
          #endif
        }

        public static void SetInAppContexts(string[] contexts)
        {
         if (!isPluginInitialized()) return;
         string contextPayload = MoEUtils.GetContextsPayload(contexts, appId);
         Debug.Log(":SetInAppContexts:: payload: " + contextPayload);

         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.SetInAppContexts(contextPayload);
         #endif

        }

        public static void InvalidateInAppContexts()
        {
         if (!isPluginInitialized()) return;
         string accountPayload = MoEUtils.GetAccountPayload(appId);
         Debug.Log(": Reset InAppContext:: payload: " + accountPayload);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
           moengageHandler.InvalidateInAppContexts(accountPayload);
         #endif
        }

        public static void GetSelfHandledInApp()
        {
         if (!isPluginInitialized()) return;
         string accountPayload = MoEUtils.GetAccountPayload(appId);
         Debug.Log(": SelfHanledInApp:: payload: " + accountPayload);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
           moengageHandler.GetSelfHandledInApp(accountPayload);
         #endif 
        }
    /// <summary>
    /// Track shown impression for SelfHandled campaign.
    /// </summary>
    public static void SelfHandledShown(InAppSelfHandledCampaignData inAppData) {
      if (!isPluginInitialized()) return;
      Debug.Log(" SelfHandledShown:: ");
      string payload = MoEUtils.GetSelfHandledPayload(inAppData, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_IMPRESSION);
      Debug.Log(" SelfHandledShown() payload: " + payload);
      #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
      moengageHandler.SelfHandledShown(payload);
      #endif
    }

    /// <summary>
    /// Track Clicked impression for SelfHandled campaign
    /// </summary>
    public static void SelfHandledClicked(InAppSelfHandledCampaignData inAppData) {
      if (!isPluginInitialized()) return;
      Debug.Log(" SelfHandledClicked:: ");
      string payload = MoEUtils.GetSelfHandledPayload(inAppData, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_CLICK);
      Debug.Log(" SelfHandledClicked:: payload: " + payload);
      #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
      moengageHandler.SelfHandledClicked(payload);
      #endif
    }

    /// <summary>
    /// Track Dismissed impression for SelfHandled campaign
    /// </summary>
    public static void SelfHandledDismissed(InAppSelfHandledCampaignData inAppData) {
      if (!isPluginInitialized()) return;
      Debug.Log(" SelfHandledDismissed::");
      string payload = MoEUtils.GetSelfHandledPayload(inAppData, MoEConstants.ATTRIBUTE_TYPE_SELF_HANDLED_DISMISSED);
      Debug.Log(" SelfHandledDismissed:: payload: " + payload);
      #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
      moengageHandler.SelfHandledDismissed(payload);
      #endif
    }
        #endregion

        #region GDPR OptOut Methods

        public static void optOutDataTracking(bool shouldOptOut)
        { 
          if (!isPluginInitialized()) return;
          Debug.Log(" optOutDataTracking::");
          string payload = MoEUtils.GetOptOutTrackingPayload(MoEConstants.PARAM_TYPE_DATA, shouldOptOut, appId);
          Debug.Log(" optOutDataTracking:: payload: " + payload);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                moengageHandler.optOutDataTracking(payload);
          #endif
        }

        #endregion

        #region Enable/Disable SDK Methods

        public static void EnableSdk()
        {
         if (!isPluginInitialized()) return;
         string payload = MoEUtils.GetSdkStatePayload(true, appId);
         Debug.Log(" UpdateSdkState:: payload " + payload);
         #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            moengageHandler.UpdateSdkState(payload);
         #endif
        }

        public static void DisableSdk()
        {
           if (!isPluginInitialized()) return;
          string payload = MoEUtils.GetSdkStatePayload(false, appId);
          Debug.Log(" UpdateSdkState:: payload " + payload);
          #if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
             moengageHandler.UpdateSdkState(payload);
          #endif
        }

        #endregion

        #region iOS Specific Methods
       public static void RegisterForPush()
       {
         if (!isPluginInitialized()) return;
         Debug.Log("Regiter for Push");
         #if UNITY_IOS && !UNITY_EDITOR
            moengageHandler.RegisterForPush();
         #endif
       }

        #endregion

        #region Android Specific Methods

        public static void PassFcmPushPayload(IDictionary<string, string> pushPayload)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.PassFcmPushPayload(pushPayload);
#endif
        }

        public static void PassFcmPushToken(string pushToken)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.PassFcmPushToken(pushToken);
#endif
        }

        public static void OnOrientationChanged()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.OnOrientationChanged();
#endif
        }

        public static void EnableAdIdTracking()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.EnableAdIdTracking();
#endif
        }

        public static void DisableAdIdTracking()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.DisableAdIdTracking();
#endif
        }

        public static void EnableAndroidIdTracking()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.EnableAndroidIdTracking();
#endif
        }

        public static void DisableAndroidIdTracking()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MoEngageAndroid.DisableAndroidIdTracking();
#endif
        }

        #endregion

    }
}