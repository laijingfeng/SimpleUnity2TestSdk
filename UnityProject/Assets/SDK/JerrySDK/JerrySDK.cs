
public class JerrySDK
{
    private JerrySDKImpl sdk = null;

    public JerrySDK(string mgr)
    {
#if UNITY_ANDROID
        sdk = new JerrySDKAndroidImpl(mgr);
#endif
        if (sdk != null)
        {
            UnityEngine.Debug.LogWarning(sdk.GetDeviceId());
        }
    }
}