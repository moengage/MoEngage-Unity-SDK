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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoEMiniJSON;

namespace MoEngage {
  public class MoEGameObject: MonoBehaviour {
    [SerializeField]
    private string appId;

    [SerializeField]
    private string shouldDeliverCallbackOnForegroundClick;

    private
    const string TAG = "MoEGameObject";

    public static event EventHandler < PushToken > PushTokenCallback;
    public static event EventHandler < PushCampaignData > PushNotifCallback;
    public static event EventHandler < InAppData > InAppShown;
    public static event EventHandler < InAppClickData > InAppClicked;
    public static event EventHandler < InAppData > InAppDismissed;
    public static event EventHandler < InAppClickData > InAppCustomAction;
    public static event EventHandler < InAppSelfHandledCampaignData > InAppSelfHandled;
    public static event EventHandler < PermissionResultData > PermissionResultCallback;

    // Start is called before the first frame update
    void Start() {
      MoEngageClient.Initialize(
        gameObject,
        appId, 
        shouldDeliverCallbackOnForegroundClick);
    }

    public void PushToken(string payload) {
      Debug.Log(TAG + " PushToken() Callback from native: " + payload);
      PushToken token = MoEPushParser.GetPushTokenFromPayload(payload);
      OnPushTokenGenerated(token);
    }

    public void PushClicked(string payload) {
      Debug.Log(TAG + " PushClicked() Callback from Native: " + payload);
      PushCampaignData campaign = MoEPushParser.GetPushClickPayload(payload);
      OnPushClicked(campaign);
    }

    public void InAppCampaignShown(string payload) {
      Debug.Log(TAG + " InAppCampaignShown() Callback From Native" + payload);
      InAppData inAppData = MoEInAppParser.GetInAppCampaignFromPayload(payload);
      OnInAppShown(inAppData);
    }

    public void InAppCampaignClicked(string payload) {
      Debug.Log(TAG + " InAppCampaignClicked() Callback From Native" + payload);
      InAppClickData inAppData = MoEInAppParser.GetInAppClickData(payload);
      OnInAppClicked(inAppData);
    }

    public void InAppCampaignDismissed(string payload) {
      Debug.Log(TAG + " InAppCampaignDismissed() Callback from Native: " + payload);
      InAppData inAppData = MoEInAppParser.GetInAppCampaignFromPayload(payload);
      OnInAppDismissed(inAppData);
    }

    public void InAppCampaignCustomAction(string payload) {
      Debug.Log(TAG + " InAppCampaignCustomAction() Callback from Native: " + payload);
      InAppClickData inAppData = MoEInAppParser.GetInAppClickData(payload);
      OnInAppCustomAction(inAppData);
    }

    public void InAppCampaignSelfHandled(string payload) {
      Debug.Log(TAG + " InAppCampaignSelfHandled() Callback from Native: " + payload);
      InAppSelfHandledCampaignData inAppData = MoEInAppParser.GetInAppSelfHandledData(payload);
      OnInAppSelfHandled(inAppData);
    }

    public void PermissionResult(string payload) {
      try {
        Debug.Log(TAG + " PermissionResult() : Callback from Native: " + payload);
        PermissionResultData permissionResultData = MoEParser.GetPermissionResultData(payload);
        OnPushPermissionCallbackReceived(permissionResultData);
      } catch (Exception e) {
        Debug.LogError(TAG + " PermissionResult() : couldn't send callback due to exception." +
          $"\n{e.Message}" +
          $"\n{e.StackTrace}");
      }
    }

    protected virtual void OnPushClicked(PushCampaignData payload) {
      PushNotifCallback?.Invoke(this, payload);
    }

    protected virtual void OnInAppShown(InAppData inAppData) {
      InAppShown?.Invoke(this, inAppData);
    }

    protected virtual void OnInAppClicked(InAppClickData inAppData) {
      InAppClicked?.Invoke(this, inAppData);
    }

    protected virtual void OnInAppDismissed(InAppData inAppData) {
      InAppDismissed?.Invoke(this, inAppData);
    }

    protected virtual void OnInAppCustomAction(InAppClickData inAppData) {
      InAppCustomAction?.Invoke(this, inAppData);
    }

    protected virtual void OnInAppSelfHandled(InAppSelfHandledCampaignData inAppData) {
      InAppSelfHandled?.Invoke(this, inAppData);
    }

    protected virtual void OnPushTokenGenerated(PushToken pushToken) {
      PushTokenCallback?.Invoke(this, pushToken);
    }

    protected virtual void OnPushPermissionCallbackReceived(PermissionResultData permissionResultData) {
      PermissionResultCallback?.Invoke(this, permissionResultData);
    }
  }
}