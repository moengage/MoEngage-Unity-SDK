﻿/*
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

namespace MoEngage
{
    static class MoEConstants
    {
        /* ***************************** Method Argument Names ************************************** */
        public const string ARGUMENT_EVENT_NAME = "eventName";
        public const string ARGUMENT_EVENT_ATTRIBUTES = "eventAttributes";
        public const string ARGUMENT_GENERAL_EVENT_ATTRIBUTES = "generalAttributes";
        public const string ARGUMENT_LOCATION_EVENT_ATTRIBUTES = "locationAttributes";
        public const string ARGUMENT_TIMESTAMP_EVENT_ATTRIBUTES = "dateTimeAttributes";
        public const string ARGUMENT_IS_NON_INTERACTIVE_EVENT = "isNonInteractive";
        public const string ARGUMENT_LATITUDE = "latitude";
        public const string ARGUMENT_LONGITUDE = "longitude";
        public const string ARGUMENT_USER_ATTRIBUTE_NAME = "attributeName";
        public const string ARGUMENT_USER_ATTRIBUTE_VALUE = "attributeValue";
        public const string ARGUMENT_USER_ATTRIBUTE_LOCATION_VALUE = "locationAttribute";
        public const string ARGUMENT_TYPE = "type";

        public const string ARGUMENT_GAME_OBJECT = "gameObjectName";
        public const string ARGUMENT_APP_STATUS = "appStatus";
        public const string ARGUMENT_ALIAS = "alias";
        public const string ARGUMENT_TOKEN = "token";
        public const string ARGUMENT_SERVICE = "service";
        public const string ARGUEMENT_PLATFORM = "platform";

        public const string ATTRIBUTE_TYPE_GENERAL = "general";
        public const string ATTRIBUTE_TYPE_TIMESTAMP = "timestamp";
        public const string ATTRIBUTE_TYPE_LOCATION = "location";
        public const string ATTRIBUTE_TYPE_SELF_HANDLED_IMPRESSION = "impression";
        public const string ATTRIBUTE_TYPE_SELF_HANDLED_CLICK = "click";
        public const string ATTRIBUTE_TYPE_SELF_HANDLED_DISMISSED = "dismissed";
        public const string ATTRIBUTE_TYPE_SELF_HANDLED_PRIMARY_CLICKED = "primary_clicked";


        public const string ARGUMENT_CAMPAIGN_NAME = "campaignName";
        public const string ARGUMENT_CAMPAIGN_ID = "campaignId";
        public const string ARGUMENT_CAMPAIGN_CONTEXT = "campaignContext";
        public const string ARGUMENT_SELF_HANDLED = "selfHandled";
        public const string ARGUMENT_PAYLOAD = "payload";
        public const string ARGUMENT_DISMISS_INTERVAL = "dismissInterval";
        public const string ARGUMENT_IS_CANCELLABLE = "isCancellable";

        public const string ARGUMENT_CONTEXTS = "contexts";
        public const string ARGUMENT_OPT_OUT_STATUS = "isOptedOut";

        /* ***************************** User Attribute Names ************************************** */
        public const string USER_ATTRIBUTE_UNIQUE_ID = "USER_ATTRIBUTE_UNIQUE_ID";
        public const string USER_ATTRIBUTE_USER_EMAIL = "USER_ATTRIBUTE_USER_EMAIL";
        public const string USER_ATTRIBUTE_USER_MOBILE = "USER_ATTRIBUTE_USER_MOBILE";
        public const string USER_ATTRIBUTE_USER_NAME = "USER_ATTRIBUTE_USER_NAME";
        public const string USER_ATTRIBUTE_USER_GENDER = "USER_ATTRIBUTE_USER_GENDER";
        public const string USER_ATTRIBUTE_USER_FIRST_NAME = "USER_ATTRIBUTE_USER_FIRST_NAME";
        public const string USER_ATTRIBUTE_USER_LAST_NAME = "USER_ATTRIBUTE_USER_LAST_NAME";
        public const string USER_ATTRIBUTE_USER_BDAY = "USER_ATTRIBUTE_USER_BDAY";
        public const string USER_ATTRIBUTE_USER_LOCATION_IOS = "USER_ATTRIBUTE_USER_LOCATION";
        public const string USER_ATTRIBUTE_USER_LOCATION_ANDROID = "last_known_location";


        /* ***************************** CallBack Payload Keys ************************************** */
        public const string PARAM_PLATFORM = "platform";
        public const string PARAM_CAMPAIGN_NAME = "campaignName";
        public const string PARAM_CAMPAIGN_ID = "campaignId";
        public const string PARAM_CAMPAIGN_CONTEXT = "campaignContext";

        public const string PARAM_NAVIGATION = "navigation";
        public const string PARAM_NAVIGATION_TYPE = "navigationType";
        public const string PARAM_NAVIGATION_URL = "value";
        public const string PARAM_KEY_VALUE_PAIR = "kvPair";

        public const string PARAM_CUSTOM_ACTION = "customAction";

        public const string PARAM_SELF_HANDLED = "selfHandled";
        public const string PARAM_PAYLOAD = "payload";
        public const string PARAM_DISMISS_INTERVAL = "dismissInterval";
        public const string PARAM_IS_CANCELLABLE = "isCancellable";
        public const string PARAM_IS_DEFAULT_ACTION = "isDefaultAction";
        public const string PARAM_CLICKED_ACTION = "clickedAction";

        public const string PARAM_PUSH_TOKEN = "token";
        public const string PARAM_PUSH_SERVICE = "pushService";

        /* ***************************** Opt-out Tracking Keys ************************************** */

        public const string PARAM_STATE = "state";
        public const string PARAM_TYPE_DATA = "data";
        public const string PARAM_TYPE_PUSH = "push";
        public const string PARAM_TYPE_INAPP = "inapp";

        public const string FEATURE_STATUS_IS_SDK_ENABLED = "isSdkEnabled";

        /* ****************************** Push Service Type ***************************************** */

        public const string PUSH_SERVICE_TYPE_FCM = "FCM";

        /* ****************************** Payload Keys ***************************************** */

        public const string PAYLOAD_ACCOUNT_META = "accountMeta";
        public const string PAYLOAD_APPID = "appId";
        public const string PAYLOAD_DATA = "data";
        public const string PAYLOAD_GAME_OBJECT = "gameObjectName";

        public const string PAYLOAD_INAPP_FORMATTED_CID = "cid";

        /* ****************************** Push Permission Keys ***************************************** */
        public const string PARAM_IS_PERMISSION_GRANTED = "isGranted";
        public const string PARAM_IS_PERMISSION_TYPE = "type";
        public const string PARAM_UPDATE_PUSH_PERMISSION_COUNT = "pushOptinInAttemptCount";

        /* ****************************** Device Identifiers Keys ***************************************** */
        public const string KEY_ANDROID_ID = "isAndroidIdTrackingEnabled";
        public const string KEY_AD_ID = "isAdIdTrackingEnabled";
        public const string KEY_DEVICE_ID = "isDeviceIdTrackingEnabled";



        /* ****************************** Init Config Keys ***************************************** */
        public const string KEY_INIT_CONFIG = "initConfig";
        public const string KEY_PUSH_CONFIG = "pushConfig";
        public const string KEY_ANALYTICS_CONFIG = "analyticsConfig";

        /* ****************************** Analytics Config Keys ***************************************** */
        public const string KEY_SHOULD_TRACK_BOOLEAN_AS_NUMBER = "shouldTrackUserAttributeBooleanAsNumber";

        /* ****************************** User Delete Keys ***************************************** */
        public const string PARAM_IS_SUCCESS = "isSuccess";

        /* ****************************** Core-SDK Version ***************************************** */
        public const string KEY_VERSION = "version";

        /* ****************************** Integration Meta ***************************************** */
        public const string KEY_INTEGRATION_META = "integrationMeta";

        /* ****************************** Platform type ***************************************** */
        public const string PARAM_PLATFORM_UNITY = "unity";
    }

    public static class MoEUnityConstants {
        /* ****************************** Plugin Version ***************************************** */
        public const string PLUGIN_VERSION = "4.0.0";
    }
}