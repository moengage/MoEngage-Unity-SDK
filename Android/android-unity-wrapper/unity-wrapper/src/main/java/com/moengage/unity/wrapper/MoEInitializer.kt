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

import android.app.Application
import android.content.Context
import com.moengage.core.LogLevel
import com.moengage.core.MoEngage
import com.moengage.core.internal.logger.Logger
import com.moengage.core.internal.model.IntegrationMeta
import com.moengage.core.model.SdkState
import com.moengage.plugin.base.internal.PluginInitializer

/**
 * @author Umang Chamaria
 * Date: 26/06/20
 */
public object MoEInitializer {

    private const val tag = "${MODULE_TAG}MoEInitializer"

    /**
     * Initialise the default instance of SDK with configuration provided in [MoEngage.Builder]
     *
     * @param context Context
     * @param builder Instance of [MoEngage.Builder]
     * @since 3.0.0
     */
    public fun initialiseDefaultInstance(context: Context, builder: MoEngage.Builder) {
        try {
            MoEAndroidWrapper.getInstance().setContext(context)
            PluginInitializer.initialize(
                builder,
                IntegrationMeta(INTEGRATION_TYPE, BuildConfig.MOENGAGE_ANDROID_UNITY_WRAPPER)
            )
            Logger.print { "$tag initialiseDefaultInstance(): initialised the sdk" }
            Logger.print { "$tag initialiseDefaultInstance(): unity wrapper version: ${BuildConfig.MOENGAGE_ANDROID_UNITY_WRAPPER}" }
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag initialiseDefaultInstance(): " }
        }
    }

    /**
     * Initialise the default instance of SDK with configuration provided in [MoEngage.Builder]
     *
     * @param context Context
     * @param builder Instance of [MoEngage.Builder]
     * @param sdkState [SdkState.ENABLED]
     * @since 3.0.0
     */
    public fun initialiseDefaultInstance(
        context: Context,
        builder: MoEngage.Builder,
        sdkState: SdkState
    ) {
        try {
            MoEAndroidWrapper.getInstance().setContext(context)
            PluginInitializer.initialize(
                builder = builder,
                integrationMeta = IntegrationMeta(INTEGRATION_TYPE, BuildConfig.MOENGAGE_ANDROID_UNITY_WRAPPER),
                sdkState = sdkState
            )
            Logger.print { "$tag initialiseDefaultInstance(): initialised the sdk" }
            Logger.print { "$tag initialiseDefaultInstance(): unity wrapper version: ${BuildConfig.MOENGAGE_ANDROID_UNITY_WRAPPER}" }
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag initialiseDefaultInstance(): " }
        }
    }

    /**
     * Initialize SDK using file based configuration.
     *
     * Note: While using this function to intialize the SDK, make sure you have configured all the
     * required configuration in the xml as resource value
     */
    public fun initialiseDefaultInstance(
        application: Application,
        sdkState: SdkState? = null,
    ) {
        try {
            MoEAndroidWrapper.getInstance().setContext(application.applicationContext)
            PluginInitializer.initialize(
                application = application,
                integrationMeta = IntegrationMeta(
                    INTEGRATION_TYPE,
                    BuildConfig.MOENGAGE_ANDROID_UNITY_WRAPPER
                ),
                sdkState = sdkState
            )
            Logger.print { "$tag initialize(): initialised the sdk via file" }
            Logger.print { "$tag initialiseDefaultInstance(): unity wrapper version: ${BuildConfig.MOENGAGE_ANDROID_UNITY_WRAPPER}" }
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag initialize(): " }
        }
    }
}