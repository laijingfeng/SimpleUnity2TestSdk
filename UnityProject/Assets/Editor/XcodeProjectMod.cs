using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode.Custom;
#endif
using System.IO;

public class XcodeProjectMod : MonoBehaviour
{
    //该属性是在build完成后，被调用的callback
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        //BuildTarget需为iOS
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        //初始化
        string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        //添加flag，bugly需要
        pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
        //关闭Bitcode
        pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");

        // 添加framwrok，bugly需要的
        pbxProject.AddFrameworkToProject(targetGuid, "Security.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "JavaScriptCore.framework", true);

        //添加lib
        AddLibToProject(pbxProject, targetGuid, "libc++.tbd");
        AddLibToProject(pbxProject, targetGuid, "libz.tbd");

        // 应用修改
        File.WriteAllText(projectPath, pbxProject.WriteToString());
    }

    /// <summary>
    /// 添加lib方法
    /// </summary>
    /// <param name="inst"></param>
    /// <param name="targetGuid"></param>
    /// <param name="lib"></param>
    static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
    {
        string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }
}