using Jerry;

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private JerrySDK m_JerrySDK = null;

    private void JerrySDKMgr_Init()
    {
        m_JerrySDK = new JerrySDK(this.gameObject.name);
    }
}