using Jerry;
using UnityEngine.UI;

public class LogItem : LayoutItem
{
    private Text m_Text;

    private bool m_Awaked = false;

    private LogInfo curData = null;
    private LogInfo newData = null;
    private bool isDataDirty = false;

    void Awake()
    {
        m_Text = this.transform.FindChild("Text").GetComponent<Text>();

        //TODO，UGUIEventListener不能滑动
        this.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            JerryEventMgr.DispatchEvent(Enum_Event.SelectOneLog.ToString(), new object[] { curData });
        });
        m_Awaked = true;
        TryFillData();
    }

    public override void TryRefreshUI(ILayoutItemData data)
    {
        if (data == null)
        {
            return;
        }
        newData = data as LogInfo;
        isDataDirty = true;
        TryFillData();
    }

    private void TryFillData()
    {
        if (!m_Awaked
            || !isDataDirty)
        {
            return;
        }
        isDataDirty = false;
        curData = newData;

        m_Text.text = curData.ToString();
        m_Text.color = JerryDebugUGUI.GetLogColor(curData.m_LogType);
    }

    public class LogInfo : ILayoutItemData
    {
        public Enum_LogType m_LogType;
        public Enum_LogTag m_LogTag;
        public string m_Time;
        public string m_Msg;
        public string m_Trace;

        public override string ToString()
        {
            string ret = string.Format("{0} {1} ->\n{2}", m_LogTag, m_Time, m_Msg);
            if (!string.IsNullOrEmpty(m_Trace))
            {
                ret += "\n" + m_Trace;
            }
            return ret;
        }
    }
}