using UnityEngine;
using System.Collections;
using System;

public class JerrySDKAndroidImpl : JerrySDKImpl
{
#if UNITY_ANDROID

    private AndroidJavaObject m_SDK = null;

    public JerrySDKAndroidImpl(string mgr)
    {
        try
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    m_SDK = new AndroidJavaObject("com.jerry.lai.lib.UnityPluginInterface", mgr, jo);
                }
            }
        }
        catch (Exception e)
        {
            Log(e.Message);
        }
    }

    public override string GetDeviceId()
    {
        if (m_SDK != null)
        {
            return m_SDK.Call<string>("getDeviceId");
        }
        return base.GetDeviceId();
    }

    public void Log(string str)
    {
        UnityEngine.Debug.LogWarning(str);
    }
#endif
}