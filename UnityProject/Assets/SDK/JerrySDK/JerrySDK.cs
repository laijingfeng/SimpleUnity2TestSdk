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

    public void DownloadApk(DownloadPar par)
    {
        if (sdk != null)
        {
            sdk.DownloadApk(JsonUtility.ToJson(par));
        }
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

    [Serializable]
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