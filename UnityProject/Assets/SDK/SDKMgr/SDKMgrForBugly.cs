using Jerry;

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private void Bugly_Init()
    {
        BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.Log);
#if UNITY_ANDROID
        BuglyAgent.InitWithAppId("085dd1eab0");
#endif
        BuglyAgent.EnableExceptionHandler();
    }
}