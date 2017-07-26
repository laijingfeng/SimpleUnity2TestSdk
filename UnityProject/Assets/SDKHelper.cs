#if UNITY_ANDROID && !UNITY_EDITOR
using System;
#endif
using Jerry;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
#endif

public class SDKHelper : Singleton<SDKHelper>
{
    #region AndroidSDK

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaClass jc;
    private AndroidJavaObject jo;

    private bool CheckJCJO()
    {
        if (jo != null)
        {
            return true;
        }
        if (jc != null)
        {
            try
            {
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning(e.ToString());
                return false;
            }
            return true;
        }
        try
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning(e.ToString());
            return false;
        }
        return true;
    }

    public void UnityCallAnroid(string methodName, bool isStatic = false, params object[] args)
    {
        if (!CheckJCJO())
        {
            return;
        }
        try
        {
            if (isStatic)
            {
                jo.CallStatic(methodName, args);
            }
            else
            {
                jo.Call(methodName, args);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning(e.ToString());
        }
    }

    public T UnityCallAnroid<T>(string methodName, bool isStatic = false, params object[] args)
    {
        T ret = default(T);
        if (!CheckJCJO())
        {
            return ret;
        }
        try
        {
            if (isStatic)
            {
                ret = jo.CallStatic<T>(methodName, args);
            }
            else
            {
                ret = jo.Call<T>(methodName, args);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning(e.ToString());
            ret = default(T);
        }
        return ret;
    }
#endif

    #endregion AndroidSDK
}