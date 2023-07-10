/*
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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoEngage {
  public class MoEngageGeofenceClient: MonoBehaviour {
    private static string appId;

    private static MoEGeofenceUnityPlatform _moengageHandler;
    private static MoEGeofenceUnityPlatform moengageHandler {
      get {
        if (_moengageHandler != null) {
          return _moengageHandler;
        }
        #if UNITY_ANDROID
        _moengageHandler = new MoEngageGeofenceAndroid();
        #elif UNITY_IOS
        _moengageHandler = new MoEngageGeofenceiOS();
        #endif
        return _moengageHandler;
      }
    }

    private static Boolean isPluginInitialized() {
      return appId != null;
    }

    #region Initialize
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId">Account Identifier</param>
    public static void Initialize(string appId) {
      Debug.Log(": Geofence initialized " + appId);
      MoEngageGeofenceClient.appId = appId;
    }

    #endregion

    #region Geofence

    public static void StartGeofenceMonitoring() {
      if (!isPluginInitialized()) return;
      #if(UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
      string accountPayload = MoEUtils.GetAccountPayload(appId);
      Debug.Log(": StartGeofenceMonitoring:: payload: " + accountPayload);
      moengageHandler.StartGeofenceMonitoring(accountPayload);
      #endif
    }

    public static void StopGeofenceMonitoring() {
      if (!isPluginInitialized()) return;
      #if(UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
      string accountPayload = MoEUtils.GetAccountPayload(appId);
      Debug.Log(": StopGeofenceMonitoring:: payload: " + accountPayload);
      moengageHandler.StopGeofenceMonitoring(accountPayload);
      #endif
    }

    #endregion

  }
}