using Jerry;
#if UNITY_IOS && !UNITY_EDITOR && USE_MSC
using System.Runtime.InteropServices;
#endif

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private void IFlyMSCSDKMgr_Create()
    {
        SDKMgr.Inst.IFlyMSCSDKMgr_StartUp();
    }

#if UNITY_IOS && !UNITY_EDITOR && USE_MSC

    [DllImport("__Internal")]
    private static extern void __startup();

    [DllImport("__Internal")]
    private static extern void __startVoice();

    [DllImport("__Internal")]
    private static extern void __closeVoice();

#endif

    public void IFlyMSCSDKMgr_StartUp()
    {
#if UNITY_IOS && !UNITY_EDITOR && USE_MSC
        __startup();
#endif
    }

    public void IFlyMSCSDKMgr_StartVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR && USE_MSC
        __startVoice();
#endif
    }

    public void IFlyMSCSDKMgr_StopVoice()
    {
#if UNITY_IOS && !UNITY_EDITOR && USE_MSC
        __closeVoice();
#endif
    }

#if UNITY_IOS && !UNITY_EDITOR && USE_MSC
    private void SDK2Unity_IFlyMSCCallback(string msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }
#endif
}