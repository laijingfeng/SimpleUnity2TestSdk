#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class JerrySDKiOSImpl : JerrySDKImpl
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void __getIDFA();

    /// <summary>
    /// 获取设备ID
    /// </summary>
    /// <returns></returns>
    public override string GetDeviceId()
    {
        __getIDFA();
        return base.GetDeviceId();
    }
#endif
}