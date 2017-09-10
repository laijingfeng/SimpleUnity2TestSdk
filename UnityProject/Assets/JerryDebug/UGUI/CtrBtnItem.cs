using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CtrBtnItem : MonoBehaviour
{
    private Text m_Text;
    private Image m_Image;

    private bool m_Awaked = false;
    private bool m_Inited = false;

    private CtrConfig m_Data;

    void Awake()
    {
        m_Text = this.transform.FindChild("Text").GetComponent<Text>();
        m_Image = this.transform.GetComponent<Image>();

        //TODO，UGUIEventListener不能滑动
        this.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (m_Inited)
            {
                if (m_Data.m_Callback != null)
                {
                    m_Data.m_Callback(this);
                }
            }
        });
        m_Awaked = true;
        TryWork();
    }

    public void Init(CtrConfig data)
    {
        m_Data = data;
        m_Inited = true;
        TryWork();
    }

    public void ChangeColor(Color col)
    {
        if (!Usefull())
        {
            return;
        }
        m_Data.m_Color = col;
        m_Image.color = m_Data.m_Color;
    }

    private bool Usefull()
    {
        return m_Inited && m_Awaked;
    }

    private void TryWork()
    {
        if (!Usefull())
        {
            return;
        }
        m_Text.text = m_Data.m_Name;
        m_Image.color = m_Data.m_Color;
    }
}