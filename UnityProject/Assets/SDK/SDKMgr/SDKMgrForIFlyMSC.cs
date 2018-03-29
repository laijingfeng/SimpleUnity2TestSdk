using Jerry;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private void IFlyMSCSDKMgr_Create()
    {
        SDKMgr.Inst.IFlyMSCSDKMgr_RegisterVoice();
    }

#if UNITY_IOS && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void __registerVoice();

    [DllImport("__Internal")]
    private static extern void __startVoice();

    [DllImport("__Internal")]
    private static extern void __stopVoice();

    [DllImport("__Internal")]
    private static extern void __cancelVoice();
#endif

    /// <summary>
    /// 注册，实例化
    /// </summary>
    public void IFlyMSCSDKMgr_RegisterVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR
        __registerVoice();
#endif
    }

    /// <summary>
    /// 开始
    /// </summary>
    public void IFlyMSCSDKMgr_StartVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR
        __startVoice();
#endif
    }

    /// <summary>
    /// 主动结束
    /// </summary>
    public void IFlyMSCSDKMgr_StopVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR
        __stopVoice();
#endif
    }

    /// <summary>
    /// 主动取消
    /// </summary>
    public void IFlyMSCSDKMgr_CancelVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR
        __cancelVoice();
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR
    private void SDK2Unity_IFlyMSCCallback(string msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }
#endif
}