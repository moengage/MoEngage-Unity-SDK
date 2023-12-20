package com.moengage.unity.wrapper.internal

import com.moengage.core.model.user.deletion.UserDeletionData

/**
 * Internal callback interface for User Deletion result. This is implemented by the Unity C#
 * class.
 *
 * @author Arshiya Khanum
 */
public interface UserDeletionCallback {

    /**
     * Callback method triggered when [UserDeletionCallback.onResult] is received from Native SDK.
     *
     * @param result returns payload with [UserDeletionData]
     */
    public fun onResult(result: String)

}