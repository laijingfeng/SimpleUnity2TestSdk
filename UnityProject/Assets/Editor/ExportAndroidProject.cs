using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportAndroidProject : Editor
{
    [MenuItem("Tools/导出Android工程", false, 1)]
    static public void ExportAndroid()
    {
        PlayerSettings.productName = "UnityProject";
        PlayerSettings.keystorePass = "jerrylai@jingfeng*1990";
        PlayerSettings.keyaliasPass = "lai123";

        string exportPath = Application.dataPath + "/../ExportAndroid";
        if (Directory.Exists(exportPath))
        {
            Directory.Delete(exportPath, true);
        }
        Directory.CreateDirectory(exportPath);

        BuildPipeline.BuildPlayer(new string[] 
        {
            "Assets/Main.unity",
        },
        exportPath,
        BuildTarget.Android,
        BuildOptions.AcceptExternalModificationsToPlayer);
    }
}