using Jerry;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatBtn : MonoBehaviour, IDragHandler
{
    private Canvas m_Canvas;
    private Vector3 m_LastDownPos = Vector3.zero;

    void Awake()
    {
        m_Canvas = this.transform.parent.GetComponent<Canvas>();

        UGUIEventListener.Get(this.gameObject).onDown += (go) =>
        {
            m_LastDownPos = JerryUtil.GetClickPos();
        };

        UGUIEventListener.Get(this.gameObject).onUp += (go) =>
        {
            Vector3 pos = JerryUtil.GetClickPos();
            if ((pos - m_LastDownPos).sqrMagnitude > 0.01f)
            {
                return;
            }
            DoClick();
        };
    }

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (m_Canvas != null)
        {
            this.transform.localPosition = JerryUtil.PosScreen2Canvas(m_Canvas, JerryUtil.GetClickPos(), this.transform);
        }
    }

    private void DoClick()
    {
        JerryEventMgr.DispatchEvent(Enum_Event.ShowOrHideLog.ToString());
    }
}