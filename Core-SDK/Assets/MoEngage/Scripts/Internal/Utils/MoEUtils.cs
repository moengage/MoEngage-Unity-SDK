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
  /// Class responsible to construct the dict and serialize it inorder to pass it to the native.
  public class MoEUtils {

    public static Dictionary < string, string > GetAppIdPayload(string appId) {
      var payloadDict = new Dictionary < string,
        string > () {
          {
            MoEConstants.PAYLOAD_APPID, appId
          }
        };

      return payloadDict;
    }

    public static string GetInitializePayload(string gameObjectName, string appId, string shouldDeliverCallbackOnForegroundClick) {
      var dataPayload = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_GAME_OBJECT,
              gameObjectName
          }, {
            MoEConstants.KEY_INIT_CONFIG,
            GetInitConfigPayload(shouldDeliverCallbackOnForegroundClick)
          }
        };
      var payloadDict = new Dictionary < string,
        object > {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          },
          {
            MoEConstants.PAYLOAD_DATA,
            dataPayload
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

      return Json.Serialize(payloadDict);
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

    public static string GetPushPermissionResponsePayload(bool isGranted, PermissionType type) {
      var payloadDict = new Dictionary < string,
        object > () {
          {
            MoEConstants.PARAM_IS_PERMISSION_GRANTED, isGranted
          }, {
            MoEConstants.PARAM_IS_PERMISSION_TYPE,
            type.ToString()
          }
        };

      return Json.Serialize(payloadDict);
    }

    public static string GetUpdatePushPermissionRequestCountPayload(string appId, int requestCount) {
      var dataPayload = new Dictionary < string,
        object > () {
          {
            MoEConstants.PARAM_UPDATE_PUSH_PERMISSION_COUNT, requestCount
          }
        };

      var payloadDict = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          }, {
            MoEConstants.PAYLOAD_DATA,
            dataPayload
          }
        };

      return Json.Serialize(payloadDict);
    }

    public static string GetDeviceIdentifiersPayload(string appId, string identifierType, bool state) {
      var dataPayload = new Dictionary < string,
        object > () {
          {
            identifierType,
            state
          }
        };

      var deviceIdentifierDictionary = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          }, {
            MoEConstants.PAYLOAD_DATA,
            dataPayload
          }
        };

      return Json.Serialize(deviceIdentifierDictionary);
    }

    public static string GetPushPayload(string appId, IDictionary < string, string > payload, string service) {
      Dictionary < string, object > dataPayload = new Dictionary < string, object > {
        {
          MoEConstants.ARGUMENT_PAYLOAD, payload
        },
        {
          MoEConstants.ARGUMENT_SERVICE,
          service
        }
      };

      var pushPayload = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          }, {
            MoEConstants.PAYLOAD_DATA,
            dataPayload
          }
        };

      return Json.Serialize(pushPayload);
    }

    public static string GetPushTokenPayload(string appId, string pushToken, string service) {
      Dictionary < string, string > dataPayload = new Dictionary < string, string > {
        {
          MoEConstants.ARGUMENT_TOKEN, pushToken
        },
        {
          MoEConstants.ARGUMENT_SERVICE,
          service
        }
      };

      var pushTokenPayload = new Dictionary < string,
        object > () {
          {
            MoEConstants.PAYLOAD_ACCOUNT_META, GetAppIdPayload(appId)
          }, {
            MoEConstants.PAYLOAD_DATA,
            dataPayload
          }
        };

      return Json.Serialize(pushTokenPayload);
    }

    private static Dictionary < string, object > GetInitConfigPayload(string shouldDeliverCallbackOnForegroundClick) {
      var payloadDict = new Dictionary < string,
        object > () {
          {
            MoEConstants.KEY_PUSH_CONFIG,
            parseShouldDeliverCallbackOnForegroundClick(shouldDeliverCallbackOnForegroundClick)
          }
        };

      return payloadDict;
    }

    private static bool parseShouldDeliverCallbackOnForegroundClick(string shouldDeliverCallbackOnForegroundClick) {
       return shouldDeliverCallbackOnForegroundClick.ToLower() == "true";
      }

  }
}