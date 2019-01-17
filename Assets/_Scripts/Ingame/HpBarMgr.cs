using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarMgr : MonoBehaviour
{
    public HpCtrl[] m_arrScriptHpCtrl;
    int m_iFullHP;
    int m_iIndexCurHp;
    public float m_fDelayActivateEachHp;

    private void Awake()
    {
        MyGlobals.HpBarMgr = this;
        EventListener.AddListener("OnFailed", this);
        EventListener.AddListener("OnRecoverHp", this);
    }

    void Start()
    {
        m_iFullHP = m_arrScriptHpCtrl.Length;
        m_iIndexCurHp = m_iFullHP - 1;
        for (int i = 0; i < m_iFullHP; ++i)
        {
            m_arrScriptHpCtrl[i].gameObject.SetActive(false);
        }

        StartCoroutine("CoroutineActivateHp");
    }

    IEnumerator CoroutineActivateHp()
    {
        int iActivatedCount = 0;

        while(iActivatedCount < m_iFullHP)
        {
            yield return new WaitForSeconds(m_fDelayActivateEachHp);

            m_arrScriptHpCtrl[iActivatedCount].gameObject.SetActive(true);
            m_arrScriptHpCtrl[iActivatedCount].ActivateHP();
            ++iActivatedCount;
        }

    }

    public bool IsHpFull()
    {
        if (m_iIndexCurHp + 1 == m_iFullHP)
            return true;
        else
            return false;
    }

    public void OnRecoverHp()
    {
        ++m_iIndexCurHp;
        m_arrScriptHpCtrl[m_iIndexCurHp].gameObject.SetActive(true);
        m_arrScriptHpCtrl[m_iIndexCurHp].ActivateHP();
    }

    void OnFailed()
    {
        m_arrScriptHpCtrl[m_iIndexCurHp].DisableHP();
        --m_iIndexCurHp;

        if (m_iIndexCurHp < 0)
            GameOver();
    }

    void GameOver()
    {
        EventListener.Broadcast("OnGameOver", false);
    }

    private void OnDestroy()
    {
        EventListener.RemoveListener(this);
    }
}
