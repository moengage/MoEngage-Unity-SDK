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
package com.moengage.unity.wrapper

import com.moengage.core.LogLevel
import com.moengage.core.internal.logger.Logger
import com.moengage.plugin.base.internal.EventEmitter
import com.moengage.plugin.base.internal.clickDataToJson
import com.moengage.plugin.base.internal.inAppDataToJson
import com.moengage.plugin.base.internal.model.events.Event
import com.moengage.plugin.base.internal.model.events.EventType
import com.moengage.plugin.base.internal.model.events.inapp.InAppActionEvent
import com.moengage.plugin.base.internal.model.events.inapp.InAppLifecycleEvent
import com.moengage.plugin.base.internal.model.events.inapp.InAppSelfHandledEvent
import com.moengage.plugin.base.internal.model.events.push.PermissionEvent
import com.moengage.plugin.base.internal.model.events.push.PushClickedEvent
import com.moengage.plugin.base.internal.model.events.push.TokenEvent
import com.moengage.plugin.base.internal.permissionResultToJson
import com.moengage.plugin.base.internal.pushPayloadToJson
import com.moengage.plugin.base.internal.selfHandledDataToJson
import com.moengage.plugin.base.internal.tokenEventToJson
import com.unity3d.player.UnityPlayer
import org.json.JSONObject
import java.util.EnumMap

/**
 * @author Umang Chamaria
 * Date: 2020/09/24
 */
internal class EventEmitterImpl(private val gameObjectName: String) : EventEmitter {


    private val tag = "${MODULE_TAG}EventEmitterImpl"
    private val eventMap = EnumMap<EventType, String>(EventType::class.java)

    init {
        eventMap[EventType.PUSH_CLICKED] = METHOD_NAME_PUSH_REDIRECTION
        eventMap[EventType.INAPP_SHOWN] = METHOD_NAME_IN_APP_SHOWN
        eventMap[EventType.INAPP_NAVIGATION] = METHOD_NAME_IN_APP_CLICKED
        eventMap[EventType.INAPP_CLOSED] = METHOD_NAME_IN_APP_CLOSED
        eventMap[EventType.INAPP_CUSTOM_ACTION] = METHOD_NAME_IN_APP_CUSTOM_ACTION
        eventMap[EventType.INAPP_SELF_HANDLED_AVAILABLE] = METHOD_NAME_IN_APP_SELF_HANDLED
        eventMap[EventType.PUSH_TOKEN_GENERATED] = METHOD_NAME_PUSH_TOKEN
        eventMap[EventType.PERMISSION] = METHOD_NAME_PERMISSION_RESULT
    }

    override fun emit(event: Event) {
        try {
            Logger.print { "$tag emit() : $event" }
            when (event) {
                is InAppActionEvent -> emitInAppActionEvent(event)
                is PushClickedEvent -> emitPushEvent(event)
                is TokenEvent -> emitPushTokenEvent(event)
                is InAppLifecycleEvent -> emitInAppLifecycleEvent(event)
                is InAppSelfHandledEvent -> emitInAppSelfHandledEvent(event)
                is PermissionEvent -> emitPermissionEvent(event)

            }
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emit() : Exception: " }
        }
    }

    private fun emitInAppActionEvent(inAppActionEvent: InAppActionEvent) {
        try {
            Logger.print {
                "$tag emitInAppActionEvent() : inAppActionEvent=${inAppActionEvent.eventType} clickData=${inAppActionEvent.clickData}"
            }
            val methodName = eventMap[inAppActionEvent.eventType] ?: return
            val campaign = clickDataToJson(inAppActionEvent.clickData)
            emit(methodName, campaign)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emitInAppActionEvent():" }
        }
    }

    private fun emitPushEvent(pushClickedEvent: PushClickedEvent) {
        try {
            Logger.print { "$tag emitPushEvent(): pushClickedEvent=$pushClickedEvent" }
            val methodName = eventMap[pushClickedEvent.eventType] ?: return
            val payload = pushPayloadToJson(pushClickedEvent.payload)
            emit(methodName, payload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emitPushEvent(): " }
        }
    }

    private fun emitPushTokenEvent(tokenEvent: TokenEvent) {
        try {
            Logger.print { "$tag emitPushTokenEvent() : tokenEvent=$tokenEvent" }
            val methodName = eventMap[tokenEvent.eventType] ?: return
            val tokenPayload = tokenEventToJson(tokenEvent)
            emit(methodName, tokenPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emitPushToken() : " }
        }
    }

    private fun emitInAppLifecycleEvent(inAppLifecycleEvent: InAppLifecycleEvent) {
        try {
            Logger.print { "$tag emitInAppLifecycleEvent(): inAppLifecycleEvent=${inAppLifecycleEvent.eventType}, inAppData=${inAppLifecycleEvent.inAppData}" }
            val methodName = eventMap[inAppLifecycleEvent.eventType] ?: return
            val tokenPayload = inAppDataToJson(inAppLifecycleEvent.inAppData)
            emit(methodName, tokenPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emitInAppLifecycleEvent() : " }
        }
    }

    private fun emitInAppSelfHandledEvent(inAppSelfHandledEvent: InAppSelfHandledEvent) {
        try {
            Logger.print { "$tag emitInAppSelfHandledEvent(): inAppSelfHandledEvent=${inAppSelfHandledEvent.eventType}, selfHandledData=${inAppSelfHandledEvent.data}" }
            val methodName = eventMap[inAppSelfHandledEvent.eventType] ?: return
            val inAppSelfHandledEventPayload = selfHandledDataToJson(inAppSelfHandledEvent.data)
            emit(methodName, inAppSelfHandledEventPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emitInAppSelfHandledEvent() : " }
        }
    }

    private fun emitPermissionEvent(permissionEvent: PermissionEvent) {
        try {
            Logger.print { "$tag emitPermissionEvent(): permissionResult=${permissionEvent.result}" }
            val methodName = eventMap[permissionEvent.eventType] ?: return
            val permissionPayload = permissionResultToJson(permissionEvent.result)
            emit(methodName, permissionPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emitPermissionEvent() : " }
        }
    }

    private fun emit(methodName: String, payload: JSONObject) {
        try {
            Logger.print { tag + "methodName=" + methodName + ", payload=" + payload }
            UnityPlayer.UnitySendMessage(gameObjectName, methodName, payload.toString())
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag emit() : " }
        }
    }


}