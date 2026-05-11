/*
 * Copyright (c) 2014-2024 MoEngage Inc.
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

using UnityEngine;

namespace MoEngage
{
    public enum MoEDataCenter
    {
        DATA_CENTER_1 = 1,
        DATA_CENTER_2 = 2,
        DATA_CENTER_3 = 3,
        DATA_CENTER_4 = 4,
        DATA_CENTER_5 = 5,
        DATA_CENTER_6 = 6
    }

    public enum MoELogLevel
    {
        NO_LOG = 0,
        ERROR = 1,
        WARN = 2,
        INFO = 3,
        DEBUG = 4,
        VERBOSE = 5
    }

    public enum MoETestEnvironment
    {
        Default,
        Test,
        Live
    }

    public class MoEngageSettings : ScriptableObject
    {
        public static readonly string SettingsAssetPath = "Assets/MoEngageSettings.asset";

        // ─── Common ──────────────────────────────────────────────────────────────
        [Header("Common")]
        [Tooltip("Your MoEngage Workspace ID (required).")]
        public string WorkspaceId = "";

        [Tooltip("Your MoEngage Project ID.")]
        public string ProjectId = "";

        [Tooltip("Data center region for your MoEngage account.")]
        public MoEDataCenter DataCenter = MoEDataCenter.DATA_CENTER_1;

        [Tooltip("APNs push environment. Default lets the SDK auto-detect; Test sets isTestEnvironment=true; Live sets isTestEnvironment=false.")]
        public MoETestEnvironment TestEnvironment = MoETestEnvironment.Default;

        [Tooltip("Enable SDK logging for both Release and Debug builds.")]
        public bool IsLoggingEnabled = false;

        [Tooltip("Log verbosity level.")]
        public MoELogLevel LogLevel = MoELogLevel.NO_LOG;

        [Tooltip("Custom base domain for network calls (leave empty to use default).")]
        public string CustomBaseDomain = "";

        [Tooltip("Enables JWT-based user authentication.")]
        public bool IsJwtEnabled = false;

        [Tooltip("Encrypts outbound network traffic.")]
        public bool IsNetworkEncryptionEnabled = false;

        [Tooltip("Encoded public key for network encryption in live builds.")]
        public string EncryptionEncodedLiveKey = "";

        [Tooltip("Encoded public key for network encryption in test builds.")]
        public string EncryptionEncodedTestKey = "";

        [Tooltip("Encrypts data stored locally by the SDK.")]
        public bool IsStorageEncryptionEnabled = false;

        [Tooltip("Enables periodic background data sync.")]
        public bool EnablePeriodicDataSync = true;

        [Tooltip("Interval in seconds for periodic data sync.")]
        public int DataSyncInterval = 60;

        // ─── iOS Only ─────────────────────────────────────────────────────────────
        [Header("iOS Only")]
        [Tooltip("App Group identifier shared with Notification Service Extension (e.g. group.com.example.app.moengage).")]
        public string AppGroupName = "";

        [Tooltip("Keychain access group for secure storage sharing across targets.")]
        public string KeychainGroupName = "";

        [Tooltip("Safe area inset applied around in-app message display.")]
        public float InAppDisplaySafeAreaInset = 0f;

        [Tooltip("Enables custom deep link handling callbacks from in-app messages.")]
        public bool InAppShouldProvideDeeplinkCallback = false;

        [Tooltip("Enables swizzling of the Unity App Controller.")]
        public bool IsUnityAppControllerSwizzlingEnabled = true;

        [Tooltip("Enables auto-initialization of the SDK on app launch.")]
        public bool IsSdkAutoInitialisationEnabled = true;

        [Tooltip("Enables user registration tracking on iOS.")]
        public bool IsUserRegistrationEnabled = false;
    }
}
