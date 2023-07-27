/*
 * Copyright (c) 2014-2023 MoEngage Inc.
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

package com.moengage.unity.wrapper.geofence

import android.content.Context
import com.moengage.core.LogLevel
import com.moengage.core.internal.logger.Logger
import com.moengage.plugin.base.geofence.internal.GeofencePluginHelper

/**
 * Bridge between Unity and Android Native
 * @author Rishabh Harish
 * @since TODO
 */
class MoEGeofenceWrapper private constructor() {

    private val tag = "${MODULE_TAG}MoEGeofenceWrapper"
    private val geofencePluginHelper = GeofencePluginHelper()
    private var context: Context? = null

    companion object {

        private var instance: MoEGeofenceWrapper? = null

        @JvmStatic
        public fun getInstance(): MoEGeofenceWrapper {
            return instance ?: synchronized(MoEGeofenceWrapper::class.java) {
                val inst = instance ?: MoEGeofenceWrapper()
                instance = inst
                inst
            }
        }
    }

    fun setContext(context: Context) {
        this.context = context
    }

    fun startGeofenceMonitoring(geofencePayload: String) {
        try {
            Logger.print { "$tag startGeofenceMonitoring(): " }
            geofencePluginHelper.startGeofenceMonitoring(getContext(), geofencePayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag startGeofenceMonitoring(): " }
        }
    }

    fun stopGeofenceMonitoring(geofencePayload: String) {
        try {
            Logger.print { "$tag stopGeofenceMonitoring(): " }
            geofencePluginHelper.stopGeofenceMonitoring(getContext(), geofencePayload)
        } catch (t: Throwable) {
            Logger.print(LogLevel.ERROR, t) { "$tag stopGeofenceMonitoring(): " }
        }
    }

    @Throws(NullPointerException::class)
    private fun getContext(): Context {
        return context ?: throw NullPointerException("cannot proceed with null context")
    }
}
