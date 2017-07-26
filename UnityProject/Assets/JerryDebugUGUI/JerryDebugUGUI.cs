using System;
using System.Collections.Generic;
using Jerry;
using UnityEngine;

public class JerryDebugUGUI : JerryDebug<JerryDebugUGUI>
{
    #region 配置

    /// <summary>
    /// <para>通过Unity的MessageReceived来转换日志</para>
    /// <para>这样的好处是有堆栈</para>
    /// <para>可能不好的是会监听到干扰Log</para>
    /// </summary>
    public bool m_UseMessageReceived = false;

    public TraceLev m_TraceLev = TraceLev.None;

    /// <summary>
    /// 是否编辑器，输出文件路径编辑器和非编辑不一样
    /// </summary>
    public bool m_IsNowEditor = false;

    public enum TraceLev
    {
        None = 0,
        Simple,
        Full,
    }

    /// <summary>
    /// 当前配置
    /// </summary>
    private List<CtrConfig> m_Ctrs = new List<CtrConfig>();

    public List<CtrConfig> Ctrs
    {
        get
        {
            return m_Ctrs;
        }
    }

    private bool m_ToBottom = false;
    public bool ToBottom
    {
        get
        {
            return m_ToBottom;
        }
    }

    #endregion 配置

    private Transform m_LogPanel;
    private Transform m_DetailPanel;
    private Transform m_CtrPanel;
    private Transform m_Log;

    public override void Awake()
    {
        base.Awake();

        AddSysCtr();

        m_Log = this.transform.FindChild("Log");

        m_LogPanel = this.transform.FindChild("Log/LogPanel");
        m_LogPanel.gameObject.AddComponent<LogPanel>().Init();

        m_DetailPanel = this.transform.FindChild("Log/DetailPanel");
        m_DetailPanel.gameObject.AddComponent<DetailPanel>().Init();

        m_CtrPanel = this.transform.FindChild("Log/CtrPanel");
        m_CtrPanel.gameObject.AddComponent<CtrPanel>().Init();

        //Application.logMessageReceived += HandleLog;
        Application.logMessageReceivedThreaded += HandleLog;

        JerryEventMgr.AddEvent(Enum_Event.ShowOrHideLog.ToString(), ShowOrHideLog);
    }

    private void HandleLog(string logString, string stackTrace, UnityEngine.LogType type)
    {
        if (!m_UseMessageReceived)
        {
            return;
        }
        AddLog(logString + "\n" + stackTrace, JerryDebugUGUI.LogTypeUnity2Custom(type), Enum_LogTag.All, false, false);
    }

    void OnDestroy()
    {
        JerryEventMgr.RemoveEvent(Enum_Event.ShowOrHideLog.ToString(), ShowOrHideLog);
    }

    private void ShowOrHideLog(object[] args)
    {
        m_Log.gameObject.SetActive(!m_Log.gameObject.activeSelf);
    }

    #region 对外接口

    /// <summary>
    /// LOG
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="tag"></param>
    /// <param name="isProtoMsg"></param>
    public void LogInfo(object msg, Enum_LogTag tag = Enum_LogTag.All, bool isProtoMsg = false)
    {
        AddLog(msg, Enum_LogType.Inf, tag, isProtoMsg);
    }

    /// <summary>
    /// LOG
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="tag"></param>
    /// <param name="isProtoMsg"></param>
    public void LogWarn(object msg, Enum_LogTag tag = Enum_LogTag.All, bool isProtoMsg = false)
    {
        AddLog(msg, Enum_LogType.War, tag, isProtoMsg);
    }

    /// <summary>
    /// LOG
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="tag"></param>
    /// <param name="isProtoMsg"></param>
    public void LogError(object msg, Enum_LogTag tag = Enum_LogTag.All, bool isProtoMsg = false)
    {
        AddLog(msg, Enum_LogType.Err, tag, isProtoMsg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="outConsole"></param>
    /// <param name="outScreen"></param>
    /// <param name="outFile"></param>
    /// <param name="isNowEditor"></param>
    public void Set(bool outConsole = true, bool outScreen = false, bool outFile = false, bool isNowEditor = false)
    {
        m_OutConsole = outConsole;
        m_OutScreen = outScreen;
        m_OutFile = outFile;
        if (m_OutFile)
        {
            if (m_IsNowEditor)
            {
                LOG_FILE_PATH = Application.dataPath + "/../JerryDebug.txt";
            }
            else
            {
                LOG_FILE_PATH = Application.persistentDataPath + "/JerryDebug.txt";
            }
        }
    }

    public void AddCtr(CtrConfig config)
    {
        if (m_Active == false)
        {
            return;
        }

        CtrConfig oldConfig = m_Ctrs.Find((x) => x.m_Name.Equals(config.m_Name));
        if (oldConfig != null)
        {
            m_Ctrs.Remove(oldConfig);
        }

        if (config != null)
        {
            m_Ctrs.Add(config);
            JerryEventMgr.DispatchEvent(Enum_Event.AddOneCtr.ToString(), new object[] { config });
        }
    }

    #endregion 对外接口

    /// <summary>
    /// 增加系统配置
    /// </summary>
    private void AddSysCtr()
    {
        m_Ctrs.Add(new CtrConfig()
        {
            m_Name = "ReceiveMsg",
            m_Callback = (x) =>
            {
                m_ReceiveMsg = !m_ReceiveMsg;
                x.ChangeColor(m_ReceiveMsg ? Color.green : Color.white);
            },
            m_Color = m_ReceiveMsg ? Color.green : Color.white,
        });

        m_Ctrs.Add(new CtrConfig()
        {
            m_Name = "ToBottom",
            m_Callback = (x) =>
            {
                m_ToBottom = !m_ToBottom;
                x.ChangeColor(m_ToBottom ? Color.green : Color.white);
                if (m_ToBottom)
                {
                    JerryEventMgr.DispatchEvent(Enum_Event.Change2Bottom.ToString());
                }
            },
            m_Color = m_ToBottom ? Color.green : Color.white,
        });

        m_Ctrs.Add(new CtrConfig()
        {
            m_Name = "MessageReceived",
            m_Callback = (x) =>
            {
                m_UseMessageReceived = !m_UseMessageReceived;
                x.ChangeColor(m_UseMessageReceived ? Color.green : Color.white);
            },
            m_Color = m_UseMessageReceived ? Color.green : Color.white,
        });

        m_Ctrs.Add(new CtrConfig()
        {
            m_Name = "TraceLev",
            m_Callback = (x) =>
            {
                m_TraceLev = ChangeTraceLev(m_TraceLev);
                x.ChangeColor(GetTraceLevColor(m_TraceLev));
            },
            m_Color = GetTraceLevColor(m_TraceLev),
        });
    }

    private Color GetTraceLevColor(TraceLev lev)
    {
        switch (lev)
        {
            case TraceLev.None:
                {
                    return Color.white;
                }
            case TraceLev.Simple:
                {
                    return Color.cyan;
                }
            case TraceLev.Full:
                {
                    return Color.green;
                }
        }
        return Color.white;
    }

    private TraceLev ChangeTraceLev(TraceLev lev)
    {
        switch (lev)
        {
            case TraceLev.None:
                {
                    return TraceLev.Simple;
                }
            case TraceLev.Simple:
                {
                    return TraceLev.Full;
                }
            case TraceLev.Full:
                {
                    return TraceLev.None;
                }
        }
        return TraceLev.None;
    }

    protected void AddLog(object msg, Enum_LogType logType = Enum_LogType.Inf, Enum_LogTag tag = Enum_LogTag.All, bool isProtoMsg = false, bool isDirect = true)
    {
        if (!AddLogCheck(logType, tag))
        {
            return;
        }

        LogItem.LogInfo info = new LogItem.LogInfo();
        info.m_LogType = logType;
        info.m_LogTag = tag;
        if (isProtoMsg)
        {
            info.m_Msg = HandleProtoMsg(msg);
        }
        else
        {
            info.m_Msg = HandleInfo(msg);
        }
        info.m_Time = DateTime.Now.ToString("HH:mm:ss");
        if (isDirect && m_TraceLev != TraceLev.None)
        {
            info.m_Trace = StackTraceUtility.ExtractStackTrace();
            if (m_TraceLev == TraceLev.Simple)
            {
                int idx = info.m_Trace.IndexOf("UnityEngine.");
                if (idx != -1)
                {
                    info.m_Trace = info.m_Trace.Substring(0, idx);
                }
            }
        }

        if (m_OutScreen == true)
        {
            AddToScreen(info);
        }

        if (m_OutFile == true)
        {
            AddToFile(info.ToString());
        }

        if (m_OutConsole && !m_UseMessageReceived)//防止循环
        {
            AddToConsole(info.m_LogType, info.ToString());
        }
    }

    #region 输出到屏幕处理

    private void AddToScreen(LogItem.LogInfo info)
    {
        JerryEventMgr.DispatchEvent(Enum_Event.AddLog2Screen.ToString(), new object[] { info });
    }

    #endregion 输出到屏幕处理
}