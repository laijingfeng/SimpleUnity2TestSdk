using Jerry;

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private JerrySDK m_JerrySDK = null;

    private void JerrySDKMgr_Init()
    {
        m_JerrySDK = new JerrySDK(this.gameObject.name);
    }

    public void JerrySDKMgr_DownloadApk(JerrySDK.DownloadPar par)
    {
        if (m_JerrySDK != null)
        {
            m_JerrySDK.DownloadApk(par);
        }
    }

    public JerrySDK.DownLoadPro JerrySDKMgr_GetDownloadPro()
    {
        if (m_JerrySDK != null)
        {
            return m_JerrySDK.GetDownLoadPro();
        }
        return null;
    }
}