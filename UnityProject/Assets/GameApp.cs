using System.Collections;
using Jerry;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class GameApp : SingletonMono<GameApp>
{
    private Button btnDoLogin;
    private Button btnDoSwitchAccount;
    private Button btnTest;
    private Text tex;
    private InputField input;

    public string m_DownloadPath;
    private Text loginText;

    protected override void Awake()
    {
        base.Awake();

        btnDoLogin = this.transform.FindChild("doLogin").GetComponent<Button>();
        loginText = btnDoLogin.transform.FindChild("Text").GetComponent<Text>();
        btnDoSwitchAccount = this.transform.FindChild("doSwitchAccount").GetComponent<Button>();
        btnTest = this.transform.FindChild("testBtn").GetComponent<Button>();
        tex = this.transform.FindChild("log/Viewport/Text").GetComponent<Text>();
        input = this.transform.FindChild("InputField").GetComponent<InputField>();
    }

    void Start()
    {
        LoadData();

        btnDoLogin.onClick.AddListener(() =>
        {
            //SDKMgr.Inst.Login();
            int status = SDKMgr.Inst.JerrySDKMgr_DownloadApk(new JerrySDK.DownloadPar()
            {
                url = m_DownloadPath,
                apkName = "jerryTest",
                noticeShowName = "Jerry",
            });
            if (status == 0)
            {
                this.StartCoroutine(IE_RefreshDownloadPro());
            }
        });

        btnDoSwitchAccount.onClick.AddListener(() =>
        {
            AddLog("xxx");
            //c_ctest();
            //SDKMgr.Inst.SwitchAccout();
            //DoTest();
            //SDKMgr.Inst.JerrySDKMgr_DoTest();
        });

        btnTest.onClick.AddListener(() =>
        {
            if (btnTest.gameObject.GetComponent<TestOne>() == null)
            {
                btnTest.gameObject.AddComponent<TestOne>();
            }
        });
    }

    private void DoTest()
    {
        string pdPath = PlatformUtil.GetPersistentDataPath() + "/ResVer.txt";
        Debug.LogWarning("Exists:" + File.Exists(pdPath));
        if (!File.Exists(pdPath))
        {
            File.WriteAllText(pdPath, "txt", Encoding.UTF8);
        }
    }

    private IEnumerator IE_RefreshDownloadPro()
    {
        while (true)
        {
            JerrySDK.DownLoadPro pro = SDKMgr.Inst.JerrySDKMgr_GetDownloadPro();
            if (pro == null)
            {
                loginText.text = "0%";
            }
            else
            {
                loginText.text = pro.GetPro();
                if (pro.finish)
                {
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SDKMgr.Inst.ExitGame();
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            //切后台时可能马上要退出
            DoWhileExitGame("pause");
        }
    }

    void OnApplicationQuit()
    {
        DoWhileExitGame("quit");
    }

    private void LoadData()
    {
        input.text = PlayerPrefs.GetString("testSave", "");
        dataSaveTag = "";
    }

    private string dataSaveTag = "";
    private void SaveData(string tag = "")
    {
        dataSaveTag += tag;
        PlayerPrefs.SetString("testSave", input.text + dataSaveTag);
    }

    /// <summary>
    /// 增加日志
    /// </summary>
    /// <param name="msg"></param>
    public void AddLog(string msg)
    {
        if (tex == null
            || string.IsNullOrEmpty(msg))
        {
            return;
        }
        tex.text += "\n" + msg;
    }

    public void DoWhileExitGame(string tag = "")
    {
        AddLog("DoExit");
        SaveData(tag);
    }

    public void LoginCallback(SDKMgr.LoginCallbackData data)
    {
        AddLog("loginData uid:[" + data.uid + "] token:[" + data.token + "]");
    }

    public void SwitchAccountCallback(string info)
    {
        AddLog("DoSwitchAccountCallback:" + info);
    }

#if UNITY_IOS
    //[DllImport("__Internal")]
    //private static extern void c_ctest();
#endif
}