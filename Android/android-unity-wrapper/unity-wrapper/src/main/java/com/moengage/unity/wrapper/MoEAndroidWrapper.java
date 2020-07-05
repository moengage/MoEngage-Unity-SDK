/*
 * Copyright (c) 2014-2020 MoEngage Inc.
 *
 * All rights reserved.
 *
 *  Use of source code or binaries contained within MoEngage SDK is permitted only to enable use
 * of the MoEngage platform by customers of MoEngage.
 *  Modification of source code and inclusion in mobile apps is explicitly allowed provided that
 * all other conditions are met.
 *  Neither the name of MoEngage nor the names of its contributors may be used to endorse or
 * promote products derived from this software without specific prior written permission.
 *  Redistribution of source code or binaries is disallowed except with specific prior written
 * permission. Any such redistribution must retain the above copyright notice, this list of
 * conditions and the following disclaimer.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
 *  IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
 * AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
 *  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
 * OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

package com.moengage.unity.wrapper;

import android.content.Context;
import android.os.Bundle;
import com.moe.pushlibrary.MoEHelper;
import com.moe.pushlibrary.models.GeoLocation;
import com.moengage.core.Logger;
import com.moengage.core.MoEUtils;
import com.moengage.core.Properties;
import com.moengage.core.SdkConfig;
import com.moengage.core.model.AppStatus;
import com.moengage.core.utils.ApiUtility;
import com.moengage.inapp.MoEInAppHelper;
import com.moengage.inapp.model.MoEInAppCampaign;
import com.moengage.push.PushManager;
import com.moengage.pushbase.MoEPushHelper;
import com.unity3d.player.UnityPlayer;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Iterator;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

/**
 * Bridge between Unity and Android Native
 *
 * @author Umang Chamaria
 * Date: 26/06/20
 */
public class MoEAndroidWrapper {

  private static final String TAG = Constants.MODULE_TAG + "MoEAndroidWrapper";

  private MoEAndroidWrapper() {

  }

  private static MoEAndroidWrapper instance = new MoEAndroidWrapper();

  public static MoEAndroidWrapper getInstance() {
    return instance;
  }

  private Context context;
  private String gameObjectName;
  private boolean isInitialized;
  private final List<Message> messageQueue =
      Collections.synchronizedList(new ArrayList<Message>());

  void setContext(Context context){
    this.context = context;
  }

  public void initialize(String initializePayload) {
    try {
      Logger.v(TAG + " initialize() : Initialization payload: " + initializePayload);
      JSONObject initializationJson = new JSONObject(initializePayload);
      gameObjectName = initializationJson.getString(ARGUMENT_GAME_OBJECT);
      if (MoEUtils.isEmptyString(gameObjectName)) {
        Logger.e(TAG + " initialize() : Game object name is empty cannot pass callbacks");
        return;
      }
      isInitialized = true;
      flushPendingMessagesIfAny();
    } catch (Exception e) {
      Logger.e(TAG + " initialise() : ", e);
    }
  }

  public void trackEvent(String eventPayload) {
    try {
      Logger.v(TAG + " trackEvent() : Event Payload: " + eventPayload);
      if (MoEUtils.isEmptyString(eventPayload)) {
        Logger.e(TAG + " trackEvent() : Payload is null or empty cannot process further.");
        return;
      }
      JSONObject eventJson = new JSONObject(eventPayload);
      String eventName = eventJson.getString(ARGUMENT_EVENT_NAME);
      if (MoEUtils.isEmptyString(eventName)) {
        Logger.e(TAG + " trackEvent() : Event name cannot be null or empty.");
        return;
      }
      if (context == null) {
        Logger.e(TAG + " trackEvent() : Context is null cannot process further.");
        return;
      }
      Properties properties = new Properties();
      JSONObject attributeJson = eventJson.optJSONObject(ARGUMENT_EVENT_ATTRIBUTES);
      if (attributeJson == null || attributeJson.length() == 0) {
        MoEHelper.getInstance(context).trackEvent(eventName, properties);
        return;
      }
      appendGeneralAttributes(attributeJson.optJSONObject(ARGUMENT_GENERAL_EVENT_ATTRIBUTES),
          properties);
      appendDateAttributes(attributeJson.optJSONObject(ARGUMENT_TIMESTAMP_EVENT_ATTRIBUTES),
          properties);
      appendLocationAttributes(attributeJson.optJSONObject(ARGUMENT_LOCATION_EVENT_ATTRIBUTES),
          properties);
      if (attributeJson.optBoolean(ARGUMENT_IS_NON_INTERACTIVE_EVENT, false)) {
        properties.setNonInteractive();
      }
      MoEHelper.getInstance(context).trackEvent(eventName, properties);
    } catch (Exception e) {
      Logger.e(TAG + " trackEvent() : ", e);
    }
  }

  private void appendGeneralAttributes(JSONObject attributesJson, Properties properties) {
    try {
      if (attributesJson == null || attributesJson.length() == 0) {
        Logger.v(TAG + " appendGeneralAttributes() : No general attributes to track.");
        return;
      }
      Iterator<String> keys = attributesJson.keys();
      while (keys.hasNext()) {
        String key = keys.next();
        if (!MoEUtils.isEmptyString(key)) {
          properties.addAttribute(key, attributesJson.get(key));
        }
      }
    } catch (Exception e) {
      Logger.e(TAG + " appendGeneralAttributes() : ", e);
    }
  }

  private void appendDateAttributes(JSONObject attributesJson, Properties properties) {
    try {
      if (attributesJson == null || attributesJson.length() == 0) {
        Logger.v(TAG + " appendGeneralAttributes() : No general attributes to track.");
        return;
      }
      Iterator<String> keys = attributesJson.keys();
      while (keys.hasNext()) {
        String key = keys.next();
        if (!MoEUtils.isEmptyString(key)) {
          properties.addDateIso(key, attributesJson.getString(key));
        }
      }
    } catch (Exception e) {
      Logger.e(TAG + " appendGeneralAttributes() : ", e);
    }
  }

  private void appendLocationAttributes(JSONObject attributesJson, Properties properties) {
    try {
      if (attributesJson == null || attributesJson.length() == 0) {
        Logger.v(TAG + " appendGeneralAttributes() : No general attributes to track.");
        return;
      }
      Iterator<String> keys = attributesJson.keys();
      while (keys.hasNext()) {
        String key = keys.next();
        if (!MoEUtils.isEmptyString(key)) {
          JSONObject locationJson = attributesJson.getJSONObject(key);
          properties.addAttribute(key, new GeoLocation(locationJson.getDouble(ARGUMENT_LATITUDE),
              locationJson.getDouble(ARGUMENT_LONGITUDE)));
        }
      }
    } catch (Exception e) {
      Logger.e(TAG + " appendGeneralAttributes() : ", e);
    }
  }

  public void passPushPayload(String pushPayload) {
    try {
      Logger.v(TAG + " passPushPayload() : Push Payload: " + pushPayload);
      if (MoEUtils.isEmptyString(pushPayload)) {
        Logger.e(TAG + " trackEvent() : Payload is null or empty cannot process further.");
        return;
      }
      JSONObject payloadJson = new JSONObject(pushPayload);
      Logger.v(TAG + " passPushPayload() : Payload Json: " + payloadJson);
      JSONObject payload = payloadJson.getJSONObject(ARGUMENT_PUSH_PAYLOAD);
      if (payload.length() == 0) {
        Logger.e(TAG + " passPushPayload() : Push payload is either null or empty");
        return;
      }
      Bundle payloadBundle = MoEUtils.jsonToBundle(payload);
      if (payloadBundle == null) {
        Logger.e(TAG + " passPushPayload() : Could not parse payload json.");
        return;
      }
      if (context == null) {
        Logger.e(TAG + " passPushPayload() : Context is null cannot process further.");
        return;
      }
      MoEPushHelper.getInstance().handlePushPayload(context, payloadBundle);
    } catch (Exception e) {
      Logger.e(TAG + " passPushPayload() : ", e);
    }
  }

  public void passPushToken(String tokenPayload) {
    try {
      Logger.v(TAG + " passPushToken() : Token Payload: " + tokenPayload);
      if (MoEUtils.isEmptyString(tokenPayload)) {
        Logger.e(TAG + " passPushToken() : Payload is null or empty cannot process further.");
        return;
      }
      JSONObject tokenJson = new JSONObject(tokenPayload);
      String token = tokenJson.getString(ARGUMENT_FCM_TOKEN);
      if (context == null) {
        Logger.e(TAG + " passPushToken() : Context is null cannot process further.");
        return;
      }
      PushManager.getInstance().refreshToken(context, token);
    } catch (Exception e) {
      Logger.e(TAG + " passPushToken() : ", e);
    }
  }

  public void getSelfHandledInApp() {
    try {
      Logger.v(TAG + " getSelfHandledInApp() : Will try to fetch self-handled in-app");
      if (context == null) {
        Logger.e(TAG + " getSelfHandledInApp() : Context is null cannot process further.");
        return;
      }
      MoEInAppHelper.getInstance().getSelfHandledInApp(context);
    } catch (Exception e) {
      Logger.e(TAG + " getSelfHandledInApp() : ", e);
    }
  }

  public void showInApp() {
    try {
      Logger.v(TAG + " showInApp() : Will try to show in-app");
      if (context == null) {
        Logger.e(TAG + " showInApp() : Context is null cannot process further.");
        return;
      }
      MoEInAppHelper.getInstance().showInApp(context);
    } catch (Exception e) {
      Logger.e(TAG + " showInApp() : ", e);
    }
  }

  public void logout() {
    try {
      Logger.v(TAG + " logout() : Will try to logout user.");
      if (context == null) {
        Logger.e(TAG + " logout() : Context is null cannot process further.");
        return;
      }
      MoEHelper.getInstance(context).logoutUser();
    } catch (Exception e) {
      Logger.e(TAG + " logout() : ", e);
    }
  }

  public void setAlias(String aliasPayload) {
    try {
      Logger.v(TAG + " setAlias() : Alias Payload: " + aliasPayload);
      JSONObject aliasJson = new JSONObject(aliasPayload);
      String alias = aliasJson.getString(ARGUMENT_ALIAS);
      if (MoEUtils.isEmptyString(alias)) {
        Logger.e(TAG + " setAlias() : Alias cannot be null or empty");
        return;
      }
      if (context == null) {
        Logger.e(TAG + " setAlias() : Context is null cannot process further.");
        return;
      }
      MoEHelper.getInstance(context).setAlias(alias);
    } catch (Exception e) {
      Logger.e(TAG + " setAlias() : ", e);
    }
  }

  public void setAppStatus(String appStatusPayload) {
    try {
      Logger.v(TAG + " setAppStatus() : App status payload: " + appStatusPayload);
      JSONObject appStatusJson = new JSONObject(appStatusPayload);
      String appStatus = appStatusJson.getString(ARGUMENT_APP_STATUS);
      if (MoEUtils.isEmptyString(appStatus)) {
        Logger.e(TAG + " setAppStatus() : App status cannot be null or empty");
        return;
      }
      if (context == null) {
        Logger.e(TAG + " setAppStatus() : Context is null cannot process further.");
        return;
      }
      MoEHelper.getInstance(context).setAppStatus(AppStatus.valueOf(appStatus.toUpperCase()));
    } catch (Exception e) {
      Logger.e(TAG + " setAppStatus() : ", e);
    }
  }

  public void setUserAttribute(String userAttributePayload) {
    try {
      Logger.v(TAG + " setUserAttribute() : User Attribute payload: " + userAttributePayload);
      JSONObject attributeJson = new JSONObject(userAttributePayload);
      String attributeName = attributeJson.getString(ARGUMENT_USER_ATTRIBUTE_NAME);
      if (MoEUtils.isEmptyString(attributeName)) {
        Logger.e(TAG + " setUserAttribute() : Attribute name cannot be null or empty.");
        return;
      }
      String attributeType = attributeJson.getString(ARGUMENT_TYPE);
      if (MoEUtils.isEmptyString(attributeType)) {
        Logger.e(TAG + " setUserAttribute() : Attribute type cannot be null or empty");
        return;
      }
      if (context == null) {
        Logger.e(TAG + " setUserAttribute() : Cannot proceed further context is null.");
        return;
      }
      switch (attributeType) {
        case Constants.ATTRIBUTE_TYPE_GENERAL:
          trackGeneralAttribute(attributeJson, attributeName, context);
          break;
        case Constants.ATTRIBUTE_TYPE_TIMESTAMP:
          trackTimeStampAttribute(attributeJson, attributeName, context);
          break;
        case Constants.ATTRIBUTE_TYPE_LOCATION:
          trackLocationAttribute(attributeJson, attributeName, context);
          break;
        default:
          Logger.e(TAG + " setUserAttribute() : Invalid attribute type: " + attributeType);
      }
    } catch (Exception e) {
      Logger.e(TAG + " setUserAttribute() : ", e);
    }
  }

  private void trackTimeStampAttribute(JSONObject attributeJson, String attributeName,
      Context context) throws JSONException {
    String attributeValue = attributeJson.getString(ARGUMENT_USER_ATTRIBUTE_VALUE);
    if (MoEUtils.isEmptyString(attributeValue)) {
      Logger.e(TAG + " setUserAttribute() : Attribute value should not be null or empty.");
      return;
    }
    MoEHelper.getInstance(context).setUserAttributeISODate(attributeName, attributeValue);
  }

  private void trackLocationAttribute(JSONObject attributeJson, String attributeName,
      Context context) throws JSONException {
    if (!attributeJson.has(ARGUMENT_LOCATION_ATTRIBUTE)) {
      Logger.e(TAG
          + " setUserAttribute() : Cannot track location attribute without "
          + ARGUMENT_LOCATION_ATTRIBUTE);
      return;
    }
    JSONObject locationJson = attributeJson.getJSONObject(ARGUMENT_LOCATION_ATTRIBUTE);
    MoEHelper.getInstance(context)
        .setUserAttribute(attributeName,
            new GeoLocation(locationJson.getDouble(ARGUMENT_LATITUDE),
                locationJson.getDouble(ARGUMENT_LONGITUDE)));
  }

  private void trackGeneralAttribute(JSONObject attributeJson, String attributeName,
      Context context) throws JSONException {
    Object attributeValue = attributeJson.get(ARGUMENT_USER_ATTRIBUTE_VALUE);
    if (attributeValue == null) {
      Logger.e(TAG + " setUserAttribute() : Attribute value should not be null");
      return;
    }
    if (attributeValue instanceof Integer) {
      MoEHelper.getInstance(context).setUserAttribute(attributeName, (int) attributeValue);
    } else if (attributeValue instanceof Double) {
      MoEHelper.getInstance(context).setUserAttribute(attributeName, (double) attributeValue);
    } else if (attributeValue instanceof Float) {
      MoEHelper.getInstance(context).setUserAttribute(attributeName, (float) attributeValue);
    } else if (attributeValue instanceof Boolean) {
      MoEHelper.getInstance(context)
          .setUserAttribute(attributeName, (boolean) attributeValue);
    } else if (attributeValue instanceof Long) {
      MoEHelper.getInstance(context).setUserAttribute(attributeName, (long) attributeValue);
    } else if (attributeValue instanceof String) {
      MoEHelper.getInstance(context).setUserAttribute(attributeName,
          String.valueOf(attributeValue));
    } else {
      Logger.e(TAG + " trackGeneralAttribute() : Not a supported datatype");
    }
  }

  public void setAppContext(String contextPayload) {
    try {
      Logger.v(TAG + " setContext() : Context Payload: " + contextPayload);
      JSONObject contextJson = new JSONObject(contextPayload);
      JSONArray contextArray = contextJson.getJSONArray(ARGUMENT_CONTEXT);
      List<String> contextList = ApiUtility.jsonArrayToStringList(contextArray);
      if (context != null && contextList != null && !contextList.isEmpty()) {
        MoEHelper.getInstance(context).setAppContext(contextList);
      }
    } catch (Exception e) {
      Logger.e(TAG + " setAppContext() : ", e);
    }
  }

  public void resetContext(){
    try {
      if (context != null) {
        MoEHelper.getInstance(context).resetAppContext();
      }
    } catch (Exception e) {
      Logger.e( TAG + " resetContext() : ", e);
    }
  }

  public void selfHandledShown(String selfHandledPayload){
    try{
     Logger.v(TAG + " selfHandledShown() : Campaign payload: " + selfHandledPayload);
     JSONObject campaignJson = new JSONObject(selfHandledPayload);
      MoEInAppCampaign campaign = Utils.jsonToInAppCampaign(campaignJson);
      if (context == null){
        Logger.e( TAG + " selfHandledShown() : Cannot proceed further context is null.");
        return;
      }
      MoEInAppHelper.getInstance().selfHandledShown(context, campaign);
    }catch(Exception e){
      Logger.e(TAG + " selfHandledShown() : ", e);
    }
  }

  public void selfHandledClicked(String selfHandledPayload){
    try{
      Logger.v(TAG + " selfHandledClicked() : Campaign payload: " + selfHandledPayload);
      JSONObject campaignJson = new JSONObject(selfHandledPayload);
      MoEInAppCampaign campaign = Utils.jsonToInAppCampaign(campaignJson);
      if (context == null){
        Logger.e( TAG + " selfHandledClicked() : Cannot proceed further context is null.");
        return;
      }
      MoEInAppHelper.getInstance().selfHandledClicked(context, campaign);
    }catch(Exception e){
      Logger.e(TAG + " selfHandledClicked() : ", e);
    }
  }

  public void selfHandledDismissed(String selfHandledPayload){
    try{
     Logger.v(TAG + " selfHandledDismissed() : Campaign payload: " + selfHandledPayload);
      JSONObject campaignJson = new JSONObject(selfHandledPayload);
      MoEInAppCampaign campaign = Utils.jsonToInAppCampaign(campaignJson);
      if (context == null){
        Logger.e( TAG + " selfHandledDismissed() : Cannot proceed further context is null.");
        return;
      }
      MoEInAppHelper.getInstance().selfHandledDismissed(context, campaign);
    }catch(Exception e){
      Logger.e(TAG + " selfHandledDismissed() : ", e);
    }
  }

  public void enableSDKLogs() {
    SdkConfig.getConfig().logLevel = Logger.VERBOSE;
    Logger.v(TAG + " enableSDKLogs(): Logging enabled");
  }

  void sendOrQueueCallback(String methodName, JSONObject payload) {
    try {
      Logger.v(
          TAG + " sendOrQueueCallback() : Method Name: " + methodName + " Payload: " + payload);
      payload.put(Constants.PARAM_PLATFORM, Constants.PLATFORM_NAME);
      if (isInitialized){
        sendUnityMessage(new Message(methodName, payload.toString()));
      }else {
        messageQueue.add(new Message(methodName, payload.toString()));
      }
    } catch (Exception e) {
      Logger.e(TAG + " sendOrQueueCallback() : ", e);
    }
  }

  private void flushPendingMessagesIfAny() {
    for (Message message : messageQueue) {
      sendUnityMessage(message);
    }
    messageQueue.clear();
  }

  private void sendUnityMessage(Message message) {
    try {
      if (MoEUtils.isEmptyString(gameObjectName)) {
        Logger.e(TAG + " sendUnityMessage() : Game object name is null or empty cannot send "
            + "message.");
        return;
      }
      Logger.v(TAG + " sendUnityMessage() : Sending Message: " + message);
      UnityPlayer.UnitySendMessage(gameObjectName, message.methodName, message.payload);
    } catch (Exception e) {
      Logger.e(TAG + " sendUnityMessage() : ", e);
    }
  }

  private static final String ARGUMENT_EVENT_NAME = "eventName";
  private static final String ARGUMENT_EVENT_ATTRIBUTES = "eventAttributes";
  private static final String ARGUMENT_GENERAL_EVENT_ATTRIBUTES = "generalAttributes";
  private static final String ARGUMENT_LOCATION_EVENT_ATTRIBUTES = "locationAttributes";
  private static final String ARGUMENT_TIMESTAMP_EVENT_ATTRIBUTES = "dateTimeAttributes";
  private static final String ARGUMENT_IS_NON_INTERACTIVE_EVENT = "isNonInteractive";
  private static final String ARGUMENT_LATITUDE = "latitude";
  private static final String ARGUMENT_LONGITUDE = "longitude";
  private static final String ARGUMENT_USER_ATTRIBUTE_NAME = "attributeName";
  private static final String ARGUMENT_USER_ATTRIBUTE_VALUE = "attributeValue";
  private static final String ARGUMENT_LOCATION_ATTRIBUTE = "locationAttribute";
  private static final String ARGUMENT_TYPE = "type";

  private static final String ARGUMENT_GAME_OBJECT = "gameObjectName";
  private static final String ARGUMENT_APP_STATUS = "appStatus";
  private static final String ARGUMENT_ALIAS = "alias";
  private static final String ARGUMENT_FCM_TOKEN = "fcmToken";
  private static final String ARGUMENT_PUSH_PAYLOAD = "pushPayload";
  private static final String ARGUMENT_CONTEXT = "contexts";
}
