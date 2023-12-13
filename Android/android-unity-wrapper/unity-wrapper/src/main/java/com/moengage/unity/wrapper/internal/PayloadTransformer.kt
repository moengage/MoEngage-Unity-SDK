package com.moengage.unity.wrapper.internal

import com.moengage.core.model.user.deletion.UserDeletionData
import com.moengage.plugin.base.internal.ARGUMENT_ACCOUNT_META
import com.moengage.plugin.base.internal.ARGUMENT_APP_ID
import com.moengage.unity.wrapper.ARGUMENT_IS_SUCCESS
import org.json.JSONObject

/**
 * @author Arshiya Khanum
 */
internal class PayloadTransformer {

    /**
     * Transforms [UserDeletionData] to [JSONObject] string.
     *
     * @param userDeletionData instance of [UserDeletionData]
     * @return [String]
     */
    internal fun userDeletionDataToJsonString(userDeletionData: UserDeletionData): String {
        val userDeletionDataJs = JSONObject()
        userDeletionDataJs.put(
            ARGUMENT_ACCOUNT_META,
            JSONObject().put(ARGUMENT_APP_ID, userDeletionData.accountMeta.appId)
        )
        userDeletionDataJs.put(ARGUMENT_IS_SUCCESS, userDeletionData.isSuccess)
        return userDeletionDataJs.toString()
    }
}