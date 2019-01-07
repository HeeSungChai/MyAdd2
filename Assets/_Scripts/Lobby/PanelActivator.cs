using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    public UIPanel m_panel;
    public GameObject m_objContents;
    public float m_fDuration;
    public float m_fAlphaStart;
    public float m_fAlphaTarget;

    public void OnActivate()
    {
        if (m_objContents == null)
            m_objContents = GetComponentsInChildren<GameObject>()[1];

        if (m_objContents)
            m_objContents.SetActive(true);

        StopCoroutine("CoroutinePanelAlphaCtrl");
        StartCoroutine("CoroutinePanelAlphaCtrl", true);
    }

    public void OnDeactivate()
    {
        if (m_objContents)
            m_objContents.SetActive(false);

        StopCoroutine("CoroutinePanelAlphaCtrl");
        StartCoroutine("CoroutinePanelAlphaCtrl", false);
    }

    IEnumerator CoroutinePanelAlphaCtrl(bool bActivated)
    {
        float fElased = 0.0f;

        float fLerpStart;
        float fLerpEnd;

        if (bActivated)
        {
            fLerpStart = m_fAlphaStart;
            fLerpEnd = m_fAlphaTarget;
        }
        else
        {
            fLerpStart = m_fAlphaTarget;
            fLerpEnd = m_fAlphaStart;
        }

        while (fElased < m_fDuration)
        {
            fElased += Time.deltaTime;

            m_panel.alpha = Mathf.Lerp(fLerpStart, fLerpEnd, fElased / m_fDuration);

            yield return null;
        }

        m_panel.alpha = fLerpEnd;
    }
}
