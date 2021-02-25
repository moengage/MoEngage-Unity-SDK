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

import androidx.annotation.NonNull;
import com.moengage.core.internal.logger.Logger;
import com.moengage.plugin.base.EventEmitter;
import com.moengage.plugin.base.UtilsKt;
import com.moengage.plugin.base.model.Event;
import com.moengage.plugin.base.model.EventType;
import com.moengage.plugin.base.model.InAppEvent;
import com.moengage.plugin.base.model.PushEvent;
import com.moengage.plugin.base.model.TokenEvent;
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

  public EventEmitterImpl(String gameObjectName) {
    this.gameObjectName = gameObjectName;
  }

  @Override public void emit(@NonNull Event event) {
    try {
      Logger.v(TAG + " emit() : " + event);
      if (event instanceof InAppEvent) {
        Logger.v(TAG + " emit() : Emitting in-app event.");
        this.emitInAppEvent((InAppEvent) event);
      } else if (event instanceof PushEvent) {
        Logger.v(TAG + " emit() : Emitting push event");
        this.emitPushEvent((PushEvent) event);
      } else if (event instanceof TokenEvent) {
        Logger.v(TAG + " emit() : Emitting token event");
        emitPushToken((TokenEvent) event);
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

  private void emitPushToken(TokenEvent tokenEvent) {
    try {
      Logger.v(TAG + " emitPushToken() : " + tokenEvent);
      String methodName = eventMap.get(tokenEvent.getEventType());
      if (methodName == null) return;
      JSONObject tokenPayload = UtilsKt.pushTokenToJson(tokenEvent.getPushToken());
      emit(tokenEvent.getEventType(), tokenPayload);
    } catch (Exception e) {
      Logger.e(TAG + " emitPushToken() : ", e);
    }
  }

  private void emit(EventType eventType, JSONObject payload) {
    try {
      UnityPlayer.UnitySendMessage(gameObjectName, eventMap.get(eventType), payload.toString());
    } catch (Exception e) {
      Logger.e(TAG + " emit() : ", e);
    }
  }

  private static final EnumMap<EventType, String> eventMap = new EnumMap<>(EventType.class);

  static {
    eventMap.put(EventType.PUSH_CLICKED, Constants.METHOD_NAME_PUSH_REDIRECTION);
    eventMap.put(EventType.INAPP_SHOWN, Constants.METHOD_NAME_IN_APP_SHOWN);
    eventMap.put(EventType.INAPP_NAVIGATION, Constants.METHOD_NAME_IN_APP_CLICKED);
    eventMap.put(EventType.INAPP_CLOSED, Constants.METHOD_NAME_IN_APP_CLOSED);
    eventMap.put(EventType.INAPP_CUSTOM_ACTION, Constants.METHOD_NAME_IN_APP_CUSTOM_ACTION);
    eventMap.put(EventType.INAPP_SELF_HANDLED_AVAILABLE, Constants.METHOD_NAME_IN_APP_SELF_HANDLED);
    eventMap.put(EventType.PUSH_TOKEN_GENERATED, Constants.METHOD_NAME_PUSH_TOKEN);
  }
}
