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

package com.moengage.unity.wrapper;

/**
 * @author Umang Chamaria
 * Date: 26/06/20
 */
interface Constants {
  String MODULE_TAG = "Unity_";
  String INTEGRATION_TYPE = "unity";
  String INTEGRATION_VERSION = "1.0.1";

  String PLATFORM_NAME = "android";

  String PARAM_PLATFORM = "platform";
  String PARAM_PAYLOAD = "payload";
  String PARAM_CAMPAIGN_NAME = "campaignName";
  String PARAM_CAMPAIGN__ID = "campaignId";
  String PARAM_CUSTOM_ACTION = "customAction";
  String PARAM_NAVIGATION_ACTION = "navigation";
  String PARAM_NAVIGATION_TYPE = "navigationType";
  String PARAM_NAVIGATION_URL = "value";
  String PARAM_KEY_VALUE_PAIR = "kvPair";
  String PARAM_DISMISS_INTERVAL = "dismissInterval";
  String PARAM_IS_CANCELLABLE = "isCancellable";
  String PARAM_SELF_HANDLED = "selfHandled";

  String METHOD_NAME_PUSH_REDIRECTION = "PushClicked";
  String METHOD_NAME_IN_APP_SHOWN = "InAppCampaignShown";
  String METHOD_NAME_IN_APP_CLICKED = "InAppCampaignClicked";
  String METHOD_NAME_IN_APP_CLOSED = "InAppCampaignDismissed";
  String METHOD_NAME_IN_APP_CUSTOM_ACTION = "InAppCampaignCustomAction";
  String METHOD_NAME_IN_APP_SELF_HANDLED = "InAppCampaignSelfHandled";

  String ATTRIBUTE_TYPE_GENERAL = "general";
  String ATTRIBUTE_TYPE_TIMESTAMP = "timestamp";
  String ATTRIBUTE_TYPE_LOCATION = "location";

}
