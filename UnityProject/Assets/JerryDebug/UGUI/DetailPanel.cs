using System.Collections;
using Jerry;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanel : PanelBase
{
    private Text m_Text;

    public override void Init()
    {
        base.Init();

        m_Text = this.transform.FindChild("Viewport/Content/Text").GetComponent<Text>();

        JerryEventMgr.AddEvent(Enum_Event.SelectOneLog.ToString(), SelectOneLog);
    }

    void OnDestroy()
    {
        JerryEventMgr.RemoveEvent(Enum_Event.SelectOneLog.ToString(), SelectOneLog);
    }

    public void SelectOneLog(object[] args)
    {
        if (!m_Inited)
        {
            return;
        }
        if (args == null || args.Length != 1)
        {
            return;
        }
        LogItem.LogInfo data = (LogItem.LogInfo)args[0];
        m_Text.text = data.ToString();
        m_Text.color = JerryDebugUGUI.GetLogColor(data.m_LogType);
    }
}