#if UNITY_5_4_OR_NEWER && UNITY_IPHONE && UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class GeofenceBuildPostProcessor
{
    public static readonly string DEFAULT_PROJECT_TARGET_NAME = "Unity-iPhone";

    private static readonly string[] GEOFENCE_FRAMEWORKS_TO_ADD = {
         "CoreLocation.framework"
      };

#if UNITY_2019_3_OR_NEWER
    private static string GetPBXProjectUnityFrameworkGUID(PBXProject project)
    {
        return project.GetUnityFrameworkTargetGuid();
    }
#else

     private static string GetPBXProjectTargetGUID(PBXProject project)
    {
        return project.TargetGuidByName(PBXProject.GetUnityTargetName());
    }
    
    private static string GetPBXProjectUnityFrameworkGUID(PBXProject project)
    {
        return GetPBXProjectTargetGUID(project);
    }
#endif

    [PostProcessBuildAttribute(2)]
    public static void OnPostProcessBuild_Geofence(BuildTarget target, string path)
    {
        var projectPath = PBXProject.GetPBXProjectPath(path);
        var project = new PBXProject();

        project.ReadFromString(File.ReadAllText(projectPath));
        var unityFrameworkGUID = GetPBXProjectUnityFrameworkGUID(project);

        foreach (var framework in GEOFENCE_FRAMEWORKS_TO_ADD)
        {
            project.AddFrameworkToProject(unityFrameworkGUID, framework, false);
        }
        File.WriteAllText(projectPath, project.WriteToString());

    }

}
#endif