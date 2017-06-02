using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button btnDoLogin;
    public Button btnDoSwitchAccount;
    public Button btnTest;
    public Text tex;

    void Awake()
    {
        DoHideAndroidSplash();
    }

    void Start()
    {
        btnDoLogin.onClick.AddListener(() =>
        {
            DoLogin();
        });

        btnDoSwitchAccount.onClick.AddListener(() =>
        {
            DoSwitchAccount();
        });

        btnTest.onClick.AddListener(() =>
        {
            btnTest.gameObject.AddComponent<TestOne>();
        });
    }

    private void AddLog(string msg)
    {
        if (tex == null
            || string.IsNullOrEmpty(msg))
        {
            return;
        }
        tex.text += "\n" + msg;
    }

    #region AndroidSDK

    #region Unity2SDK

    private void DoHideAndroidSplash()
    {
        UnityCallAnroid("DoHideAndroidSplash", false);
    }

    private void DoLogin()
    {
        UnityCallAnroid("DoLogin", false);
    }

    private void DoSwitchAccount()
    {
        UnityCallAnroid("DoSwitchAccount", false);
    }

    #endregion Unity2SDK

    #region SDK2Unity

    private void DoLoginCallback(string info)
    {
        AddLog("DoLoginCallback:" + info);
    }

    private void DoSwitchAccountCallback(string info)
    {
        AddLog("DoSwitchAccountCallback:" + info);
    }

    private void DoExit()
    {
        AddLog("DoExit");
    }

    #endregion SDK2Unity

    #endregion AndroidSDK

    #region AndroidHelper

    private void UnityCallAnroid(string methodName, bool isStatic = false, params object[] args)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
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
        }
#endif
    }

    private T UnityCallAnroid<T>(string methodName, bool isStatic = false, params object[] args)
    {
        T ret = default(T);

#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
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
        }
#endif
        return ret;
    }

    #endregion AndroidHelper
}