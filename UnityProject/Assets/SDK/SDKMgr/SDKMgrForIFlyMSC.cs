using Jerry;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private void IFlyMSCSDKMgr_Create()
    {
    }

#if UNITY_IOS && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _StartVoiceRecognition();
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _EndVoiceRecognition();
#endif

    /// <summary>
    /// 开始
    /// </summary>
    public void IFlyMSCSDKMgr_StartVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR
        _StartVoiceRecognition();
#endif
    }

    /// <summary>
    /// 主动结束
    /// </summary>
    public void IFlyMSCSDKMgr_StopVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR
        _EndVoiceRecognition();
#endif
    }

    /// <summary>
    /// 主动取消
    /// </summary>
    public void IFlyMSCSDKMgr_CancelVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR
        _EndVoiceRecognition();
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR
    private void OnVoiceResult(string hh)
    {
        UnityEngine.Debug.LogWarning(hh);
        GameApp.Inst.AddLog(hh);
    }

    private void SDK2Unity_IFlyMSCCallback(string msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }
#endif
}