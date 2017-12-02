using Jerry;
using UnityEngine;

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// 登录回包数据
    /// </summary>
    public class LoginCallbackData
    {
        public string uid;
        public string token;
    }

    private void Init()
    {
        JerrySDKMgr_Create();
        //Bugly_Init();

#if UNITY_EDITOR
        UnityEngine.Debug.LogWarning("Init Editor");
#elif UNITY_ANDROID
        SDKHelper.Inst.UnityCallAnroid("DoHideAndroidSplash", false);
#else
        UnityEngine.Debug.LogWarning("Init TODO");
#endif
    }

    #region Unity2SDK

    /// <summary>
    /// 登录
    /// </summary>
    public void Login()
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogWarning("Login Editor");
#elif UNITY_ANDROID
        SDKHelper.Inst.UnityCallAnroid("DoLogin", false);
#else
        UnityEngine.Debug.LogWarning("Login TODO");
#endif
    }

    public void SwitchAccout()
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogWarning("SwitchAccout Editor");
#elif UNITY_ANDROID
        SDKHelper.Inst.UnityCallAnroid("DoSwitchAccount", false);
#else
        UnityEngine.Debug.LogWarning("SwitchAccout TODO");
#endif
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        Application.Quit();
#endif
    }

    #endregion Unity2SDK

    #region AndroidSDK2Unity

    /// <summary>
    /// 登录回调
    /// </summary>
    /// <param name="loginData"></param>
    private void DoLoginCallback(string loginData)
    {
        LoginCallbackData data = JsonUtility.FromJson<LoginCallbackData>(loginData);
        GameApp.Inst.LoginCallback(data);
    }

    /// <summary>
    /// 切换帐号回调
    /// </summary>
    /// <param name="info"></param>
    private void DoSwitchAccountCallback(string info)
    {
        GameApp.Inst.SwitchAccountCallback(info);
    }

    /// <summary>
    /// <para>SDK的退出通知</para>
    /// <para>Unity的OnDestroy等有时并不能准确监听到SDK的退出，给一个回调，用来做存档之类的</para>
    /// </summary>
    private void DoExitFromSDK()
    {
        GameApp.Inst.DoWhileExitGame("sdk");
    }

    #endregion AndroidSDK2Unity
}