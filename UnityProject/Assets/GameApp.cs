using System.Collections;
using System.IO;
using System.Text;
using Jerry;
using UnityEngine;
using UnityEngine.UI;

public class GameApp : SingletonMono<GameApp>
{
    private const int BTN_CNT = 3;
    private Button[] btns;
    private Text tex;
    private InputField input;

    public string m_DownloadPath;

    protected override void Awake()
    {
        base.Awake();

        btns = new Button[BTN_CNT];
        string[] btnsName = new string[BTN_CNT] 
        {
            "1",
            "2",
            "UnityTest"
        };
        for (int i = 0; i < BTN_CNT; i++)
        {
            btns[i] = this.transform.FindChild(string.Format("btn{0}", i + 1)).GetComponent<Button>();
            Text txt = btns[i].transform.FindChild("Text").GetComponent<Text>();
            if (txt)
            {
                txt.text = btnsName[i];
            }
        }
        tex = this.transform.FindChild("log/Viewport/Text").GetComponent<Text>();
        input = this.transform.FindChild("InputField").GetComponent<InputField>();
    }

    void Start()
    {
        LoadData();

        btns[0].onClick.AddListener(() =>
        {
            //SDKMgr.Inst.Login();
            
            //Download
            //int status = SDKMgr.Inst.JerrySDKMgr_DownloadApk(new JerrySDK.DownloadPar()
            //{
            //    url = m_DownloadPath,
            //    apkName = "jerryTest",
            //    noticeShowName = "Jerry",
            //});
            //if (status == 0)
            //{
            //    this.StartCoroutine(IE_RefreshDownloadPro());
            //}
        });

        btns[1].onClick.AddListener(() =>
        {
            SDKMgr.Inst.JerrySDKMgr_GetDeviceId();
            AddLog("xxx idfa:" + SDKMgr.Inst.IDFA);

            UnityEngine.Debug.LogError("just test");
        });

        btns[2].onClick.AddListener(() =>
        {
            if (btns[2].gameObject.GetComponent<TestOne>() == null)
            {
                btns[2].gameObject.AddComponent<TestOne>();
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
                //loginText.text = "0%";
            }
            else
            {
                //loginText.text = pro.GetPro();
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

    private string dataSaveTag = "";

    /// <summary>
    /// 加载本地存储数据
    /// </summary>
    private void LoadData()
    {
        input.text = PlayerPrefs.GetString("testSave", "");
        dataSaveTag = "";
    }
    
    /// <summary>
    /// 保存本地数据
    /// </summary>
    /// <param name="tag"></param>
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
}