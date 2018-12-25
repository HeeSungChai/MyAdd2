using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarMgr : MonoBehaviour
{
    public HpCtrl[] m_arrScriptHpCtrl;
    int m_iIndexCurHp;
    public float m_fDelayActivateEachHp;

    private void Awake()
    {
        EventListener.AddListener("OnFailed", this);
    }

    void Start()
    {
        m_iIndexCurHp = m_arrScriptHpCtrl.Length - 1;
        for (int i = 0; i < m_arrScriptHpCtrl.Length; ++i)
        {
            m_arrScriptHpCtrl[i].gameObject.SetActive(false);
        }

        StartCoroutine("CoroutineActivateHp");
    }

    IEnumerator CoroutineActivateHp()
    {
        int iActivatedCount = 0;

        while(iActivatedCount < m_arrScriptHpCtrl.Length)
        {
            yield return new WaitForSeconds(m_fDelayActivateEachHp);

            m_arrScriptHpCtrl[iActivatedCount].gameObject.SetActive(true);
            m_arrScriptHpCtrl[iActivatedCount].ActivateHP();
            ++iActivatedCount;
        }

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

    }

    private void OnDestroy()
    {
        EventListener.RemoveListener(this);
    }
}
