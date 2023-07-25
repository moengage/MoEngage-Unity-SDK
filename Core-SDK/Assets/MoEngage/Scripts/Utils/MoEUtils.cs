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
using MoEMiniJSON;

namespace MoEngage {
<<<<<<< HEAD
  public class MoEUtils {
=======
  /// Class responsible to construct the dict and serialize it inorder to pass it to the native.
  public class MoEUtils {

>>>>>>> MOEN-20006-Unity-Multiinstance
    public static Dictionary < string, string > GetAppIdPayload(string appId) {
      var payloadDict = new Dictionary < string,
        string > () {
          {
            MoEConstants.PAYLOAD_APPID, appId
          }
        };

      return payloadDict;
    }

    public static Dictionary < string, string > GetGameObjectPayload(string gameObjectName) {
      var payloadDict = new Dictionary < string,
        string > () {
          {
            MoEConstants.PAYLOAD_GAME_OBJECT,
              gameObjectName
          }
        };

      return payloadDict;
    }

    public static string GetInitializePayload(string gameObjectName, string appId) {

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            GetGameObjectPayload(gameObjectName)
          }
        };

      return Json.Serialize(payloadDict);
    }

    public static string GetAppStatusPayload(MoEAppStatus appStatus, string appId) {
      var appStatusDict = new Dictionary < string,
        string > {
          {
            MoEConstants.ARGUMENT_APP_STATUS, appStatus.ToString()
          }
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            appStatusDict
          }
        };

      return Json.Serialize(payloadDict);
    }

    public static string GetAliasPayload(string alias, string appId) {
      var aliasDict = new Dictionary < string,
        string > {
          {
            MoEConstants.ARGUMENT_ALIAS, alias
          }
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            aliasDict
          }
        };

      return Json.Serialize(payloadDict);
    }
    public static string GetUserAttributePayload < T > (string attrName, string attrType, T attrValue, string appId) {

      var userAttributesDict = new Dictionary < string,
        object > {
          {
            MoEConstants.ARGUMENT_USER_ATTRIBUTE_NAME, attrName
          },
          {
            MoEConstants.ARGUMENT_TYPE,
            attrType
          },
          {
            attrType.Equals(MoEConstants.ATTRIBUTE_TYPE_LOCATION) ?
            MoEConstants.ARGUMENT_USER_ATTRIBUTE_LOCATION_VALUE : MoEConstants.ARGUMENT_USER_ATTRIBUTE_VALUE,
            attrValue
          }
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            userAttributesDict
          }
        };

      return Json.Serialize(payloadDict);
    }

    public static string GetEventPayload(string eventName, Properties properties, string appId) {
      var eventDict = new Dictionary < string,
        object > {
          {
            MoEConstants.ARGUMENT_EVENT_NAME, eventName
          },
          {
            MoEConstants.ARGUMENT_EVENT_ATTRIBUTES,
            properties.ToDictionary()
          },
          {
            MoEConstants.ARGUMENT_IS_NON_INTERACTIVE_EVENT,
            properties.GetIsNonInteractive()
          }
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            eventDict
          }
        };

      return Json.Serialize(payloadDict);
    }
<<<<<<< HEAD

    public static string GetPushPayload(IDictionary < string, string > payload, string service) {
      Dictionary < string, object > pushPayloadDict = new Dictionary < string, object > {
        {
          MoEConstants.ARGUMENT_PAYLOAD, payload
        },
        {
          MoEConstants.ARGUMENT_SERVICE,
          service
        }
      };

      string pushPayload = Json.Serialize(pushPayloadDict);
      return pushPayload;
    }

    public static string GetPushTokenPayload(string pushToken, string service) {
      Dictionary < string, string > tokenDict = new Dictionary < string, string > {
        {
          MoEConstants.ARGUMENT_TOKEN, pushToken
        },
        {
          MoEConstants.ARGUMENT_SERVICE,
          service
        }
      };

      string pushTokenPayload = Json.Serialize(tokenDict);
      return pushTokenPayload;
    }

    public static PushCampaignData GetPushClickPayload(string payload) {
      Dictionary < string, object > payloadDict = Json.Deserialize(payload) as Dictionary < string, object > ;

      AccountMeta accountMetaData = GetAccountMetaInstance(payloadDict);

      var dataDictionary = payloadDict[MoEConstants.PAYLOAD_DATA] as Dictionary < string,
        object > ;

      PushCampaignData campaignData = new PushCampaignData {
        accountMeta = accountMetaData,
          data = new PushCampaign(dataDictionary),
          platform = GetPlatform(dataDictionary[MoEConstants.PARAM_PLATFORM] as string)
      };

      return campaignData;
    }

    private static Platform GetPlatform(string platform) {
      Platform currentPlatform = default;
      switch (platform.ToLower()) {
      case "ios":
        currentPlatform = Platform.iOS;
        break;
      case "android":
        currentPlatform = Platform.Android;
        break;
      }
      return currentPlatform;
    }

    private static NavigationType GetNavigationType(string type) {
      NavigationType navigationType = default;
      switch (type.ToLower()) {
      case "screen":
        navigationType = NavigationType.Screen;
        break;
      case "deep_linking":
        navigationType = NavigationType.Deeplink;
        break;
      }
      return navigationType;
    }

=======
    
>>>>>>> MOEN-20006-Unity-Multiinstance
    public static string GetContextsPayload(string[] contexts, string appId) {
      Dictionary < string, string[] > contextDict = new Dictionary < string, string[] > {
        {
          MoEConstants.ARGUMENT_CONTEXTS, contexts
        }
      };

      Dictionary < string, object > payloadDict = new Dictionary < string, object > {
        {
          MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
        },
        {
          MoEConstants.PAYLOAD_DATA,
          contextDict
        }
      };

      return Json.Serialize(payloadDict);
    }

<<<<<<< HEAD
    public static InAppSelfHandledCampaignData GetInAppSelfHandledData(string payload) {
      Dictionary < string, object > payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary < string, object > ;

      AccountMeta accountMeta = GetAccountMetaInstance(payloadDictionary);

      Dictionary < string, object > dataPayload = payloadDictionary[MoEConstants.PAYLOAD_DATA] as Dictionary < string, object > ;

      if (isValidSelfHandledInAppPayload(dataPayload)) {

        SelfHandled selfHandled = new SelfHandled(dataPayload[MoEConstants.PARAM_SELF_HANDLED] as Dictionary < string, object > );

        InAppCampaignContext context = new InAppCampaignContext(dataPayload[MoEConstants.PARAM_CAMPAIGN_CONTEXT] as Dictionary < string, object > );

        InAppCampaign campaign = new InAppCampaign(dataPayload[MoEConstants.PARAM_CAMPAIGN_ID] as string, dataPayload[MoEConstants.PARAM_CAMPAIGN_NAME] as string, context);

        InAppSelfHandledCampaignData inAppData = new InAppSelfHandledCampaignData(accountMeta, campaign, GetPlatform(dataPayload[MoEConstants.PARAM_PLATFORM] as string), selfHandled);

        return inAppData;

      }

      return null;

    }

    private static Boolean isValidInAppPayload(Dictionary < string, object > payload) {
      if (payload.ContainsKey(MoEConstants.PARAM_CAMPAIGN_ID) && payload.ContainsKey(MoEConstants.PARAM_CAMPAIGN_NAME) && payload.ContainsKey(MoEConstants.PARAM_CAMPAIGN_CONTEXT) && payload.ContainsKey(MoEConstants.PARAM_PLATFORM)) {
        return true;
      }

      return false;
    }

    private static Boolean isValidSelfHandledInAppPayload(Dictionary < string, object > payload) {
      if (isValidInAppPayload(payload) && payload.ContainsKey(MoEConstants.PARAM_SELF_HANDLED)) {
        return true;
      }

      return false;
    }

    public static InAppClickData GetInAppClickData(string payload) {
      Dictionary < string, object > payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary < string, object > ;

      AccountMeta accountMeta = GetAccountMetaInstance(payloadDictionary);

      Dictionary < string, object > dataPayload = payloadDictionary[MoEConstants.PAYLOAD_DATA] as Dictionary < string, object > ;

      if (isValidInAppPayload(dataPayload)) {

        InAppCampaignContext context = new InAppCampaignContext(dataPayload[MoEConstants.PARAM_CAMPAIGN_CONTEXT] as Dictionary < string, object > );

        InAppCampaign campaign = new InAppCampaign(dataPayload[MoEConstants.PARAM_CAMPAIGN_ID] as string, dataPayload[MoEConstants.PARAM_CAMPAIGN_NAME] as string, context);

        InAppClickAction action = new InAppClickAction();

        // Navigation Action Info
        if (dataPayload.ContainsKey(MoEConstants.PARAM_NAVIGATION)) {
          var navigationDictionary = dataPayload[MoEConstants.PARAM_NAVIGATION] as Dictionary < string,
            object > ;

          NavigationAction navigationAction = new NavigationAction() {
            navigationType = GetNavigationType(navigationDictionary[MoEConstants.PARAM_NAVIGATION_TYPE] as string),
              url = navigationDictionary[MoEConstants.PARAM_NAVIGATION_URL] as string,
          };

          if (navigationDictionary.ContainsKey(MoEConstants.PARAM_KEY_VALUE_PAIR)) {
            navigationAction.keyValuePairs = navigationDictionary[MoEConstants.PARAM_KEY_VALUE_PAIR] as Dictionary < string, object > ;
          }

          navigationAction.actionType = ActionType.Navigation;
          action = navigationAction;
        }

        /// Custom Action Info
        if (dataPayload.ContainsKey(MoEConstants.PARAM_CUSTOM_ACTION)) {

          CustomAction custom = new CustomAction() {
            keyValuePairs = dataPayload[MoEConstants.PARAM_CUSTOM_ACTION] as Dictionary < string, object >
          };

          custom.actionType = ActionType.Custom;
          action = custom;
        }

        InAppClickData inAppData = new InAppClickData(accountMeta, campaign, GetPlatform(dataPayload[MoEConstants.PARAM_PLATFORM] as string), action);

        return inAppData;
      };

      return null;
    }

    private static AccountMeta GetAccountMetaInstance(Dictionary < string, object > payloadDictionary) {
      Dictionary < string, object > accountPayload = payloadDictionary[MoEConstants.PAYLOAD_ACCOUNT_META] as Dictionary < string, object > ;
      var accountMeta = new AccountMeta(accountPayload[MoEConstants.PAYLOAD_APPID] as string);
      return accountMeta;
    }

    public static InAppData GetInAppCampaignFromPayload(string payload) {

      Dictionary < string, object > payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary < string, object > ;

      Dictionary < string, object > dataPayload = payloadDictionary[MoEConstants.PAYLOAD_DATA] as Dictionary < string, object > ;

      if (isValidInAppPayload(dataPayload)) {

        var accountMeta = GetAccountMetaInstance(payloadDictionary);

        var campaignContextPayload = dataPayload[MoEConstants.PARAM_CAMPAIGN_CONTEXT] as Dictionary < string,
          object > ;

        InAppCampaignContext context = new InAppCampaignContext(campaignContextPayload);

        InAppCampaign campaign = new InAppCampaign(dataPayload[MoEConstants.PARAM_CAMPAIGN_ID] as string, dataPayload[MoEConstants.PARAM_CAMPAIGN_NAME] as string, context);

        InAppData inappData = new InAppData(accountMeta, campaign, GetPlatform(dataPayload[MoEConstants.PARAM_PLATFORM] as string));

        return inappData;
      }

      return null;
=======
    public static string GetOptOutTrackingPayload(string type, bool shouldOptOut, string appId) {
      var optOutTrackingDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.ARGUMENT_TYPE, type
          }, {
            MoEConstants.PARAM_STATE,
            shouldOptOut
          }
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            optOutTrackingDictionary
          }
        };

      return Json.Serialize(payloadDict);
    }

    public static string GetSdkStatePayload(bool isSdkEnabled, string appId) {
      var sdkStatusDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.FEATURE_STATUS_IS_SDK_ENABLED, isSdkEnabled
          },
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            sdkStatusDictionary
          }
        };

      return Json.Serialize(sdkStatusDictionary);
    }

    public static string GetAndroidIdTrackingStatus(bool isEnabled) {
      var sdkStatusDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.KEY_ANDROID_ID_TRACKING, isEnabled
          },
        };

      return Json.Serialize(sdkStatusDictionary);
    }

    public static string GetAdIdTrackingStatus(bool isEnabled) {
      var sdkStatusDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.KEY_AD_ID_TRACKING, isEnabled
          },
        };

      return Json.Serialize(sdkStatusDictionary);
    }

    public static string GetAccountPayload(string appId) {
      var payloadDict = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          }
        };

      return Json.Serialize(payloadDict);
>>>>>>> MOEN-20006-Unity-Multiinstance
    }

    public static string GetSelfHandledPayload(InAppSelfHandledCampaignData inAppData, string type) {
      var accountMetaDictionary = GetAppIdPayload(inAppData.accountMeta.appId);

      var selfHandledDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.ARGUMENT_PAYLOAD, inAppData.selfHandled.payload
          }, {
            MoEConstants.ARGUMENT_DISMISS_INTERVAL,
            inAppData.selfHandled.dismissInterval
          }, {
            MoEConstants.ARGUMENT_IS_CANCELLABLE,
            inAppData.selfHandled.isCancellable
          }
        };

      var inAppCampaignDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.ARGUMENT_CAMPAIGN_ID, inAppData.campaignData.campaignId
          }, {
            MoEConstants.ARGUMENT_CAMPAIGN_NAME,
            inAppData.campaignData.campaignName
          }, {
            MoEConstants.ARGUMENT_SELF_HANDLED,
            selfHandledDictionary
          }, {
            MoEConstants.ARGUMENT_CAMPAIGN_CONTEXT,
            inAppData.campaignData.campaignContext.attributes
          }, {
            MoEConstants.ARGUMENT_TYPE,
            type
          }, {
            MoEConstants.ARGUEMENT_PLATFORM,
            inAppData.platform
          }
        };

      var impressionDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, accountMetaDictionary
          }, {
            MoEConstants.PAYLOAD_DATA,
            inAppCampaignDictionary
          }
        };
      return Json.Serialize(impressionDictionary);
    }
<<<<<<< HEAD

    public static string GetOptOutTrackingPayload(string type, bool shouldOptOut, string appId) {
      var optOutTrackingDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.ARGUMENT_TYPE, type
          }, {
            MoEConstants.PARAM_STATE,
            shouldOptOut
          }
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            optOutTrackingDictionary
          }
        };

      return Json.Serialize(payloadDict);
    }

    public static string GetSdkStatePayload(bool isSdkEnabled, string appId) {
      var sdkStatusDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.FEATURE_STATUS_IS_SDK_ENABLED, isSdkEnabled
          },
        };

      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            sdkStatusDictionary
          }
        };

      return Json.Serialize(sdkStatusDictionary);
    }

    public static PushToken GetPushTokenFromPayload(string payload) {
      Dictionary < string, object > payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary < string, object > ;

      return new PushToken(
        GetPlatform(payloadDictionary[MoEConstants.PARAM_PLATFORM] as string),
        payloadDictionary[MoEConstants.PARAM_PUSH_TOKEN] as string,
        (PushService) Enum.Parse(typeof (PushService), payloadDictionary[MoEConstants.PARAM_PUSH_SERVICE] as string)
      );
    }

    public static string GetAndroidIdTrackingStatus(bool isEnabled) {
      var sdkStatusDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.KEY_ANDROID_ID_TRACKING, isEnabled
          },
        };

      return Json.Serialize(sdkStatusDictionary);
    }

    public static string GetAdIdTrackingStatus(bool isEnabled) {
      var sdkStatusDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.KEY_AD_ID_TRACKING, isEnabled
          },
        };

      return Json.Serialize(sdkStatusDictionary);
    }

    public static string GetAccountPayload(string appId) {
      var payloadDict = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          }
        };

      return Json.Serialize(payloadDict);
    }
=======
>>>>>>> MOEN-20006-Unity-Multiinstance
  }
}