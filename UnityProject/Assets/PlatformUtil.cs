using System.IO;
using UnityEngine;

namespace Jerry
{
    public class PlatformUtil
    {
        /// <summary>
        /// 当前平台，决定AB的Manifest文件名
        /// </summary>
        public static Platform CurPlatform =
#if UNITY_ANDROID
            Platform.Android;
#elif UNITY_IOS
            Platform.IOS;
#elif UNITY_WEBGL
 Platform.WebGL;
#else
            Platform.UnKnow;
#endif

        /// <summary>
        /// PersistentDataPath
        /// </summary>
        /// <returns></returns>
        static public string GetPersistentDataPath()
        {
            string filepath = Application.persistentDataPath;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            return filepath;
        }

        /// <summary>
        /// StreamingAssetsPath
        /// </summary>
        /// <returns></returns>
        static public string GetStreamingAssetsPath()
        {
#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            return Application.streamingAssetsPath;
#else
            return "file://" + Application.streamingAssetsPath;
#endif
        }

        /// <summary>
        /// 平台，决定AB的Manifest文件名
        /// </summary>
        public enum Platform
        {
            UnKnow = 0,
            Android,
            IOS,
            WebGL,
        }

        public const string AssetBundlesOutputPath = "AssetBundles";
    }
}