using Jerry;
using UnityEngine;
using UnityEngine.UI;

public class GameApp : SingletonMono<GameApp>
{
    private Button btnDoLogin;
    private Button btnDoSwitchAccount;
    private Button btnTest;
    private Text tex;
    private InputField input;

    protected override void Awake()
    {
        base.Awake();

        btnDoLogin = this.transform.FindChild("doLogin").GetComponent<Button>();
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
            SDKMgr.Inst.Login();
            int[] aa = new int[2] { 1, 2 };
            for (int i = 1; i < 3; i++)
            {
                aa[i] = aa[i - 1] + 1;
            }
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