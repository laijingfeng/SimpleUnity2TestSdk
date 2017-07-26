using System.Collections.Generic;
using Jerry;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : PanelBase
{
    private Transform m_LogContent;
    private Transform m_LogPrefab;
    private InputField m_FilterInput;
    private Dropdown m_TypeDropdown;
    private Dropdown m_TagDropdown;

    private List<LogItem.LogInfo> m_LogItemInfoList = new List<LogItem.LogInfo>();
    private LogLayout layout;
    private string[] filterStrs;
    private Enum_LogType filterType = Enum_LogType.All;
    private Enum_LogTag filterTag = Enum_LogTag.All;

    public override void Init()
    {
        base.Init();

        m_LogContent = this.transform.FindChild("Log/Viewport/Content");
        layout = m_LogContent.gameObject.AddComponent<LogLayout>();

        m_LogPrefab = this.transform.FindChild("Log/LogPrefab");
        m_LogPrefab.gameObject.SetActive(false);
        m_FilterInput = this.transform.FindChild("BaseCtr/InputField").GetComponent<InputField>();
        m_FilterInput.onEndEdit.AddListener((str) =>
        {
            filterStrs = JerryUtil.String2TArray<string>(str, ';');
            RefreshList();
        });
        m_TypeDropdown = this.transform.FindChild("BaseCtr/TypeDropdown").GetComponent<Dropdown>();
        m_TypeDropdown.options.Clear();
        m_TypeDropdown.options.AddRange(new List<Dropdown.OptionData>()
        {
            new Dropdown.OptionData(){ text = Enum_LogType.All.ToString() },
            new Dropdown.OptionData(){ text = Enum_LogType.Inf.ToString() },
            new Dropdown.OptionData(){ text = Enum_LogType.War.ToString() },
            new Dropdown.OptionData(){ text = Enum_LogType.Err.ToString() },
        });
        m_TypeDropdown.onValueChanged.AddListener((val) =>
        {
            filterType = (Enum_LogType)val;
            RefreshList();
        });

        m_TagDropdown = this.transform.FindChild("BaseCtr/TagDropdown").GetComponent<Dropdown>();
        m_TagDropdown.options.Clear();
        m_TagDropdown.options.AddRange(new List<Dropdown.OptionData>()
        {
            new Dropdown.OptionData(){ text = Enum_LogTag.All.ToString() },
            new Dropdown.OptionData(){ text = Enum_LogTag.Tag0.ToString() },
            new Dropdown.OptionData(){ text = Enum_LogTag.Tag1.ToString() },
            new Dropdown.OptionData(){ text = Enum_LogTag.Tag2.ToString() },
        });
        m_TagDropdown.onValueChanged.AddListener((val) =>
        {
            filterTag = (Enum_LogTag)val;
            RefreshList();
        });

        JerryDebugUGUI.Inst.AddCtr(new CtrConfig()
        {
            m_Name = "Clear",
            m_Color = Color.red,
            m_Callback = (x) =>
            {
                CleanAllLog();
            },
        });

        CleanAllLog();
        JerryEventMgr.AddEvent(Enum_Event.AddLog2Screen.ToString(), AddLog2Screen);
        JerryEventMgr.AddEvent(Enum_Event.Change2Bottom.ToString(), EventChange2Bottom);
    }

    void OnEnable()
    {
        RefreshList();
    }

    void OnDestroy()
    {
        JerryEventMgr.RemoveEvent(Enum_Event.AddLog2Screen.ToString(), AddLog2Screen);
        JerryEventMgr.RemoveEvent(Enum_Event.Change2Bottom.ToString(), EventChange2Bottom);
    }

    private void CleanAllLog()
    {
        m_LogItemInfoList.Clear();
        RefreshList();
    }

    private void EventChange2Bottom(object[] args)
    {
        RefreshList();
    }

    private void AddLog2Screen(object[] args)
    {
        if (args == null || args.Length != 1)
        {
            return;
        }

        LogItem.LogInfo data = (LogItem.LogInfo)args[0];
        m_LogItemInfoList.Add(data);
        RefreshList();
    }

    private void RefreshList()
    {
        if (!m_Inited || !this.gameObject.activeInHierarchy)
        {
            return;
        }

        List<LogItem.LogInfo> datas = new List<LogItem.LogInfo>();
        foreach (LogItem.LogInfo info in m_LogItemInfoList)
        {
            if (JudgeLogInfo(info))
            {
                datas.Add(info);
            }
        }

        if (!layout.Inited)
        {
            layout.DoInit(new LayoutConfig()
            {
                prefab = m_LogPrefab,
                dir = GridLayoutGroup.Axis.Vertical,
                bufHalfCnt = 1,
                cellSize = new Vector2(480, 80),
                spacing = Vector2.zero,
                frameWorkCnt = 3,
                dirCellWidth = 1,
                dirViewLen = 700,
                progress = JerryDebugUGUI.Inst.ToBottom ? 1.0f : 0.0f,
            }, datas);
        }
        else
        {
            layout.RefreshDatas(datas, new ModifyConfig()
            {
                progress = JerryDebugUGUI.Inst.ToBottom ? 1.0f : layout.CurProgress(),
            });
        }
    }

    private bool JudgeLogInfo(LogItem.LogInfo info)
    {
        if (info == null)
        {
            return false;
        }

        if(info.m_LogType != Enum_LogType.All
            && filterType != Enum_LogType.All
            && filterType != info.m_LogType)
        {
            return false;
        }

        if (info.m_LogTag != Enum_LogTag.All
            && filterTag != Enum_LogTag.All
            && filterTag != info.m_LogTag)
        {
            return false;
        }

        if (filterStrs == null || filterStrs.Length <= 0)
        {
            return true;
        }

        foreach (string s in filterStrs)
        {
            if (info.m_Msg.ToLower().Contains(s.ToLower()))
            {
                return true;
            }
        }
        return false;
    }
}