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
   /// Class responsible to construct InApp Model

   public class MoEInAppParser {
     
     public static InAppSelfHandledCampaignData GetInAppSelfHandledData(string payload) {
       Dictionary < string, object > payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary < string, object > ;

       AccountMeta accountMeta = MoEParser.GetAccountMetaInstance(payloadDictionary);

       Dictionary < string, object > dataPayload = payloadDictionary[MoEConstants.PAYLOAD_DATA] as Dictionary < string, object > ;

       if (isValidSelfHandledInAppPayload(dataPayload)) {

         SelfHandled selfHandled = GetSelfHandled(dataPayload[MoEConstants.PARAM_SELF_HANDLED] as Dictionary < string, object > );

         InAppCampaignContext context = GetInAppCampaignContext(dataPayload[MoEConstants.PARAM_CAMPAIGN_CONTEXT] as Dictionary < string, object > );

         InAppCampaign campaign = GetInAppCampaign(dataPayload[MoEConstants.PARAM_CAMPAIGN_ID] as string, dataPayload[MoEConstants.PARAM_CAMPAIGN_NAME] as string, context);

         InAppSelfHandledCampaignData inAppData = GetInAppSelfHandledData(accountMeta, campaign, MoEParser.GetPlatform(dataPayload[MoEConstants.PARAM_PLATFORM] as string), selfHandled);

         return inAppData;

       }

       return null;

     }

     private static NavigationType GetNavigationType(string type) {
       NavigationType navigationType =
         default;
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

     public static InAppClickData GetInAppClickData(string payload) {
       Dictionary < string, object > payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary < string, object > ;

       AccountMeta accountMeta = MoEParser.GetAccountMetaInstance(payloadDictionary);

       Dictionary < string, object > dataPayload = payloadDictionary[MoEConstants.PAYLOAD_DATA] as Dictionary < string, object > ;

       if (isValidInAppPayload(dataPayload)) {

         InAppCampaignContext context = GetInAppCampaignContext(dataPayload[MoEConstants.PARAM_CAMPAIGN_CONTEXT] as Dictionary < string, object > );

         InAppCampaign campaign = GetInAppCampaign(dataPayload[MoEConstants.PARAM_CAMPAIGN_ID] as string, dataPayload[MoEConstants.PARAM_CAMPAIGN_NAME] as string, context);

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

         InAppClickData inAppData = new InAppClickData {
           accountMeta = accountMeta,
             campaignData = campaign,
             platform = MoEParser.GetPlatform(dataPayload[MoEConstants.PARAM_PLATFORM] as string),
             action = action
         };

         return inAppData;
       };

       return null;
     }

     public static InAppData GetInAppCampaignFromPayload(string payload) {

       Dictionary < string, object > payloadDictionary = MoEMiniJSON.Json.Deserialize(payload) as Dictionary < string, object > ;

       Dictionary < string, object > dataPayload = payloadDictionary[MoEConstants.PAYLOAD_DATA] as Dictionary < string, object > ;

       if (isValidInAppPayload(dataPayload)) {

         var accountMeta = MoEParser.GetAccountMetaInstance(payloadDictionary);

         InAppCampaignContext context = GetInAppCampaignContext(dataPayload[MoEConstants.PARAM_CAMPAIGN_CONTEXT] as Dictionary < string, object > );

         InAppCampaign campaign = GetInAppCampaign(dataPayload[MoEConstants.PARAM_CAMPAIGN_ID] as string, dataPayload[MoEConstants.PARAM_CAMPAIGN_NAME] as string, context);

         InAppData inappData = GetInAppData(accountMeta, campaign, MoEParser.GetPlatform(dataPayload[MoEConstants.PARAM_PLATFORM] as string));

         return inappData;
       }

       return null;
     }

    /// InApp Model
     private static InAppData GetInAppData(AccountMeta accountMeta, InAppCampaign campaign, Platform platform) {
       InAppData inappData = new InAppData {
         accountMeta = accountMeta,
           campaignData = campaign,
           platform = platform
       };
       return inappData;
     }

     private static InAppCampaign GetInAppCampaign(string campaignId, string campaignName, InAppCampaignContext context) {
       InAppCampaign campaign = new InAppCampaign {
         campaignId = campaignId,
           campaignName = campaignName,
           campaignContext = context
       };
       return campaign;
     }

     private static InAppCampaignContext GetInAppCampaignContext(Dictionary < string, object > campaignContextPayload) {
       InAppCampaignContext context = new InAppCampaignContext();
       if (campaignContextPayload.ContainsKey(MoEConstants.PAYLOAD_INAPP_FORMATTED_CID)) {
         context.formattedCampaignId = campaignContextPayload[MoEConstants.PAYLOAD_INAPP_FORMATTED_CID] as string;
       }
       context.attributes = campaignContextPayload;
       return context;
     }

     private static InAppSelfHandledCampaignData GetInAppSelfHandledData(AccountMeta accountMeta, InAppCampaign campaignData, Platform platform, SelfHandled selfHandledData) {
       InAppSelfHandledCampaignData selfHandled = new InAppSelfHandledCampaignData {
         accountMeta = accountMeta,
           campaignData = campaignData,
           platform = platform,
           selfHandled = selfHandledData
       };
       return selfHandled;
     }

     private static SelfHandled GetSelfHandled(Dictionary < string, object > selfHandledDictionary) {
       SelfHandled selfHandled = new SelfHandled();
       if (selfHandledDictionary.ContainsKey(MoEConstants.PARAM_PAYLOAD)) {
         selfHandled.payload = selfHandledDictionary[MoEConstants.PARAM_PAYLOAD] as string;
       };

       if (selfHandledDictionary.ContainsKey(MoEConstants.PARAM_DISMISS_INTERVAL)) {
         selfHandled.dismissInterval = (long) selfHandledDictionary[MoEConstants.PARAM_DISMISS_INTERVAL];
       }

       if (selfHandledDictionary.ContainsKey(MoEConstants.PARAM_IS_CANCELLABLE)) {
         selfHandled.isCancellable = (bool) selfHandledDictionary[MoEConstants.PARAM_IS_CANCELLABLE];
       }

       return selfHandled;
     }

     /// InApp Validator
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
   }
 }