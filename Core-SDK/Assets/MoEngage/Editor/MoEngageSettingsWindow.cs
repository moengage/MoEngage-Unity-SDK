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

            DrawSection("Common", () =>
            {
                DrawFields("WorkspaceId", "ProjectId", "DataCenter", "TestEnvironment");
                DrawField("IsLoggingEnabled");
                if (_settings.IsLoggingEnabled)
                    DrawField("LogLevel");
                DrawFields("CustomBaseDomain", "IsJwtEnabled");
                DrawField("IsNetworkEncryptionEnabled");
                if (_settings.IsNetworkEncryptionEnabled)
                    DrawFields("EncryptionEncodedLiveKey", "EncryptionEncodedTestKey");
                DrawFields("IsStorageEncryptionEnabled", "EnablePeriodicDataSync", "DataSyncInterval");
            });

            DrawSection("iOS Only", () =>
            {
                DrawFields(
                    "KeychainGroupName", "InAppDisplaySafeAreaInset",
                    "InAppShouldProvideDeeplinkCallback", "IsUnityAppControllerSwizzlingEnabled",
                    "IsSdkAutoInitialisationEnabled", "IsUserRegistrationEnabled"
                );
            });

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(8);
            if (GUILayout.Button("Save"))
                Save();
            EditorGUILayout.Space(4);

            if (_serializedSettings.ApplyModifiedProperties())
                Save();
        }

        private void DrawSection(string label, System.Action content)
        {
            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            content();
            EditorGUI.indentLevel--;
        }

        private void DrawField(string name)
        {
            var prop = _serializedSettings.FindProperty(name);
            if (prop != null) EditorGUILayout.PropertyField(prop, true);
        }

        private void DrawFields(params string[] names)
        {
            foreach (var name in names)
                DrawField(name);
        }

        private void Save()
        {
            EditorUtility.SetDirty(_settings);
            AssetDatabase.SaveAssets();
        }
    }
}

#endif
