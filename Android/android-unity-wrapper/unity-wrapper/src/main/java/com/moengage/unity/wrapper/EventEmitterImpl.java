package com.moengage.unity.wrapper;

import androidx.annotation.NonNull;
import com.moengage.core.Logger;
import com.moengage.plugin.base.EventEmitter;
import com.moengage.plugin.base.UtilsKt;
import com.moengage.plugin.base.model.Event;
import com.moengage.plugin.base.model.EventType;
import com.moengage.plugin.base.model.InAppEvent;
import com.moengage.plugin.base.model.PushEvent;
import com.unity3d.player.UnityPlayer;
import java.util.EnumMap;
import org.json.JSONObject;

/**
 * @author Umang Chamaria
 * Date: 2020/09/24
 */
public class EventEmitterImpl implements EventEmitter {

  private static final String TAG = Constants.MODULE_TAG + "EventEmitterImpl";
  private String gameObjectName;

  public EventEmitterImpl(String gameObjectName){
    this.gameObjectName = gameObjectName;
  }

  @Override public void emit(@NonNull Event event) {
    try {
      Logger.v(TAG + " emit() : " + event);
      if (event instanceof InAppEvent) {
        this.emitInAppEvent((InAppEvent) event);
      } else if (event instanceof PushEvent) {
        this.emitPushEvent((PushEvent) event);
      }
    } catch (Exception e) {
      Logger.e(TAG + " emit() : Exception: ", e);
    }
  }

  private void emitInAppEvent(InAppEvent inAppEvent) {
    try {
      Logger.v(TAG + " emitInAppEvent() : " + inAppEvent);
      String eventType = eventMap.get(inAppEvent.getEventType());
      if (eventType == null) return;
      JSONObject campaign = UtilsKt.inAppCampaignToJson(inAppEvent.getInAppCampaign());
      emit(inAppEvent.getEventType(), campaign);
    } catch (Exception e) {
      Logger.e(TAG + " inAppToJSON() : Exception: ", e);
    }
  }

  private void emitPushEvent(PushEvent pushEvent) {
    try {
      Logger.v(TAG + " emitPushEvent() : " + pushEvent);
      String eventType = eventMap.get(pushEvent.getEventType());
      if (eventType == null) return;
      JSONObject payload = UtilsKt.pushPayloadToJson(pushEvent.getPayload());
      emit(pushEvent.getEventType(), payload);
    } catch (Exception e) {
      Logger.e(TAG + " inAppToJSON() : Exception: ", e);
    }
  }

  private void emit(EventType eventType, JSONObject payload) {
    try {
      UnityPlayer.UnitySendMessage(gameObjectName, eventMap.get(eventType), payload.toString());
    } catch (Exception e) {
      Logger.e(TAG + " emit() : ", e);
    }
  }

  private static EnumMap<EventType, String> eventMap = new EnumMap<>(EventType.class);

  static {
    eventMap.put(EventType.PUSH_CLICKED, Constants.METHOD_NAME_PUSH_REDIRECTION);
    eventMap.put(EventType.INAPP_SHOWN, Constants.METHOD_NAME_IN_APP_SHOWN);
    eventMap.put(EventType.INAPP_NAVIGATION, Constants.METHOD_NAME_IN_APP_CLICKED);
    eventMap.put(EventType.INAPP_CLOSED, Constants.METHOD_NAME_IN_APP_CLOSED);
    eventMap.put(EventType.INAPP_CUSTOM_ACTION, Constants.METHOD_NAME_IN_APP_CUSTOM_ACTION);
    eventMap.put(EventType.INAPP_SELF_HANDLED_AVAILABLE, Constants.METHOD_NAME_IN_APP_SELF_HANDLED);
  }
}
