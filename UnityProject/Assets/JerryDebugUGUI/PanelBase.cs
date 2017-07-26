using UnityEngine;

public class PanelBase : MonoBehaviour
{
    protected bool m_Inited = false;

    public virtual void Init()
    {
        m_Inited = true;
    }
}