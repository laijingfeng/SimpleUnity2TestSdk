using System;
using UnityEngine;

public class JerrySDK
{
    private JerrySDKImpl sdk = null;

    public JerrySDK(string mgr)
    {
#if UNITY_ANDROID
        sdk = new JerrySDKAndroidImpl(mgr);
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="par">-1参数异常；-2禁用了下载；0正常；1下载完成，进入安装</param>
    /// <returns></returns>
    public int DownloadApk(DownloadPar par)
    {
        if (sdk != null)
        {
            return sdk.DownloadApk(JsonUtility.ToJson(par));
        }
        return -2;
    }

    public DownLoadPro GetDownLoadPro()
    {
        if (sdk != null)
        {
            string str = sdk.GetDownloadPro();
            if (!string.IsNullOrEmpty(str))
            {
                return JsonUtility.FromJson<DownLoadPro>(str);
            }
        }
        return null;
    }

    public class DownloadPar
    {
        public string url;
        public string noticeShowName;
        public string apkName;
    }

    public class DownLoadPro
    {
        public int loadedSize;
        public int totalSize;
        public bool finish;

        public string GetPro()
        {
            if (finish)
            {
                return "100%";
            }
            if (totalSize == 0)
            {
                return "0%";
            }
            return ((loadedSize * 100f) / totalSize).ToString("F0") + "%";
        }
    }
}