using Jerry;
using System.Runtime.InteropServices;

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private void IFlyMSCSDKMgr_Create()
    {
        SDKMgr.Inst.IFlyMSCSDKMgr_StartUp();
    }

    [DllImport("__Internal")]
    private static extern void __startup();

    [DllImport("__Internal")]
    private static extern void __startVoice();

    [DllImport("__Internal")]
    private static extern void __closeVoice();

    public void IFlyMSCSDKMgr_StartUp()
    {
        __startup();
    }

    public void IFlyMSCSDKMgr_StartVoice()
    {
        __startVoice();
    }

    public void IFlyMSCSDKMgr_StopVoice()
    {
        __closeVoice();
    }

#if UNITY_IOS
    private void SDK2Unity_IFlyMSCCallback(string msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }
#endif
}