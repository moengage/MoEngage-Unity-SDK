#define ADD_APP_GROUP
#define ADD_PUSH_TEMPLATES

#if UNITY_5_4_OR_NEWER && UNITY_IPHONE && UNITY_EDITOR

using System.IO;
using System;
using System.Xml;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Text;
using System.Collections.Generic;
using UnityEditor.Build;
using MoEngage;

#if UNITY_2017_2_OR_NEWER
using UnityEditor.iOS.Xcode.Extensions;
#endif

public static class BuildPostProcessor
{
    public static readonly string DEFAULT_PROJECT_TARGET_NAME = "Unity-iPhone";
    public static readonly string NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME = "MoENotificationServiceExtension";
    public static readonly string NOTIFICATION_SERVICE_EXTENSION_OBJECTIVEC_FILENAME = "NotificationService";

    public static readonly string PUSH_TEMPLATES_EXTENSION_TARGET_NAME = "MoEPushTemplateExtension";
    public static readonly string PUSH_TEMPLATES_EXTENSION_OBJECTIVEC_FILENAME = "NotificationViewController";

    private static readonly char DIR_CHAR = Path.DirectorySeparatorChar;
    public static readonly string MOE_IOS_LOCATION = "Assets" + DIR_CHAR + "MoEngage" + DIR_CHAR + "Plugins" + DIR_CHAR + "iOS";
    public static readonly string MOE_IOS_PUSH_TEMP_LOCATION = MOE_IOS_LOCATION + DIR_CHAR + "PushTemplates";

    private static readonly string[] FRAMEWORKS_TO_ADD = {
         "Foundation.framework",
         "UIKit.framework",
         "SystemConfiguration.framework",
         "CoreGraphics.framework",
         "Security.framework",
         "ImageIO.framework",
         "NotificationCenter.framework",
         "UserNotifications.framework",
         "UserNotificationsUI.framework"
      };

    private enum EntitlementOptions
    {
        ApsEnv,
        AppGroups,
        KeychainSharing
    }

    // Unity 2019.3 made large changes to the Xcode build system / API.
    // There is now two targets;
    //  * Unity-Iphone (Main)
    //  * UnityFramework
    //     - Plugins are now added to this instead of the main target
#if UNITY_2019_3_OR_NEWER
    private static string GetPBXProjectTargetName(PBXProject project)
    {
        // var projectUUID = project.GetUnityMainTargetGuid();
        // return project.GetBuildPhaseName(projectUUID);
        // The above always returns null, using a static value for now.
        return DEFAULT_PROJECT_TARGET_NAME;
    }

    private static string GetPBXProjectTargetGUID(PBXProject project)
    {
        return project.GetUnityMainTargetGuid();
    }

    private static string GetPBXProjectUnityFrameworkGUID(PBXProject project)
    {
        return project.GetUnityFrameworkTargetGuid();
    }
#else
         private static string GetPBXProjectTargetName(PBXProject project)
         {
            return PBXProject.GetUnityTargetName();
         }

         private static string GetPBXProjectTargetGUID(PBXProject project)
         { 
            return project.TargetGuidByName(PBXProject.GetUnityTargetName());
         }

         private static string GetPBXProjectUnityFrameworkGUID(PBXProject project)
         {
            return GetPBXProjectTargetGUID(project);
         }
#endif

    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var project = new PBXProject();

        project.ReadFromString(File.ReadAllText(projectPath));

        var mainTargetName = GetPBXProjectTargetName(project);
        var mainTargetGUID = GetPBXProjectTargetGUID(project);
        var unityFrameworkGUID = GetPBXProjectUnityFrameworkGUID(project);

        var settings = AssetDatabase.LoadAssetAtPath<MoEngage.MoEngageSettings>(
            MoEngage.MoEngageSettings.SettingsAssetPath);
        var keychainGroup = settings != null ? settings.KeychainGroupName : null;
        var appGroup = GetAppGroupId();

        foreach (var framework in FRAMEWORKS_TO_ADD)
        {
            project.AddFrameworkToProject(unityFrameworkGUID, framework, false);
        }

        // Weak link AppTrackingTransparency framework
        project.AddFrameworkToProject(unityFrameworkGUID,"AppTrackingTransparency.framework", true);

        project.SetBuildProperty(project.GetUnityMainTargetGuid(), "ENABLE_BITCODE", "NO");
        project.SetBuildProperty(unityFrameworkGUID, "ENABLE_BITCODE", "NO"); // Disabled to run on Xcode 14+
        project.SetBuildProperty(mainTargetGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "Yes");
        // @import requires modules to be enabled; UnityFramework hosts the ObjC bridge files, main target needs it too for SPM
        project.SetBuildProperty(unityFrameworkGUID, "CLANG_ENABLE_MODULES", "YES");
        project.SetBuildProperty(mainTargetGUID, "CLANG_ENABLE_MODULES", "YES");
        project.SetBuildProperty(mainTargetGUID, "IPHONEOS_DEPLOYMENT_TARGET", "13.0");
        // SPM Swift packages linked into UnityFramework require Swift stdlib embedded in the main app bundle
        // project.SetBuildProperty(mainTargetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        project.AddBuildProperty(mainTargetGUID, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
        project.AddBuildProperty(unityFrameworkGUID, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");

        var mainEntitlementOptions = new HashSet<EntitlementOptions> {
            EntitlementOptions.ApsEnv, EntitlementOptions.AppGroups
        };
        if (!string.IsNullOrEmpty(keychainGroup))
            mainEntitlementOptions.Add(EntitlementOptions.KeychainSharing);

        AddOrUpdateEntitlements(path, project, mainTargetGUID, mainTargetName, mainEntitlementOptions, appGroup, keychainGroup);

        // Add the NSE target to the Xcode project
        AddNotificationServiceExtension(project, path, appGroup, keychainGroup);

#if ADD_PUSH_TEMPLATES
        AddPushTemplateExtension(project, path, appGroup);
#endif

        // Reload file after changes from AddNotificationServiceExtension
        project.WriteToFile(projectPath);
        var contents = File.ReadAllText(projectPath);
        project.ReadFromString(contents);

        // Add push notifications as a capability on the main app target
        AddPushCapability(project, path, mainTargetGUID, mainTargetName);

        File.WriteAllText(projectPath, project.WriteToString());

        MergeMoEngageInfoPlist(path);

        RemoveExtensionFilesFromMainTarget(path);
    }

    private static void RemoveExtensionFilesFromMainTarget(string path) {
        string projPath = PBXProject.GetPBXProjectPath(path);
        PBXProject project = new PBXProject();
        project.ReadFromFile(projPath);
        string targetGuid = project.TargetGuidByName("UnityFramework");

        //Remove File From Build
        project.RemoveFileFromBuild(targetGuid, "NotificationServices.m");
        var notificationServiceFile = project.FindFileGuidByProjectPath("Libraries/MoEngage/Plugins/iOS/NotificationService.m");
        project.RemoveFile(notificationServiceFile);
        project.RemoveFrameworkFromProject(targetGuid, "NotificationService.m");

        project.RemoveFileFromBuild(targetGuid, "NotificationServices.h");
        var notificationServiceHeaderFile = project.FindFileGuidByProjectPath("Libraries/MoEngage/Plugins/iOS/NotificationService.h");
        project.RemoveFile(notificationServiceHeaderFile);
        project.RemoveFrameworkFromProject(targetGuid, "NotificationService.h");

        project.RemoveFileFromBuild(targetGuid, "NotificationViewController.h");
        var notificationViewControllerHeaderFile = project.FindFileGuidByProjectPath("Libraries/MoEngage/Plugins/iOS/PushTemplates/NotificationViewController.h");
        project.RemoveFile(notificationViewControllerHeaderFile);
        project.RemoveFrameworkFromProject(targetGuid, "NotificationViewController.h");

        project.RemoveFileFromBuild(targetGuid, "NotificationViewController.m");
        var notificationViewControllerFile = project.FindFileGuidByProjectPath("Libraries/MoEngage/Plugins/iOS/PushTemplates/NotificationViewController.m");
        project.RemoveFile(notificationViewControllerFile);
        project.RemoveFrameworkFromProject(targetGuid, "NotificationViewController.m");

        File.WriteAllText(projPath, project.WriteToString());
  }
  
    
    // Runs between Podfile generation (40) and pod install (50) — CocoaPods path only.
    [PostProcessBuildAttribute(47)]
    public static void PostProcessBuild_iOS_Pods(BuildTarget target, string buildPath)
    {
#if UNITY_EDITOR
        if (!IsSPMEnabled())
        {
            using (StreamWriter sw = File.AppendText(buildPath + "/Podfile"))
            {
                sw.WriteLine("\ntarget '" + NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME + "' do\n  pod 'MoEngage-iOS-SDK/RichNotification' \nend");
                sw.WriteLine("\ntarget '" + PUSH_TEMPLATES_EXTENSION_TARGET_NAME + "' do\n  pod 'MoEngage-iOS-SDK/RichNotification' \nend");
            }
        }
#endif
    }

    // Runs after EDM4U SPM resolution (~75) so AddRemotePackageReferenceAtVersion calls are not overwritten.
    [PostProcessBuildAttribute(77)]
    public static void PostProcessBuild_iOS_SPM(BuildTarget target, string buildPath)
    {
#if UNITY_EDITOR
        if (IsSPMEnabled())
        {
            var projectPath = PBXProject.GetPBXProjectPath(buildPath);
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

            var mainTargetGUID = project.GetUnityMainTargetGuid();
            var nseGUID = project.TargetGuidByName(NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME);
            var pushTemplateGUID = project.TargetGuidByName(PUSH_TEMPLATES_EXTENSION_TARGET_NAME);

            var appleSDKRefGUID = project.AddRemotePackageReferenceAtVersion("https://github.com/moengage/apple-sdk.git", "10.12.0");
            if (nseGUID != null)
                project.AddRemotePackageFrameworkToProject(nseGUID, "MoEngageRichNotification", appleSDKRefGUID, false);
            if (pushTemplateGUID != null)
                project.AddRemotePackageFrameworkToProject(pushTemplateGUID, "MoEngageRichNotification", appleSDKRefGUID, false);

            // Dynamic xcframeworks from apple-sdk are transitive dependencies of the static
            // MoEngagePluginBase linked into UnityFramework. They must be linked to the main
            // target so Xcode embeds them into MoEngage.app/Frameworks/ for @rpath resolution.
            // Only exposed products can be referenced; MoEngageCampaignsCore is an internal
            // binary target, not a product — it is embedded transitively via MoEngage-iOS-SDK.
            foreach (var framework in new[] {
                "MoEngage-iOS-SDK", "MoEngageInApps", "MoEngageTriggerEvaluator", "MoEngageRichNotification"
            })
            {
                project.AddRemotePackageFrameworkToProject(mainTargetGUID, framework, appleSDKRefGUID, false);
            }

            var cardsXml = Path.Combine(Application.dataPath, "Cards", "Editor", "CardsDependencies.xml");
            if (File.Exists(cardsXml))
                project.AddRemotePackageFrameworkToProject(mainTargetGUID, "MoEngageCards", appleSDKRefGUID, false);


            File.WriteAllText(projectPath, project.WriteToString());
        }
#endif
    }

    // Returns exisiting file if found, otherwises provides a default name to use
    private static string GetEntitlementsPath(string path, PBXProject project, string targetGUI, string targetName)
    {
        // Check if there is already an eltitlements file configured in the Xcode project
#if UNITY_2018_2_OR_NEWER
        var relativeEntitlementPath = project.GetBuildPropertyForConfig(targetGUI, "CODE_SIGN_ENTITLEMENTS");
        if (relativeEntitlementPath != null)
        {
            var entitlementPath = path + DIR_CHAR + relativeEntitlementPath;
            if (File.Exists(entitlementPath))
            {
                return entitlementPath;
            }
        }
#endif

        // No existing file, use a new name
        return path + DIR_CHAR + targetName + DIR_CHAR + targetName + ".entitlements";
    }

    private static void AddOrUpdateEntitlements(string path, PBXProject project, string targetGUI, string targetName, HashSet<EntitlementOptions> options, string appGroup, string keychainGroup = null)
    {
        string entitlementPath = GetEntitlementsPath(path, project, targetGUI, targetName);
        var entitlements = new PlistDocument();

        // Check if the file already exisits and read it
        if (File.Exists(entitlementPath))
        {
            entitlements.ReadFromFile(entitlementPath);
        }

        if (options.Contains(EntitlementOptions.ApsEnv))
        {
            if (entitlements.root["aps-environment"] == null)
                entitlements.root.SetString("aps-environment", "development");
        }

        // TOOD: This can be updated to use project.AddCapability() in the future
#if ADD_APP_GROUP
        if (options.Contains(EntitlementOptions.AppGroups) && entitlements.root["com.apple.security.application-groups"] == null)
        {
            var groups = entitlements.root.CreateArray("com.apple.security.application-groups");
            groups.AddString(appGroup);
        }
#endif

        if (options.Contains(EntitlementOptions.KeychainSharing) && !string.IsNullOrEmpty(keychainGroup))
        {
            var existing = entitlements.root["keychain-access-groups"] as PlistElementArray;
            if (existing == null)
                existing = entitlements.root.CreateArray("keychain-access-groups");
            existing.AddString(keychainGroup);
        }

        entitlements.WriteToFile(entitlementPath);

        // Copy the entitlement file to the xcode project
        var entitlementFileName = Path.GetFileName(entitlementPath);
        var relativeDestination = targetName + "/" + entitlementFileName;

        // Add the pbx configs to include the entitlements files on the project
        project.AddFile(relativeDestination, entitlementFileName);
        project.AddBuildProperty(targetGUI, "CODE_SIGN_ENTITLEMENTS", relativeDestination);
    }

    private static void AddPushCapability(PBXProject project, string path, string targetGUID, string targetName)
    {
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var entitlementsPath = GetEntitlementsPath(path, project, targetGUID, targetName);

        project.AddCapability(targetGUID, PBXCapabilityType.PushNotifications, entitlementsPath, false);
        project.AddCapability(targetGUID, PBXCapabilityType.BackgroundModes, entitlementsPath, false);

        // NOTE: ProjectCapabilityManager's 4th constructor param requires Unity 2019.3+
        var projCapability = new ProjectCapabilityManager(projectPath, entitlementsPath, targetName);
        projCapability.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
        projCapability.WriteToFile();
    }

    private static void AddNotificationServiceExtension(PBXProject project, string path, string appGroup, string keychainGroup = null)
    {
#if UNITY_2017_2_OR_NEWER && !UNITY_CLOUD_BUILD
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var mainTargetGUID = GetPBXProjectTargetGUID(project);
        var extensionTargetName = NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME;
        var extensionBundleIdentifier = GetExtensionBundleIdentifier(extensionTargetName);
        var exisitingPlistFile = CreateExtensionPlistFile(path, true, appGroup);
        // If file exisits then the below has been completed before from another build
        // The below will not be updated on Append builds
        // Changes would most likely need to be made to support Append builds
        if (exisitingPlistFile)
            return;

        var extensionGUID = PBXProjectExtensions.AddAppExtension(
           project,
           mainTargetGUID,
           extensionTargetName,
           extensionBundleIdentifier,
           extensionTargetName + "/" + "Info.plist" // Unix path as it's used by Xcode
        );

        AddExtensionSourceFilesToTarget(project, extensionGUID, path, true);

        foreach (var framework in FRAMEWORKS_TO_ADD)
        {
            project.AddFrameworkToProject(extensionGUID, framework, true);
        }

        // Makes it so that the extension target is Universal (not just iPhone) and has an iOS 10 deployment target
        project.SetBuildProperty(extensionGUID, "TARGETED_DEVICE_FAMILY", "1,2");
        project.SetBuildProperty(extensionGUID, "IPHONEOS_DEPLOYMENT_TARGET", "13.0");

        project.SetBuildProperty(extensionGUID, "ARCHS", "$(ARCHS_STANDARD)");
        project.SetBuildProperty(extensionGUID, "DEVELOPMENT_TEAM", PlayerSettings.iOS.appleDeveloperTeamID);
        project.SetBuildProperty(extensionGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "Yes");
        project.SetBuildProperty(extensionGUID, "ENABLE_BITCODE", "NO");
        project.SetBuildProperty(extensionGUID, "CLANG_ENABLE_MODULES", "YES");

        project.WriteToFile(projectPath);

        //var contents = File.ReadAllText(projectPath);
        // This method only modifies the PBXProject string passed in (contents).
        // After this method finishes, we must write the contents string to disk
        //File.WriteAllText(projectPath, contents);

        var nseEntitlementOptions = new HashSet<EntitlementOptions> {
            EntitlementOptions.ApsEnv, EntitlementOptions.AppGroups
        };
        if (!string.IsNullOrEmpty(keychainGroup))
            nseEntitlementOptions.Add(EntitlementOptions.KeychainSharing);

        AddOrUpdateEntitlements(path, project, extensionGUID, extensionTargetName, nseEntitlementOptions, appGroup, keychainGroup);
#endif
    }

    // Copies NotificationService.m and .h files into the NotificationServiceExtension folder adds them to the Xcode target
    private static void AddExtensionSourceFilesToTarget(PBXProject project, string extensionGUID, string path, bool forServiceExtension)
    {
        var buildPhaseID = project.AddSourcesBuildPhase(extensionGUID);
        foreach (var type in new string[] { "m", "h" })
        {
            var nativeFileName = "";
            var sourcePath = "";
            var nativeFileRelativeDestination = "";

            if(forServiceExtension){
                nativeFileName = NOTIFICATION_SERVICE_EXTENSION_OBJECTIVEC_FILENAME + "." + type;
                sourcePath = MOE_IOS_LOCATION + DIR_CHAR + nativeFileName;
                nativeFileRelativeDestination = NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME + "/" + nativeFileName;
            }
            else{
                nativeFileName = PUSH_TEMPLATES_EXTENSION_OBJECTIVEC_FILENAME + "." + type;
                sourcePath = MOE_IOS_PUSH_TEMP_LOCATION + DIR_CHAR + nativeFileName;
                nativeFileRelativeDestination = PUSH_TEMPLATES_EXTENSION_TARGET_NAME + "/" + nativeFileName;
            }
            

            var destPath = path + DIR_CHAR + nativeFileRelativeDestination;
            if (!File.Exists(destPath))
                FileUtil.CopyFileOrDirectory(sourcePath, destPath);

            var sourceFileGUID = project.AddFile(nativeFileRelativeDestination, nativeFileRelativeDestination, PBXSourceTree.Source);
            project.AddFileToBuildSection(extensionGUID, buildPhaseID, sourceFileGUID);
        }
    }

    // Create a .plist file for the NSE
    // NOTE: File in Xcode project is replaced everytime, never appends
    private static bool CreateExtensionPlistFile(string path, bool forServiceExtension, string appGroup)
    {
#if UNITY_2017_2_OR_NEWER
        var pathToExtension = path + DIR_CHAR;
        if(forServiceExtension){
            pathToExtension = pathToExtension + NOTIFICATION_SERVICE_EXTENSION_TARGET_NAME;
        }
        else{
            pathToExtension = pathToExtension + PUSH_TEMPLATES_EXTENSION_TARGET_NAME;
        }
        Directory.CreateDirectory(pathToExtension);

        var extensionPlistPath = pathToExtension + DIR_CHAR + "Info.plist";
        bool exisiting = File.Exists(extensionPlistPath);

        // Read plist from MoEngage iOS folder.
        var plistFile = new PlistDocument();
        if(forServiceExtension){
            plistFile.ReadFromFile(MOE_IOS_LOCATION + DIR_CHAR + "Info.plist");
        }
        else{
            plistFile.ReadFromFile(MOE_IOS_PUSH_TEMP_LOCATION + DIR_CHAR + "Info.plist");
        }

        plistFile.root.SetString("CFBundleShortVersionString", PlayerSettings.bundleVersion);
        plistFile.root.SetString("CFBundleVersion", PlayerSettings.iOS.buildNumber.ToString());
        var moeDict = plistFile.root.CreateDict("MoEngage");
        moeDict.SetString("AppGroupName", appGroup);
        plistFile.WriteToFile(extensionPlistPath);
        return exisiting;
#else
         return true;
#endif
    }


        private static void AddPushTemplateExtension(PBXProject project, string path, string appGroup)
    {
#if UNITY_2017_2_OR_NEWER && !UNITY_CLOUD_BUILD
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var mainTargetGUID = GetPBXProjectTargetGUID(project);
        var extensionTargetName = PUSH_TEMPLATES_EXTENSION_TARGET_NAME;
        var extensionBundleIdentifier = GetExtensionBundleIdentifier(extensionTargetName);
        var exisitingPlistFile = CreateExtensionPlistFile(path, false, appGroup);
        // If file exisits then the below has been completed before from another build
        // The below will not be updated on Append builds
        // Changes would most likely need to be made to support Append builds
        if (exisitingPlistFile)
            return;

        var extensionGUID = PBXProjectExtensions.AddAppExtension(
           project,
           mainTargetGUID,
           extensionTargetName,
           extensionBundleIdentifier,
           extensionTargetName + "/" + "Info.plist" // Unix path as it's used by Xcode
        );

        AddExtensionSourceFilesToTarget(project, extensionGUID, path, false);
        AddExtensionStoryBoardToTarget(project, extensionGUID, path);

        foreach (var framework in FRAMEWORKS_TO_ADD)
        {
            project.AddFrameworkToProject(extensionGUID, framework, true);
        }

        // Makes it so that the extension target is Universal (not just iPhone) and has an iOS 10 deployment target
        project.SetBuildProperty(extensionGUID, "TARGETED_DEVICE_FAMILY", "1,2");
        project.SetBuildProperty(extensionGUID, "IPHONEOS_DEPLOYMENT_TARGET", "13.0");

        project.SetBuildProperty(extensionGUID, "ARCHS", "$(ARCHS_STANDARD)");
        project.SetBuildProperty(extensionGUID, "DEVELOPMENT_TEAM", PlayerSettings.iOS.appleDeveloperTeamID);
        project.SetBuildProperty(extensionGUID, "GCC_ENABLE_OBJC_EXCEPTIONS", "Yes");
        project.SetBuildProperty(extensionGUID, "ENABLE_BITCODE", "NO");
        project.SetBuildProperty(extensionGUID, "CLANG_ENABLE_MODULES", "YES");

        project.WriteToFile(projectPath);

        //var contents = File.ReadAllText(projectPath);
        // This method only modifies the PBXProject string passed in (contents).
        // After this method finishes, we must write the contents string to disk
        //File.WriteAllText(projectPath, contents);

        AddOrUpdateEntitlements(
           path,
           project,
           extensionGUID,
           extensionTargetName,
           new HashSet<EntitlementOptions> {
               EntitlementOptions.ApsEnv, EntitlementOptions.AppGroups
           },
           appGroup
        );
#endif
    }

    private static void AddExtensionStoryBoardToTarget(PBXProject project, string extensionGUID, string path)
    {
        var buildPhaseID = project.AddSourcesBuildPhase(extensionGUID);
        var storyBoardFileName = "MainInterface.storyboard";
        var sourcePath = MOE_IOS_PUSH_TEMP_LOCATION + DIR_CHAR + storyBoardFileName;
        var nativeFileRelativeDestination = PUSH_TEMPLATES_EXTENSION_TARGET_NAME + DIR_CHAR + storyBoardFileName;

        var destPath = path + DIR_CHAR + nativeFileRelativeDestination;
        if (!File.Exists(destPath))
            FileUtil.CopyFileOrDirectory(sourcePath, destPath);

        string resourcesBuildPhase = project.GetResourcesBuildPhaseByTarget(extensionGUID);
        string resourcesFilesGuid = project.AddFile(destPath, nativeFileRelativeDestination, PBXSourceTree.Source);
        project.AddFileToBuildSection(extensionGUID, resourcesBuildPhase, resourcesFilesGuid);
    }


    private static string GetAppGroupId()
    {
        string moeInfoPlistPath = Path.Combine(Application.dataPath, "MoEngage-Info.plist");
        if (File.Exists(moeInfoPlistPath))
        {
            var moePlist = new PlistDocument();
            moePlist.ReadFromFile(moeInfoPlistPath);
            var appGroupElement = moePlist.root["AppGroupName"] as PlistElementString;
            if (appGroupElement != null && !string.IsNullOrEmpty(appGroupElement.value))
                return appGroupElement.value;
        }
        return "group." + PlayerSettings.applicationIdentifier + ".moengage";
    }

    private static void MergeMoEngageInfoPlist(string buildPath)
    {
        string mainInfoPlistPath = buildPath + DIR_CHAR + "Info.plist";
        if (!File.Exists(mainInfoPlistPath))
        {
            Debug.LogWarning("MoEngage: Main Info.plist not found at " + mainInfoPlistPath);
            return;
        }

        string moeInfoPlistPath = Path.Combine(Application.dataPath, "MoEngage-Info.plist");
        if (!File.Exists(moeInfoPlistPath))
        {
            Debug.LogWarning("MoEngage: MoEngage-Info.plist not found at " + moeInfoPlistPath);
            return;
        }

        var mainPlist = new PlistDocument();
        mainPlist.ReadFromFile(mainInfoPlistPath);

        var moePlist = new PlistDocument();
        moePlist.ReadFromFile(moeInfoPlistPath);

        PlistElementDict moeDict = mainPlist.root.CreateDict("MoEngage");
        foreach (var kvp in moePlist.root.values)
        {
            moeDict.values[kvp.Key] = kvp.Value;
        }

        mainPlist.WriteToFile(mainInfoPlistPath);
        Debug.Log("MoEngage: merged MoEngage-Info.plist into Info.plist under 'MoEngage' key.");
    }
    
    public static bool IsSPMEnabled()
    {
        var settingsPath = Path.Combine("ProjectSettings", "GvhProjectSettings.xml");
        if (!File.Exists(settingsPath))
            return false;
        var doc = new XmlDocument();
        doc.Load(settingsPath);
        foreach (XmlNode node in doc.SelectNodes("//projectSetting"))
        {
            if (node.Attributes["name"]?.Value == "Google.IOSResolver.SwiftPackageManagerEnabled")
                return node.Attributes["value"]?.Value == "True";
        }
        return false;
    }

    // Returns the bundle identifier for the specified extension target
    private static string GetExtensionBundleIdentifier(string extensionTargetName)
    {
#if UNITY_2021_2_OR_NEWER
        return PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.iOS) + "." + extensionTargetName;
#else
        return PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS) + "." + extensionTargetName;
#endif
    }

}
#endif