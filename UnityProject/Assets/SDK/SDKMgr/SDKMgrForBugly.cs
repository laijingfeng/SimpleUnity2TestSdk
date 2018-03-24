using Jerry;

public partial class SDKMgr : SingletonMono<SDKMgr>
{
    private void Bugly_Init()
    {
        // 开启SDK的日志打印，发布版本请务必关闭
        BuglyAgent.ConfigDebugMode(true);

#if UNITY_IPHONE || UNITY_IOS
        BuglyAgent.InitWithAppId("8ec9139b10");
#elif UNITY_ANDROID
        BuglyAgent.InitWithAppId("085dd1eab0");
#endif

        //如果你确认已在对应的iOS工程或Android工程中初始化SDK，那么在脚本中只需启动C#异常捕获上报功能即可
        BuglyAgent.EnableExceptionHandler();
    }
}