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

import android.content.Context;
import com.moengage.core.internal.logger.Logger;
import com.moengage.core.internal.utils.MoEUtils;
import com.moengage.plugin.base.CallbackHelper;
import com.moengage.plugin.base.PluginHelper;
import com.moengage.plugin.base.model.PushService;
import org.json.JSONObject;

/**
 * Bridge between Unity and Android Native
 *
 * @author Umang Chamaria
 * Date: 26/06/20
 */
public class MoEAndroidWrapper {

  private static final String TAG = Constants.MODULE_TAG + "MoEAndroidWrapper";

  private PluginHelper pluginHelper;

  private MoEAndroidWrapper() {
    pluginHelper = new PluginHelper();
  }

  private static MoEAndroidWrapper instance = new MoEAndroidWrapper();

  public static MoEAndroidWrapper getInstance() {
    return instance;
  }

  private Context context;

  void setContext(Context context) {
    this.context = context;
  }

  public void initialize(String initializePayload) {
    try {
      Logger.v(TAG + " initialize() : Initialization payload: " + initializePayload);
      JSONObject initializationJson = new JSONObject(initializePayload);
      String gameObjectName = initializationJson.getString(ARGUMENT_GAME_OBJECT);
      if (MoEUtils.isEmptyString(gameObjectName)) {
        Logger.e(TAG + " initialize() : Game object name is empty cannot pass callbacks");
        return;
      }
      CallbackHelper.INSTANCE.setEventEmitter(new EventEmitterImpl(gameObjectName));
      pluginHelper.initialize();
    } catch (Exception e) {
      Logger.e(TAG + " initialise() : ", e);
    }
  }

  public void trackEvent(String eventPayload) {
    try {
      Logger.v(TAG + " trackEvent() : Event Payload: " + eventPayload);
      if (context == null) {
        Logger.e(TAG + " trackEvent() : Context is null cannot process further.");
        return;
      }
      pluginHelper.trackEvent(context, eventPayload);
    } catch (Exception e) {
      Logger.e(TAG + " trackEvent() : ", e);
    }
  }

  public void passPushPayload(String pushPayload) {
    try {
      pluginHelper.passPushPayload(context, pushPayload);
    } catch (Exception e) {
      Logger.e(TAG + " passPushPayload() : ", e);
    }
  }

  public void passPushToken(String tokenPayload) {
    try {
      Logger.v(TAG + " passPushToken() : Token Payload: " + tokenPayload);
      pluginHelper.passPushToken(context, tokenPayload);
    } catch (Exception e) {
      Logger.e(TAG + " passPushToken() : ", e);
    }
  }

  public void getSelfHandledInApp() {
    try {
      Logger.v(TAG + " getSelfHandledInApp() : Will try to fetch self-handled in-app");
      if (context == null) {
        Logger.e(TAG + " getSelfHandledInApp() : Context is null cannot process further.");
        return;
      }
      pluginHelper.getSelfHandledInApp(context);
    } catch (Exception e) {
      Logger.e(TAG + " getSelfHandledInApp() : ", e);
    }
  }

  public void showInApp() {
    try {
      Logger.v(TAG + " showInApp() : Will try to show in-app");
      if (context == null) {
        Logger.e(TAG + " showInApp() : Context is null cannot process further.");
        return;
      }
      pluginHelper.showInApp(context);
    } catch (Exception e) {
      Logger.e(TAG + " showInApp() : ", e);
    }
  }

  public void logout() {
    try {
      Logger.v(TAG + " logout() : Will try to logout user.");
      if (context == null) {
        Logger.e(TAG + " logout() : Context is null cannot process further.");
        return;
      }
      pluginHelper.logout(context);
    } catch (Exception e) {
      Logger.e(TAG + " logout() : ", e);
    }
  }

  public void setAlias(String aliasPayload) {
    try {
      Logger.v(TAG + " setAlias() : Alias Payload: " + aliasPayload);
      if (context == null) {
        Logger.e(TAG + " setAlias() : Context is null cannot process further.");
        return;
      }
      pluginHelper.setAlias(context, aliasPayload);
    } catch (Exception e) {
      Logger.e(TAG + " setAlias() : ", e);
    }
  }

  public void setAppStatus(String appStatusPayload) {
    try {
      Logger.v(TAG + " setAppStatus() : App status payload: " + appStatusPayload);
      if (context == null) {
        Logger.e(TAG + " setAppStatus() : Context is null cannot process further.");
        return;
      }
      pluginHelper.setAppStatus(context, appStatusPayload);
    } catch (Exception e) {
      Logger.e(TAG + " setAppStatus() : ", e);
    }
  }

  public void setUserAttribute(String userAttributePayload) {
    try {
      Logger.v(TAG + " setUserAttribute() : User Attribute payload: " + userAttributePayload);
      if (context == null) {
        Logger.e(TAG + " setUserAttribute() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.setUserAttribute(context, userAttributePayload);
    } catch (Exception e) {
      Logger.e(TAG + " setUserAttribute() : ", e);
    }
  }

  public void setAppContext(String contextPayload) {
    try {
      Logger.v(TAG + " setAppContext() : Context Payload: " + contextPayload);
      if (context == null) {
        Logger.e(TAG + " setAppContext() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.setAppContext(context, contextPayload);
    } catch (Exception e) {
      Logger.e(TAG + " setAppContext() : ", e);
    }
  }

  public void resetContext() {
    try {
      Logger.v(TAG + " resetContext() : Resetting app context");
      if (context == null) {
        Logger.e(TAG + " resetContext() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.resetAppContext(context);
    } catch (Exception e) {
      Logger.e(TAG + " resetContext() : ", e);
    }
  }

  public void selfHandledShown(String selfHandledPayload) {
    try {
      Logger.v(TAG + " selfHandledShown() : Campaign payload: " + selfHandledPayload);
      if (context == null) {
        Logger.e(TAG + " selfHandledShown() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.selfHandledCallback(context, selfHandledPayload);
    } catch (Exception e) {
      Logger.e(TAG + " selfHandledShown() : ", e);
    }
  }

  public void selfHandledClicked(String selfHandledPayload) {
    try {
      Logger.v(TAG + " selfHandledClicked() : Campaign payload: " + selfHandledPayload);
      if (context == null) {
        Logger.e(TAG + " selfHandledClicked() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.selfHandledCallback(context, selfHandledPayload);
    } catch (Exception e) {
      Logger.e(TAG + " selfHandledClicked() : ", e);
    }
  }

  public void selfHandledDismissed(String selfHandledPayload) {
    try {
      Logger.v(TAG + " selfHandledDismissed() : Campaign payload: " + selfHandledPayload);
      if (context == null) {
        Logger.e(TAG + " selfHandledDismissed() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.selfHandledCallback(context, selfHandledPayload);
    } catch (Exception e) {
      Logger.e(TAG + " selfHandledDismissed() : ", e);
    }
  }

  public void enableSDKLogs() {
    Logger.v(TAG + " enableSDKLogs(): Enabling SDK logs.");
    pluginHelper.enableSDKLogs();
  }

  public void selfHandledCallback(String selfHandledPayload) {
    try {
      Logger.v(TAG + " selfHandledCallback() : Campaign payload: " + selfHandledPayload);
      if (context == null) {
        Logger.e(TAG + " selfHandledCallback() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.selfHandledCallback(context, selfHandledPayload);
    } catch (Exception e) {
      Logger.e(TAG + " selfHandledCallback() : ", e);
    }
  }

  public void optOutTracking(String optOutPayload) {
    try {
      Logger.v(TAG + " optOutTracking() : OptOut payload: " + optOutPayload);
      if (context == null) {
        Logger.e(TAG + " optOutTracking() : Cannot proceed further context is null.");
        return;
      }
      pluginHelper.optOutTracking(context, optOutPayload);
    } catch (Exception e) {
      Logger.e(TAG + " optOutTracking() : ", e);
    }
  }

  public void updateSdkState(String featureStatusPayload){
    try{
      Logger.v(TAG + " storeFeatureStatus() : Feature status payload: " + featureStatusPayload);
      if (context == null){
        Logger.e( TAG + " storeFeatureStatus() : Cannot proceed further context is null.");
      }
      pluginHelper.storeFeatureStatus(context, featureStatusPayload);
    }catch (Exception e){
      Logger.e( TAG + " storeFeatureStatus() : ", e);
    }
  }

  private static final String ARGUMENT_GAME_OBJECT = "gameObjectName";
}
