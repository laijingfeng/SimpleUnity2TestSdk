using Jerry;

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private JerrySDK m_JerrySDK = null;

    private void JerrySDKMgr_Create()
    {
        m_JerrySDK = new JerrySDK(this.gameObject.name);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="par">-1参数异常；-2禁用了下载；0正常；1下载完成，进入安装</param>
    /// <returns></returns>
    public int JerrySDKMgr_DownloadApk(JerrySDK.DownloadPar par)
    {
        if (m_JerrySDK != null)
        {
            return m_JerrySDK.DownloadApk(par);
        }
        return -2;
    }

    public JerrySDK.DownLoadPro JerrySDKMgr_GetDownloadPro()
    {
        if (m_JerrySDK != null)
        {
            return m_JerrySDK.GetDownLoadPro();
        }
        return null;
    }

    public void JerrySDKMgr_DoTest()
    {
        if (m_JerrySDK != null)
        {
            m_JerrySDK.DoTest();
        }
    }

    public void JerrySDKMgr_CleanCache()
    {
        if (m_JerrySDK != null)
        {
            m_JerrySDK.CleanCache();
        }
    }
}