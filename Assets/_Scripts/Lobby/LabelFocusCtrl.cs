using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelFocusCtrl : MonoBehaviour
{
    public UILabel m_label;
    Transform m_transform;
    public Vector3 m_vScaleFocus;
    Vector3 m_vScaleOrigin;
    public float m_fDuratinoScaleTrans;
    public Color m_colorOrigin;
    public Color m_colorFocused;
    public Color m_colorDisabled;

    private void Awake()
    {
        m_transform = transform;
        m_vScaleOrigin = m_transform.localScale;
    }

    public void SetStageNum(int iNum)
    {
        if (iNum <= 0 || iNum > MyGlobals.UserState.MaxStageLv)
            m_label.text = "";
        else if (iNum > MyGlobals.UserState.MaxSelectableLv)
        {
            m_label.color = m_colorDisabled;
            m_label.text = iNum.ToString();
        }
        else
        {
            m_label.color = m_colorOrigin;
            m_label.text = iNum.ToString();
        }
    }

    public void FocusOn()
    {
        m_label.color = m_colorFocused;

        StopCoroutine("CoroutineFocusScaler");
        StartCoroutine("CoroutineFocusScaler", true);
    }

    public void FocusOff()
    {
        m_label.color = m_colorOrigin;

        StopCoroutine("CoroutineFocusScaler");
        StartCoroutine("CoroutineFocusScaler", false);
    }

    IEnumerator CoroutineFocusScaler(bool bFocusOn)
    {
        float fElased = 0.0f;
        Vector3 vStart;
        Vector3 vTarget;
        if(bFocusOn)
        {
            vStart = m_vScaleOrigin;
            vTarget = m_vScaleFocus;
        }
        else
        {
            vStart = m_vScaleFocus;
            vTarget = m_vScaleOrigin;
        }

        while(fElased < m_fDuratinoScaleTrans)
        {
            fElased += Time.deltaTime;

            m_transform.localScale = Vector3.Lerp(vStart, vTarget, fElased / m_fDuratinoScaleTrans);

            yield return null;
        }
    }
}
