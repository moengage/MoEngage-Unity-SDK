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

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MoEngage
{
    public class MoEngageSettingsWindow : EditorWindow
    {
        private MoEngageSettings _settings;
        private SerializedObject _serializedSettings;
        private Vector2 _scrollPos;

        [MenuItem("Assets/MoEngage Settings")]
        public static void Open()
        {
            var window = GetWindow<MoEngageSettingsWindow>("MoEngage Settings");
            window.minSize = new Vector2(400, 500);
            window.Show();
        }

        private void OnEnable()
        {
            LoadOrCreateSettings();
        }

        private void LoadOrCreateSettings()
        {
            _settings = AssetDatabase.LoadAssetAtPath<MoEngageSettings>(MoEngageSettings.SettingsAssetPath);
            if (_settings == null)
            {
                _settings = CreateInstance<MoEngageSettings>();
                AssetDatabase.CreateAsset(_settings, MoEngageSettings.SettingsAssetPath);
                AssetDatabase.SaveAssets();
                Debug.Log("MoEngage: Created settings asset at " + MoEngageSettings.SettingsAssetPath);
            }
            _serializedSettings = new SerializedObject(_settings);
        }

        private void OnGUI()
        {
            if (_settings == null)
            {
                LoadOrCreateSettings();
                return;
            }

            _serializedSettings.Update();

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("MoEngage SDK Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("Common", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var name in new[] { "WorkspaceId", "ProjectId", "DataCenter", "TestEnvironment" })
            {
                var prop = _serializedSettings.FindProperty(name);
                if (prop != null) EditorGUILayout.PropertyField(prop, true);
            }
            var isLoggingProp = _serializedSettings.FindProperty("IsLoggingEnabled");
            if (isLoggingProp != null) EditorGUILayout.PropertyField(isLoggingProp, true);
            if (_settings.IsLoggingEnabled)
            {
                var logLevelProp = _serializedSettings.FindProperty("LogLevel");
                if (logLevelProp != null) EditorGUILayout.PropertyField(logLevelProp, true);
            }
            foreach (var name in new[] { "CustomBaseDomain", "IsJwtEnabled" })
            {
                var prop = _serializedSettings.FindProperty(name);
                if (prop != null) EditorGUILayout.PropertyField(prop, true);
            }
            var isNetworkEncryptionProp = _serializedSettings.FindProperty("IsNetworkEncryptionEnabled");
            if (isNetworkEncryptionProp != null) EditorGUILayout.PropertyField(isNetworkEncryptionProp, true);
            if (_settings.IsNetworkEncryptionEnabled)
            {
                var liveProp = _serializedSettings.FindProperty("EncryptionEncodedLiveKey");
                if (liveProp != null) EditorGUILayout.PropertyField(liveProp, true);
                var testProp = _serializedSettings.FindProperty("EncryptionEncodedTestKey");
                if (testProp != null) EditorGUILayout.PropertyField(testProp, true);
            }
            foreach (var name in new[] { "IsStorageEncryptionEnabled", "EnablePeriodicDataSync", "DataSyncInterval" })
            {
                var prop = _serializedSettings.FindProperty(name);
                if (prop != null) EditorGUILayout.PropertyField(prop, true);
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("iOS Only", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var name in new[] { "AppGroupName", "KeychainGroupName", "InAppDisplaySafeAreaInset",
                "InAppShouldProvideDeeplinkCallback" })
            {
                var prop = _serializedSettings.FindProperty(name);
                if (prop != null) EditorGUILayout.PropertyField(prop, true);
            }
            foreach (var name in new[] { "IsUnityAppControllerSwizzlingEnabled", "IsSdkAutoInitialisationEnabled", "IsUserRegistrationEnabled" })
            {
                var prop = _serializedSettings.FindProperty(name);
                if (prop != null) EditorGUILayout.PropertyField(prop, true);
            }
            EditorGUI.indentLevel--;

            DrawSection("Android Only", new[]
            {
                "IntegrationPartner", "EnableCarrierTracking",
                "PushSmallIcon", "PushLargeIcon", "PushNotificationColor",
                "GroupMultipleNotifications", "EnablePushBackStackBuilding",
                "EnableNotificationLargeIconDisplay", "EnableHeadsUpNotification",
                "EncryptionEncodedDebugKey", "EncryptionEncodedReleaseKey"
            });

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(8);
            if (GUILayout.Button("Save"))
            {
                Save();
            }
            EditorGUILayout.Space(4);

            if (_serializedSettings.ApplyModifiedProperties())
            {
                Save();
            }
        }

        private void DrawSection(string title, string[] propertyNames)
        {
            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var name in propertyNames)
            {
                var prop = _serializedSettings.FindProperty(name);
                if (prop != null)
                    EditorGUILayout.PropertyField(prop, true);
            }
            EditorGUI.indentLevel--;
        }

        private void Save()
        {
            _serializedSettings.ApplyModifiedProperties();
            EditorUtility.SetDirty(_settings);
            AssetDatabase.SaveAssets();
        }
    }
}

#endif
