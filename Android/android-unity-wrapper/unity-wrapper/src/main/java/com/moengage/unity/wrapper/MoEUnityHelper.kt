package com.moengage.unity.wrapper

import com.moengage.inapp.MoEInAppHelper;
import com.moengage.core.internal.inapp.InAppManager
import com.moengage.core.internal.logger.Logger

/**
 * @author Arshiya Khanum
 */
public class MoEUnityHelper {

    private val tag = MODULE_TAG + "MoEUnityHelper"

    public companion object {

        private var instance: MoEUnityHelper? = null

        @JvmStatic
        public fun getInstance(): MoEUnityHelper {
            return instance ?: synchronized(MoEUnityHelper::class.java) {
                val inst = instance ?: MoEUnityHelper()
                instance = inst
                inst
            }
        }
    }

    public fun onConfigurationChanged() {
        if (!InAppManager.hasModule()) {
            Logger.print { "$tag onConfigurationChanged() : InApp module not found." }
            return
        }
        MoEInAppHelper.getInstance().onConfigurationChanged()
    }
}