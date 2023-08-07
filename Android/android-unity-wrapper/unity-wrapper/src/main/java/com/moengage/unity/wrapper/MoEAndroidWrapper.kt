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

import android.content.Context
import com.moengage.core.LogLevel
import com.moengage.core.internal.logger.Logger
import com.moengage.plugin.base.internal.ARGUMENT_DATA
import com.moengage.plugin.base.internal.PluginHelper
import com.moengage.plugin.base.internal.setEventEmitter
import org.json.JSONObject

/**
 * Bridge between Unity and Android Native
 *
 * @author Umang Chamaria
 * Date: 26/06/20
 */

private const val ARGUMENT_GAME_OBJECT = "gameObjectName"

public class MoEAndroidWrapper private constructor() {

    private val pluginHelper: PluginHelper = PluginHelper()

    private val tag = "${MODULE_TAG}MoEAndroidWrapper"
    private var context: Context? = null

    public companion object {

        private var instance: MoEAndroidWrapper? = null

        @JvmStatic
        public fun getInstance(): MoEAndroidWrapper {
            return instance ?: synchronized(MoEAndroidWrapper::class.java) {
                val inst = instance ?: MoEAndroidWrapper()
                instance = inst
                inst
            }
        }
    }

    public fun setContext(context: Context) {
        this.context = context
    }

    public fun initialize(initializePayload: String) {
        try {
            Logger.print { "$tag initialize() : Initialization payload=$initializePayload" }
            val initializationJson = JSONObject(initializePayload)
            val gameObjectName =
                initializationJson.getJSONObject(ARGUMENT_DATA).getString(ARGUMENT_GAME_OBJECT)
            if (gameObjectName.isEmpty()) {
                Logger.print(
                    LogLevel.ERROR
                ) { "$tag initialize() : Game object name is empty cannot pass callbacks" }
                return
            }
            setEventEmitter(EventEmitterImpl(gameObjectName))
            pluginHelper.initialise(initializationJson)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag initialise() : " }
        }
    }

    public fun trackEvent(eventPayload: String) {
        try {
            Logger.print { "$tag trackEvent() : Event Payload: $eventPayload" }
            pluginHelper.trackEvent(getContext(), eventPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag trackEvent() : " }
        }
    }

    public fun passPushPayload(pushPayload: String) {
        try {
            Logger.print { "$tag passPushPayload(): pushPayload=$pushPayload" }
            pluginHelper.passPushPayload(getContext(), pushPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag passPushPayload() : " }
        }
    }

    public fun passPushToken(pushTokenPayload: String) {
        try {
            Logger.print { "$tag passPushToken() : Token Payload: $pushTokenPayload" }
            pluginHelper.passPushToken(getContext(), pushTokenPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag passPushToken() : " }
        }
    }

    public fun getSelfHandledInApp(selfHandledPayload: String) {
        try {
            Logger.print { "$tag getSelfHandledInApp() : Will try to fetch self-handled in-app" }
            pluginHelper.getSelfHandledInApp(getContext(), selfHandledPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag getSelfHandledInApp() : " }
        }
    }

    public fun showInApp(showInAppPayload: String) {
        try {
            Logger.print { "$tag showInApp() : Will try to show in-app" }
            pluginHelper.showInApp(getContext(), showInAppPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag showInApp() : " }
        }
    }

    public fun logout(logoutPayload: String) {
        try {
            Logger.print { "$tag logout() : Will try to logout user." }
            pluginHelper.logout(getContext(), logoutPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag logout() : " }
        }
    }

    public fun setAlias(aliasPayload: String) {
        try {
            Logger.print { "$tag setAlias() : Alias Payload: $aliasPayload" }
            pluginHelper.setAlias(getContext(), aliasPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setAlias() : " }
        }
    }

    public fun setAppStatus(appStatusPayload: String) {
        try {
            Logger.print { "$tag setAppStatus() : App status payload: $appStatusPayload" }
            pluginHelper.setAppStatus(getContext(), appStatusPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setAppStatus() : " }
        }
    }

    public fun setUserAttribute(userAttributePayload: String) {
        try {
            Logger.print { "$tag setUserAttribute() : User Attribute payload: $userAttributePayload" }
            pluginHelper.setUserAttribute(getContext(), userAttributePayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setUserAttribute() : " }
        }
    }

    public fun setAppContext(contextPayload: String) {
        try {
            Logger.print { "$tag setAppContext() : Context Payload: $contextPayload" }
            pluginHelper.setAppContext(getContext(), contextPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setAppContext() : " }
        }
    }

    public fun resetContext(resetContextPayload: String) {
        try {
            Logger.print { "$tag resetContext() : Resetting app context" }
            pluginHelper.resetAppContext(getContext(), resetContextPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag resetContext() : " }
        }
    }

    public fun selfHandledCallback(selfHandledPayload: String) {
        try {
            Logger.print { "$tag selfHandledCallback() : Campaign payload: $selfHandledPayload" }
            pluginHelper.selfHandledCallback(getContext(), selfHandledPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag selfHandledCallback() : " }
        }
    }

    public fun optOutTracking(optOutPayload: String) {
        try {
            Logger.print { "$tag optOutTracking() : OptOut payload: $optOutPayload" }
            pluginHelper.optOutTracking(getContext(), optOutPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag optOutTracking() : " }
        }
    }

    public fun updateSdkState(featureStatusPayload: String) {
        try {
            Logger.print { "$tag updateSdkState() : Feature status payload: $featureStatusPayload" }
            pluginHelper.storeFeatureStatus(getContext(), featureStatusPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag updateSdkState() : " }
        }
    }

    public fun onOrientationChanged() {
        try {
            Logger.print { "$tag onOrientationChanged() : " }
            pluginHelper.onConfigurationChanged()
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag onOrientationChanged() : " }
        }
    }

    public fun deviceIdentifierTrackingStatusUpdate(payload: String) {
        try {
            Logger.print { "$tag deviceIdentifierTrackingStatusUpdate() : Arguments: $payload" }
            pluginHelper.deviceIdentifierTrackingStatusUpdate(getContext(), payload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag deviceIdentifierTrackingStatusUpdate() : " }
        }
    }

    public fun permissionResponse(permissionResultPayload: String) {
        try {
            Logger.print { "$tag permissionResponse(): permissionResultPayload=$permissionResultPayload" }
            pluginHelper.permissionResponse(getContext(), permissionResultPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag permissionResponse(): " }
        }
    }

    public fun setUpNotificationChannels() {
        try {
            Logger.print { "$tag setUpNotificationChannels(): " }
            pluginHelper.setUpNotificationChannels(getContext())
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setUpNotificationChannels(): " }
        }
    }

    public fun navigateToSettings() {
        try {
            Logger.print { "$tag navigateToSettings(): " }
            pluginHelper.navigateToSettings(getContext())
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag navigateToSettings(): " }
        }
    }

    public fun requestPushPermission() {
        try {
            Logger.print { "$tag requestPushPermission(): " }
            pluginHelper.requestPushPermission(getContext())
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag requestPushPermission(): " }
        }
    }

    public fun updatePushPermissionRequestCount(pushOptInMetaPayload: String) {
        try {
            Logger.print { "$tag updatePushPermissionRequestCount(): pushOptInMeta=$pushOptInMetaPayload" }
            pluginHelper.updatePushPermissionRequestCount(getContext(), pushOptInMetaPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag updatePushPermissionRequestCount(): " }
        }
    }

    @Throws(NullPointerException::class)
    private fun getContext(): Context {
        return context ?: throw NullPointerException("Cannot proceed with null context")
    }
}