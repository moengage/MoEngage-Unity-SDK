package com.moengage.unity.wrapper

import android.content.res.Configuration
import com.moengage.core.internal.logger.Logger
import com.unity3d.player.UnityPlayerActivity

/**
 * @author Arshiya Khanum
 */
class MoEUnityPlayerActivity: UnityPlayerActivity() {

    private val tag = "MoEUnityPlayerActivity"

    override fun onConfigurationChanged(newConfig: Configuration) {
        super.onConfigurationChanged(newConfig)
        Logger.v("$tag onConfigurationChanged() : ${newConfig.orientation}")
        MoEUnityHelper.getInstance().onConfigurationChanged()
    }
}