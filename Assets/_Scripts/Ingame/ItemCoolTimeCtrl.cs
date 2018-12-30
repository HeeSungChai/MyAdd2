using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoolTimeCtrl : MonoBehaviour
{
    public UISprite m_sprBtn;
    public UISprite m_sprIcon;
    public float m_fCoolTime;
    float m_fFillAmount;
    bool m_bIsCoolTime;

    public bool IsCoolTime()
    {
        return m_bIsCoolTime;
    }

    public void OnActivate()
    {
        if (m_bIsCoolTime)
            return;

        StopCoroutine("CoroutineCoolTime");
        StartCoroutine("CoroutineCoolTime");
    }

    IEnumerator CoroutineCoolTime()
    {
        m_bIsCoolTime = true;
        float fElased = 0.0f;

        while (fElased < m_fCoolTime)
        {
            fElased += Time.deltaTime;
            m_fFillAmount = fElased / m_fCoolTime;

            m_sprBtn.fillAmount = m_fFillAmount;
            m_sprIcon.fillAmount = m_fFillAmount;

            yield return null;
        }

        m_sprBtn.fillAmount = 1.0f;
        m_sprIcon.fillAmount = 1.0f;
        m_bIsCoolTime = false;
    }
}
