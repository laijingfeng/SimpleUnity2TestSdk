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

    public class LoginCallbackData
    {
        public string uid;
        public string token;
    }

    #region AndroidSDK

    #region Unity2SDK

    private void DoHideAndroidSplash()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        UnityCallAnroid("DoHideAndroidSplash", false);
#endif
    }

    private void DoLogin()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        UnityCallAnroid("DoLogin", false);
#endif
    }

    private void DoSwitchAccount()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        UnityCallAnroid("DoSwitchAccount", false);
#endif
    }

    #endregion Unity2SDK

    #region SDK2Unity

    private void DoLoginCallback(string loginData)
    {
        LoginCallbackData data = JsonUtility.FromJson<LoginCallbackData>(loginData);
        AddLog("DoLoginCallback:" + loginData);
        AddLog("loginData:" + data.uid + " " + data.token);
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

    private void UnityCallAnroid(string methodName, bool isStatic = false, params object[] args)
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

    private T UnityCallAnroid<T>(string methodName, bool isStatic = false, params object[] args)
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

    #endregion AndroidHelper
}