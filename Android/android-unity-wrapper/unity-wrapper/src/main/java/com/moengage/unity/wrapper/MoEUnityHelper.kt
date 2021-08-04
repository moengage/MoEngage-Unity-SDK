package com.moengage.unity.wrapper

/**
 * @author Arshiya Khanum
 */
public class MoEUnityHelper {

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
        MoEAndroidWrapper.getInstance().onConfigurationChanged()
    }
}