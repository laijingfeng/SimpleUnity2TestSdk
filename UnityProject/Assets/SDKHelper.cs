using Jerry;
using UnityEngine;

public class SDKHelper : Singleton<SDKHelper>
{
    #region AndroidSDK

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaClass jc;
    private AndroidJavaObject jo;

    private void CheckJCJO()
    {
        if (jo != null)
        {
            return;
        }
        if (jc != null)
        {
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            return;
        }
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }

    public void UnityCallAnroid(string methodName, bool isStatic = false, params object[] args)
    {
        CheckJCJO();
        if (isStatic)
        {
            jo.CallStatic(methodName, args);
        }
        else
        {
            jo.Call(methodName, args);
        }
    }

    public T UnityCallAnroid<T>(string methodName, bool isStatic = false, params object[] args)
    {
        CheckJCJO();
        T ret = default(T);
        if (isStatic)
        {
            ret = jo.CallStatic<T>(methodName, args);
        }
        else
        {
            ret = jo.Call<T>(methodName, args);
        }
        return ret;
    }
#endif

    #endregion AndroidSDK
}