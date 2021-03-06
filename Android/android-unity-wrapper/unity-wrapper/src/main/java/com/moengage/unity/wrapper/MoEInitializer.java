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
import com.moengage.core.MoEngage;
import com.moengage.core.internal.logger.Logger;
import com.moengage.core.internal.model.IntegrationMeta;
import com.moengage.plugin.base.PluginInitializer;

/**
 * @author Umang Chamaria
 * Date: 26/06/20
 */
public class MoEInitializer {

  private static final String TAG = Constants.MODULE_TAG + "MoEInitializer";

  public static void initialize(Context context, MoEngage.Builder builder) {
    try {
      Logger.v(TAG + " initialize() : Initialising MoEngage SDK.");
      initialize(context, builder, true);
    } catch (Exception e) {
      Logger.e(TAG + " initialize() : ", e);
    }
  }

  public static void initialize(Context context, MoEngage.Builder builder, boolean isSdkEnabled) {
    try {
      Logger.v(TAG + " initialize() : Initialising MoEngage SDK.");
      MoEAndroidWrapper.getInstance().setContext(context);
      PluginInitializer.INSTANCE.initialize(context, builder,
          (new IntegrationMeta(Constants.INTEGRATION_TYPE, Constants.INTEGRATION_VERSION)),
          isSdkEnabled);
      Logger.v(TAG + " initialize() : Initialising MoEngage SDK.");
    } catch (Exception e) {
      Logger.e( TAG + " initialize() : ", e);
    }
  }
}
