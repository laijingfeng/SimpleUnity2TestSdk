using Jerry;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
            //Application.OpenURL(m_DownloadPath);
            //SDKMgr.Inst.Login();
            SDKMgr.Inst.DownloadApk(m_DownloadPath);
            this.StartCoroutine(IE_RefreshPro());
        });

        btnDoSwitchAccount.onClick.AddListener(() =>
        {
            SDKMgr.Inst.SwitchAccout();
        });

        btnTest.onClick.AddListener(() =>
        {
            if (btnTest.gameObject.GetComponent<TestOne>() == null)
            {
                btnTest.gameObject.AddComponent<TestOne>();
            }
        });
    }

    public class DownLoadPro
    {
        public int loadedSize = 0;
        public int TotalSize = 0;

        public string GetPro()
        {
            if (TotalSize == 0)
            {
                return "0%";
            }
            return ((loadedSize * 100f) / TotalSize) + "%";
        }
    }

    private IEnumerator IE_RefreshPro()
    {
        while (true)
        {
            string pro = SDKMgr.Inst.GetDownloadPro();
            if (string.IsNullOrEmpty(pro))
            {
                loginText.text = "0%";
            }
            else
            {
                DownLoadPro p = JsonUtility.FromJson<DownLoadPro>(pro);
                loginText.text = p.GetPro();
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