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

import android.os.Bundle;
import com.moengage.core.utils.JsonBuilder;
import com.moengage.inapp.model.MoEInAppCampaign;
import com.moengage.inapp.model.SelfHandledCampaign;
import java.util.Map;
import java.util.Map.Entry;
import java.util.Set;
import org.json.JSONException;
import org.json.JSONObject;

/**
 * @author Umang Chamaria
 * Date: 27/06/20
 */
class Utils {
  private static final String TAG = Constants.MODULE_TAG + "Utils";

  public static JSONObject bundleToJson(Bundle bundle) throws JSONException {
    Set<String> keys = bundle.keySet();
    JSONObject jsonObject = new JSONObject();
    for (String key : keys) {
      jsonObject.put(key, bundle.get(key));
    }
    return jsonObject;
  }

  public static JSONObject mapToJson(Map<String, Object> map) throws JSONException {
    JSONObject jsonObject = new JSONObject();
    for (Entry<String, Object> entry : map.entrySet()) {
      jsonObject.put(entry.getKey(), entry.getValue());
    }
    return jsonObject;
  }

  public static JSONObject inAppCampaignToJson(MoEInAppCampaign campaign) throws JSONException {
    JsonBuilder inAppJsonBuilder = new JsonBuilder();
    inAppJsonBuilder.putString(Constants.PARAM_CAMPAIGN_NAME, campaign.campaignName)
        .putString(Constants.PARAM_CAMPAIGN__ID, campaign.campaignId);
    if (campaign.navigationAction != null) {
      JsonBuilder navigationJson = new JsonBuilder();
      navigationJson.putString(Constants.PARAM_NAVIGATION_TYPE,
          campaign.navigationAction.navigationType.toString().toLowerCase())
          .putString(Constants.PARAM_NAVIGATION_URL, campaign.navigationAction.navigationUrl);
      if (campaign.navigationAction.keyValuePairs != null) {
        navigationJson.putJsonObject(Constants.PARAM_KEY_VALUE_PAIR,
            Utils.mapToJson(campaign.navigationAction.keyValuePairs));
      } else {
        navigationJson.putJsonObject(Constants.PARAM_KEY_VALUE_PAIR, new JSONObject());
      }
      inAppJsonBuilder.putJsonObject(Constants.PARAM_NAVIGATION_ACTION, navigationJson.build());
    }
    if (campaign.customAction != null) {
      inAppJsonBuilder.putJsonObject(Constants.PARAM_CUSTOM_ACTION,
          Utils.mapToJson(campaign.customAction.keyValuePairs));
    }
    if (campaign.selfHandledCampaign != null) {
      JsonBuilder selfHandledJson = new JsonBuilder();
      selfHandledJson.putString(Constants.PARAM_PAYLOAD, campaign.selfHandledCampaign.payload)
          .putLong(Constants.PARAM_DISMISS_INTERVAL, campaign.selfHandledCampaign.dismissInterval)
          .putBoolean(Constants.PARAM_IS_CANCELLABLE, campaign.selfHandledCampaign.isCancellable);
      inAppJsonBuilder.putJsonObject(Constants.PARAM_SELF_HANDLED, selfHandledJson.build());
    }
    return inAppJsonBuilder.build();
  }

  public static MoEInAppCampaign jsonToInAppCampaign(JSONObject campaignJson) throws JSONException {
    JSONObject selfHandledJson = campaignJson.getJSONObject(Constants.PARAM_SELF_HANDLED);
    SelfHandledCampaign selfHandledCampaign =
        new SelfHandledCampaign(selfHandledJson.getString(Constants.PARAM_PAYLOAD),
            selfHandledJson.getLong(Constants.PARAM_DISMISS_INTERVAL),
            selfHandledJson.getBoolean(Constants.PARAM_IS_CANCELLABLE));
    return new MoEInAppCampaign(campaignJson.getString(Constants.PARAM_CAMPAIGN__ID),
        campaignJson.getString(Constants.PARAM_CAMPAIGN_NAME), selfHandledCampaign);
  }
}
