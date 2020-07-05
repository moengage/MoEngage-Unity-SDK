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

package com.moengage.unity.wrapper;

import android.support.annotation.NonNull;
import com.moengage.core.Logger;
import com.moengage.inapp.listeners.InAppMessageListener;
import com.moengage.inapp.model.MoEInAppCampaign;

/**
 * @author Umang Chamaria
 * Date: 26/06/20
 */
public class UnityInAppMessageListener extends InAppMessageListener {
  private static final String TAG = Constants.MODULE_TAG + "UnityInAppMessageListener";

  @Override public void onShown(@NonNull MoEInAppCampaign inAppCampaign) {
    try {
      super.onShown(inAppCampaign);
      Logger.v(TAG + " onShown() : In-App Shown. Campaign: " + inAppCampaign);
      MoEAndroidWrapper.getInstance().sendOrQueueCallback(Constants.METHOD_NAME_IN_APP_SHOWN,
          Utils.inAppCampaignToJson(inAppCampaign));
    } catch (Exception e) {
      Logger.e(TAG + " onShown() : ", e);
    }
  }

  @Override public boolean onNavigation(@NonNull MoEInAppCampaign inAppCampaign) {
    try {
      Logger.v(TAG
          + " onNavigation() : InApp Clicked with navigation Action. Campaign: "
          + inAppCampaign);
      if (inAppCampaign.navigationAction == null) {
        Logger.e(TAG + " onNavigation() : Navigation action is null cannot process further.");
        return false;
      }

      MoEAndroidWrapper.getInstance()
          .sendOrQueueCallback(Constants.METHOD_NAME_IN_APP_CLICKED,
              Utils.inAppCampaignToJson(inAppCampaign));
      return true;
    } catch (Exception e) {
      Logger.e(TAG + " onNavigation() : ", e);
    }
    return super.onNavigation(inAppCampaign);
  }

  @Override public void onClosed(@NonNull MoEInAppCampaign inAppCampaign) {
    try {
      super.onClosed(inAppCampaign);
      Logger.v(TAG + " onClosed() : In-App Closed. Campaign: " + inAppCampaign);
      MoEAndroidWrapper.getInstance().sendOrQueueCallback(Constants.METHOD_NAME_IN_APP_CLOSED,
          Utils.inAppCampaignToJson(inAppCampaign));
    } catch (Exception e) {
      Logger.e(TAG + " onClosed() : ", e);
    }
  }

  @Override public void onCustomAction(@NonNull MoEInAppCampaign inAppCampaign) {
    try {
      super.onCustomAction(inAppCampaign);
      Logger.v(
          TAG + " onCustomAction() : InApp Clicked with custom action. Campaign: " + inAppCampaign);
      if (inAppCampaign.customAction == null) {
        Logger.e(TAG + " onCustomAction() : Custom action object is null, cannot process "
            + "further.");
        return;
      }
      MoEAndroidWrapper.getInstance()
          .sendOrQueueCallback(Constants.METHOD_NAME_IN_APP_CUSTOM_ACTION,
              Utils.inAppCampaignToJson(inAppCampaign));
    } catch (Exception e) {
      Logger.e(TAG + " onCustomAction() : ", e);
    }
  }

  @Override public void onSelfHandledAvailable(@NonNull MoEInAppCampaign inAppCampaign) {
    try{
     super.onSelfHandledAvailable(inAppCampaign);
      Logger.v(TAG
          + " onSelfHandledAvailable() : In-App of type self handled received."
          + inAppCampaign);
      if (inAppCampaign.selfHandledCampaign == null){
        Logger.e( TAG + " onSelfHandledAvailable() : Self handled object is null, cannot process "
            + "further.");
        return;
      }
      MoEAndroidWrapper.getInstance()
          .sendOrQueueCallback(Constants.METHOD_NAME_IN_APP_SELF_HANDLED,
              Utils.inAppCampaignToJson(inAppCampaign));
    }catch(Exception e){
      Logger.e(TAG + " onSelfHandledAvailable() : ", e);
    }
  }
}
