using System;
using Jerry;
using UnityEngine;

public enum Enum_Event
{
    ShowOrHideLog = 0,
    AddLog2Screen,
    SelectOneLog,
    AddOneCtr,
    /// <summary>
    /// 修改配置
    /// </summary>
    Change2Bottom,
}

public class CtrConfig
{
    public string m_Name = "Ctr";
    public Color m_Color = Color.white;
    public Action<CtrBtnItem> m_Callback = null;
}