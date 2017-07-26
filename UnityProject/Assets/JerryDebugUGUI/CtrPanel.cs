using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Jerry;

public class CtrPanel : PanelBase
{
    private Transform m_Content;
    private Transform m_BtnPrefab;

    public override void Init()
    {
        base.Init();

        m_Content = this.transform.FindChild("Viewport/Content");
        m_BtnPrefab = this.transform.FindChild("BtnPrefab");
        m_BtnPrefab.gameObject.SetActive(false);

        AddSysCtrs();

        JerryEventMgr.AddEvent(Enum_Event.AddOneCtr.ToString(), AddOneCtr);
    }

    void OnDestroy()
    {
        JerryEventMgr.RemoveEvent(Enum_Event.AddOneCtr.ToString(), AddOneCtr);
    }

    private void AddSysCtrs()
    {
        foreach (CtrConfig ctr in JerryDebugUGUI.Inst.Ctrs)
        {
            AddOneCtr(new object[] { ctr });
        }
    }

    private void AddOneCtr(object[] args)
    {
        if (!m_Inited)
        {
            return;
        }
        if (args == null || args.Length != 1)
        {
            return;
        }
        CtrConfig ctr = (CtrConfig)args[0];
        CtrBtnItem ctrItem = JerryUtil.CloneGo<CtrBtnItem>(new JerryUtil.CloneGoData()
        {
            parant = m_Content,
            prefab = m_BtnPrefab.gameObject,
            active = true,
        });
        ctrItem.Init(ctr);
    }
}