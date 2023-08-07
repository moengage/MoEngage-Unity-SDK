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
using System.Runtime.InteropServices;
using UnityEngine;
using MoEMiniJSON;

namespace MoEngage
{

#if UNITY_IOS
    public class MoEngageiOS: MoEngageUnityPlatform
    {
        #region DLL Imports

        [DllImport("__Internal")]
        private static extern void initialize(string gameObjPayload);

        [DllImport("__Internal")]
        private static extern void setAppStatus(string appStatusPayload);

        [DllImport("__Internal")]
        private static extern void setAlias(string aliasPayload);

        [DllImport("__Internal")]
        private static extern void setUserAttribute(string userAttrPayload);

        [DllImport("__Internal")]
        private static extern void trackEvent(string eventPayload);

        [DllImport("__Internal")]
        private static extern void resetUser(string accountPayload);

        [DllImport("__Internal")]
        private static extern void registerForPush();

        [DllImport("__Internal")]
        private static extern void showInApp(string accountPayload);

        [DllImport("__Internal")]
        private static extern void setInAppContexts(string contextsPayload);

        [DllImport("__Internal")]
        private static extern void invalidateInAppContexts(string accountPayload);

        [DllImport("__Internal")]
        private static extern void getSelfHandledInApp(string accountPayload);

        [DllImport("__Internal")]
        private static extern void updateSelfHandledInAppStatusWithPayload(string selfHandledPayload);

        [DllImport("__Internal")]
        private static extern void optOutGDPRTracking(string optOutPayload);

        [DllImport("__Internal")]
        private static extern void updateSdkState(string payload);

        #endregion

        #region Initialize

        public void Initialize(string gameObjPayload)
        {
#if !UNITY_EDITOR
			initialize(gameObjPayload);
#endif
        }

        #endregion

        #region AppStatus 
        public void SetAppStatus(string appStatusPayload)
        {
#if !UNITY_EDITOR
			setAppStatus(appStatusPayload);
#endif
        }

        #endregion

        #region UserAttribute Tracking
        public void SetAlias(string aliasPayload)
        {
#if !UNITY_EDITOR
			setAlias(aliasPayload);
#endif
        }


        public void SetUserAttribute(string userAttributesPayload)
        {
#if !UNITY_EDITOR
			setUserAttribute(userAttributesPayload);
#endif
        }

        #endregion

        #region Event Tracking

        public void TrackEvent(string eventPayload)
        {
#if !UNITY_EDITOR
			trackEvent(eventPayload);
#endif
        }

        #endregion

        #region Push Notifications

        public static void RegisterForPush()
        {
#if !UNITY_EDITOR
			registerForPush();
#endif
        }

        #endregion

        #region InApp Methods

        public void ShowInApp(string accountPayload)
        {
#if !UNITY_EDITOR
			showInApp(accountPayload);
#endif
        }

        public void SetInAppContexts(string contextPayload)
        {
#if !UNITY_EDITOR
			setInAppContexts(contextPayload);	
#endif
        }

        public void ResetInAppContexts(string accountPayload)
        {
#if !UNITY_EDITOR
			invalidateInAppContexts(accountPayload);
#endif
        }

        public void GetSelfHandledInApp(string accountPayload)
        {
#if !UNITY_EDITOR
			getSelfHandledInApp(accountPayload);
#endif
        }

        public void SelfHandledShown(string selfHandledPayload)
        {
#if !UNITY_EDITOR
			updateSelfHandledInAppStatusWithPayload(selfHandledPayload);	
#endif
        }

        public void SelfHandledClicked(string selfHandledPayload)
        {
#if !UNITY_EDITOR
			updateSelfHandledInAppStatusWithPayload(selfHandledPayload);	
#endif
        }

        public void SelfHandledDismissed(string selfHandledPayload)
        {
#if !UNITY_EDITOR
			updateSelfHandledInAppStatusWithPayload(selfHandledPayload);	
#endif
        }

        #endregion

       #region Utils and OptOuts

        public void optOutDataTracking(string optOutPayload)
	{
#if !UNITY_EDITOR
		optOutGDPRTracking(optOutPayload);
#endif
	}

        #endregion

        #region Reset User

        public void Logout(string accountPayload)
        {
#if !UNITY_EDITOR
		resetUser(accountPayload);
#endif
        }

        #endregion

        #region Reset User

        public void UpdateSdkState(string payload)
        {
#if !UNITY_EDITOR
		updateSdkState(payload);
#endif
	}
        #endregion

    }

#endif
}
