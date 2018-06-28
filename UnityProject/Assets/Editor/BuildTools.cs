﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildTools : Editor
{
#if UNITY_ANDROID
    [MenuItem("Tools/导出APK", false, 0)]
    static public void ExportAndroidApk()
    {
        DoSettings();
        string exportPath = string.Format("{0}/{1}_{2}.apk",
            Application.dataPath.Replace("/Assets", ""), PlayerSettings.productName, System.DateTime.Now.ToString("HHmmss"));
        DoBuild(exportPath, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("Tools/导出Android工程", false, 1)]
    static public void ExportAndroidProject()
    {
        DoSettings();
        string exportPath = Application.dataPath.Replace("/Assets", "") + "/ExportAndroid";
        if (Directory.Exists(exportPath))
        {
            Directory.Delete(exportPath, true);
        }
        Directory.CreateDirectory(exportPath);
        DoBuild(exportPath, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    [MenuItem("Tools/仅仅做Android设置", false, 2)]
    static public void JustDoSettings()
    {
        DoSettings();
    }
#endif

#if UNITY_WEBGL
    [MenuItem("Tools/导出WebGL", false, 3)]
    static public void ExportWebGL()
    {
        string exportPath = Application.dataPath.Replace("/Assets", "") + "/WebGL";
        if (Directory.Exists(exportPath))
        {
            Directory.Delete(exportPath, true);
        }
        Directory.CreateDirectory(exportPath);
        DoBuild(exportPath, BuildTarget.WebGL, BuildOptions.Il2CPP);
    }
#endif

#if UNITY_IOS
    [MenuItem("Tools/导出iOS", false, 4)]
    static public void ExportWebGL()
    {
        string exportPath = Application.dataPath.Replace("/Assets", "") + "/iOS" + System.DateTime.Now.ToString("yyyMMdd_HHmmss");
        if (Directory.Exists(exportPath))
        {
            Directory.Delete(exportPath, true);
        }
        Directory.CreateDirectory(exportPath);
        DoBuild(exportPath, BuildTarget.iOS, BuildOptions.Il2CPP);
    }
#endif

    private static void DoSettings()
    {
        //PlayerSettings.productName = "UnityProject";
        PlayerSettings.companyName = "Jerry";
        PlayerSettings.applicationIdentifier = string.Format("com.jerry.lai.{0}", PlayerSettings.productName);
        PlayerSettings.Android.keystoreName = "./jerry.keystore";
        PlayerSettings.Android.keystorePass = "jerrylai@jingfeng*1990";
        PlayerSettings.Android.keyaliasName = "jerrylai";
        PlayerSettings.Android.keyaliasPass = "lai123";
    }

    private static string[] GetLevels()
    {
        if (EditorBuildSettings.scenes == null || EditorBuildSettings.scenes.Length <= 0)
        {
            return null;
        }
        List<string> ret = new List<string>();
        foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
        {
            if (s.enabled == true)
            {
                ret.Add(s.path);
            }
        }
        return ret.ToArray();
    }

    private static void DoBuild(string path, BuildTarget tar, BuildOptions opt)
    {
        string[] levels = GetLevels();
        if (levels == null || levels.Length <= 0)
        {
            Debug.LogWarning("打包的场景列表为空，请在BuildSettings的ScenesInBuild设置要打包的场景");
            return;
        }
        BuildPipeline.BuildPlayer(levels,
        path,
        tar,
        opt);
        Debug.Log(string.Format("build sucess to {0} at {1}", path, System.DateTime.Now));
    }
}