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
import com.moengage.plugin.base.geofence.internal.GeofencePluginHelper
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

class MoEAndroidWrapper private constructor() {

    private val pluginHelper: PluginHelper = PluginHelper()
    private val geofencePluginHelper: GeofencePluginHelper = GeofencePluginHelper()

    private val tag = MODULE_TAG + "MoEAndroidWrapper"
    private var context: Context? = null

    companion object {

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

    fun setContext(context: Context) {
        this.context = context
    }

    fun initialize(initializePayload: String) {
        try {
            Logger.print { "$tag initialize() : Initialization payload=$initializePayload" }
            val initializationJson = JSONObject(initializePayload)
            val gameObjectName = initializationJson.getString(ARGUMENT_GAME_OBJECT)
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

    fun trackEvent(eventPayload: String) {
        try {
            Logger.print { "$tag trackEvent() : Event Payload: $eventPayload" }
            pluginHelper.trackEvent(getContext(), eventPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag trackEvent() : " }
        }
    }

    fun passPushPayload(pushPayload: String) {
        try {
            Logger.print { "$tag passPushPayload(): pushPayload=$pushPayload" }
            pluginHelper.passPushPayload(getContext(), pushPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag passPushPayload() : " }
        }
    }

    fun passPushToken(pushTokenPayload: String) {
        try {
            Logger.print { "$tag passPushToken() : Token Payload: $pushTokenPayload" }
            pluginHelper.passPushToken(getContext(), pushTokenPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag passPushToken() : " }
        }
    }

    fun getSelfHandledInApp(selfHandledPayload: String) {
        try {
            Logger.print { "$tag getSelfHandledInApp() : Will try to fetch self-handled in-app" }
            pluginHelper.getSelfHandledInApp(getContext(), selfHandledPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag getSelfHandledInApp() : " }
        }
    }

    fun showInApp(showInAppPayload: String) {
        try {
            Logger.print { "$tag showInApp() : Will try to show in-app" }
            pluginHelper.showInApp(getContext(), showInAppPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag showInApp() : " }
        }
    }

    fun logout(logoutPayload: String) {
        try {
            Logger.print { "$tag logout() : Will try to logout user." }
            pluginHelper.logout(getContext(), logoutPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag logout() : " }
        }
    }

    fun setAlias(aliasPayload: String) {
        try {
            Logger.print { "$tag setAlias() : Alias Payload: $aliasPayload" }
            pluginHelper.setAlias(getContext(), aliasPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setAlias() : " }
        }
    }

    fun setAppStatus(appStatusPayload: String) {
        try {
            Logger.print { "$tag setAppStatus() : App status payload: $appStatusPayload" }
            pluginHelper.setAppStatus(getContext(), appStatusPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setAppStatus() : " }
        }
    }

    fun setUserAttribute(userAttributePayload: String) {
        try {
            Logger.print { "$tag setUserAttribute() : User Attribute payload: $userAttributePayload" }
            pluginHelper.setUserAttribute(getContext(), userAttributePayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setUserAttribute() : " }
        }
    }

    fun setAppContext(contextPayload: String) {
        try {
            Logger.print { "$tag setAppContext() : Context Payload: $contextPayload" }
            pluginHelper.setAppContext(getContext(), contextPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setAppContext() : " }
        }
    }

    fun resetContext(resetContextPayload: String) {
        try {
            Logger.print { "$tag resetContext() : Resetting app context" }
            pluginHelper.resetAppContext(getContext(), resetContextPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag resetContext() : " }
        }
    }

    fun selfHandledShown(selfHandledPayload: String) {
        try {
            Logger.print { "$tag selfHandledShown() : Campaign payload: $selfHandledPayload" }
            pluginHelper.selfHandledCallback(getContext(), selfHandledPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag selfHandledShown() : " }
        }
    }

    fun selfHandledClicked(selfHandledPayload: String) {
        try {
            Logger.print { "$tag selfHandledClicked() : Campaign payload: $selfHandledPayload" }
            pluginHelper.selfHandledCallback(getContext(), selfHandledPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag selfHandledClicked() : " }
        }
    }

    fun selfHandledDismissed(selfHandledPayload: String) {
        try {
            Logger.print { "$tag selfHandledDismissed() : Campaign payload: $selfHandledPayload" }
            pluginHelper.selfHandledCallback(getContext(), selfHandledPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag selfHandledDismissed() : " }
        }
    }

    fun selfHandledCallback(selfHandledPayload: String) {
        try {
            Logger.print { "$tag selfHandledCallback() : Campaign payload: $selfHandledPayload" }
            pluginHelper.selfHandledCallback(getContext(), selfHandledPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag selfHandledCallback() : " }
        }
    }

    fun optOutTracking(optOutPayload: String) {
        try {
            Logger.print { "$tag optOutTracking() : OptOut payload: $optOutPayload" }
            pluginHelper.optOutTracking(getContext(), optOutPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag optOutTracking() : " }
        }
    }

    fun updateSdkState(featureStatusPayload: String) {
        try {
            Logger.print { "$tag storeFeatureStatus() : Feature status payload: $featureStatusPayload" }
            pluginHelper.storeFeatureStatus(getContext(), featureStatusPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag storeFeatureStatus() : " }
        }
    }

    fun onOrientationChanged() {
        try {
            Logger.print { "$tag onOrientationChanged() : " }
            pluginHelper.onConfigurationChanged()
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag onOrientationChanged() : " }
        }
    }

    fun deviceIdentifierTrackingStatusUpdate(payload: String) {
        try {
            Logger.print { "$tag deviceIdentifierTrackingStatusUpdate() : Arguments: $payload" }
            pluginHelper.deviceIdentifierTrackingStatusUpdate(getContext(), payload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag deviceIdentifierTrackingStatusUpdate() : " }
        }
    }

    fun permissionResponse(permissionResultPayload: String) {
        try {
            Logger.print { "$tag permissionResponse(): permissionResultPayload=$permissionResultPayload" }
            pluginHelper.permissionResponse(getContext(), permissionResultPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag permissionResponse(): " }
        }
    }

    fun setUpNotificationChannels() {
        try {
            Logger.print { "$tag setUpNotificationChannels(): " }
            pluginHelper.setUpNotificationChannels(getContext())
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag setUpNotificationChannels(): " }
        }
    }

    fun navigateToSettings() {
        try {
            Logger.print { "$tag navigateToSettings(): " }
            pluginHelper.navigateToSettings(getContext())
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag navigateToSettings(): " }
        }
    }

    fun requestPushPermission() {
        try {
            Logger.print { "$tag requestPushPermission(): " }
            pluginHelper.requestPushPermission(getContext())
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag requestPushPermission(): " }
        }
    }

    fun updatePushPermissionRequestCount(pushOptInMetaPayload: String) {
        try {
            Logger.print { "$tag updatePushPermissionRequestCount(): pushOptInMeta=$pushOptInMetaPayload" }
            pluginHelper.updatePushPermissionRequestCount(getContext(), pushOptInMetaPayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag updatePushPermissionRequestCount(): " }
        }
    }

    fun startGeofenceMonitoring(instancePayload: String) {
        try {
            Logger.print { "$tag startGeofenceMonitoring(): instancePayload=$instancePayload" }
            geofencePluginHelper.startGeofenceMonitoring(getContext(), instancePayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag startGeofenceMonitoring(): " }
        }
    }

    fun stopGeofenceMonitoring(instancePayload: String) {
        try {
            Logger.print { "$tag stopGeofenceMonitoring(): instancePayload=$instancePayload" }
            geofencePluginHelper.stopGeofenceMonitoring(getContext(), instancePayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag stopGeofenceMonitoring(): " }
        }
    }


    @Throws(NullPointerException::class)
    private fun getContext(): Context {
        return context ?: throw NullPointerException("Cannot proceed with null context")
    }
}