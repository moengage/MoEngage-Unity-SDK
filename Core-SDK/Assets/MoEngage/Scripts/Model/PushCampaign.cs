<<<<<<< HEAD
/*
=======
﻿/*
>>>>>>> MOEN-20006-Unity-Multiinstance
 * Copyright (c) 2014-2020 MoEngage Inc.
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

<<<<<<< HEAD
using System;
=======
>>>>>>> MOEN-20006-Unity-Multiinstance
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
<<<<<<<< HEAD:Core-SDK/Assets/MoEngage/Scripts/Model/PushCampaign.cs
namespace MoEngage
{
    [System.Serializable]
    public class PushCampaign
    {
        public bool isDefaultAction;
        public IDictionary<string,object> clickedAction;
        public IDictionary<string,object> payload;

        public PushCampaign(Dictionary<string, object> pushPayload)
        {
            if (pushPayload.ContainsKey(MoEConstants.PARAM_IS_DEFAULT_ACTION)) {
                isDefaultAction = (bool)pushPayload[MoEConstants.PARAM_IS_DEFAULT_ACTION];
            }
========
namespace MoEngage {

  #if UNITY_ANDROID
  public class MoEngageAndroid: MoEGeofenceUnityPlatform {
    private
    const string TAG = "MoEngageAndroid";

    private static AndroidJavaClass moengageAndroidClass = new AndroidJavaClass("com.moengage.unity.wrapper.MoEAndroidWrapper");
    private static AndroidJavaObject moengageAndroid = moengageAndroidClass.CallStatic < AndroidJavaObject > ("getInstance");
>>>>>>>> MOEN-20006-Unity-Multiinstance:Geofence/Assets/Geofence/MoEngageGeofenceAndroid.cs

    public void StartGeofenceMonitoring(string payload) {

    }

    public void StopGeofenceMonitoring(string payload) {

    }

  }

  #endif
=======
namespace MoEngage {
  [System.Serializable]
  /// <summary>
  /// Push Payload information
  /// </summary>
  public class PushCampaign {
    /// <value> This key is present only for the Android Platform. It's a boolean value indicating if the user clicked on the default content or not. true if the user clicks on the default content else false.  </value>
    public bool isDefaultAction;

    /// <value> Action to be performed on notification click.  </value>
    public IDictionary < string, object > clickedAction;

    /// <value>Complete campaign payload. </value>
    public IDictionary < string, object > payload;
  }
>>>>>>> MOEN-20006-Unity-Multiinstance
}