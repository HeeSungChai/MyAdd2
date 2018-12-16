using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitSpawner : MonoBehaviour
{
    public List<GameObject> m_listSpawnObj;
    public float m_fMinDelay = 10.0f;
    public float m_fMaxDelay = 30.0f;
    public bool m_bUseObjectPool = false;
    public int m_iObjPoolCapacity = 1;
    private List<GameObject> m_objPool;
    public bool m_bLimitedSpawnCount = false;
    public int m_iMaxSpawnCount = 20;
    private int m_iSpawnedCount;

    void Start()
    {
        if (m_bUseObjectPool)
        {
            m_objPool = new List<GameObject>();
            SetToObjPool();
        }

        StartCoroutine(CoroutineSpawn());
    }

    public void SetObjPoolCapacity(int iCapacity)
    {
        m_iObjPoolCapacity = iCapacity;
    }

    public void SetMaxSpawnCount(int iMaxCount)
    {
        m_iMaxSpawnCount = iMaxCount;
    }

    int m_iCurInstCount = 0;
    void SetToObjPool()
    {
        for (int i = 0; i < m_listSpawnObj.Count; ++i)
        {
            GameObject tempObj = Instantiate(m_listSpawnObj[i]);
            tempObj.SetActive(false);
            m_objPool.Add(tempObj);
            ++m_iCurInstCount;
        }

        if (m_iCurInstCount < m_iObjPoolCapacity)
            SetToObjPool();
    }

    public void SetMinMaxDelay(float fMin, float fMax)
    {
        m_fMinDelay = fMin;
        m_fMaxDelay = fMax;
    }

    virtual public IEnumerator CoroutineSpawn()
    {
        //while (MyGlobals.StageMgr.StageState < STAGE_STATE.RUNNING)
        //{
        //    yield return null;
        //}

        float fElased;
        float fDelay;
        while (MyGlobals.StageMgr.StageState < STAGE_STATE.GAMECLEAR)
        {
            if (m_iSpawnedCount >= m_iMaxSpawnCount)
                yield break;

            fDelay = Random.Range(m_fMinDelay, m_fMaxDelay);

            //yield return new WaitForSeconds(fDelay);
            fElased = 0.0f;
            while (fElased < fDelay)
            {
                if (MyGlobals.StageMgr.IsPauseScroll)
                {
                    yield return null;
                    continue;
                }
                fElased += Time.deltaTime;
                yield return null;
            }

            if (m_bUseObjectPool)
            {
                SpawnOne();
            }
            else
            {
                int iObjIndex = Random.Range(0, m_listSpawnObj.Count);

                Instantiate(m_listSpawnObj[iObjIndex]);
            }

            yield return null;
        }
    }

    void SpawnOne()
    {
        int iObjIndex = Random.Range(0, m_objPool.Count);

        if (m_objPool[iObjIndex].activeSelf == false)
        {
            m_objPool[iObjIndex].SetActive(true);
            ++m_iSpawnedCount;
        }
        else
        {
            if (CheckUsableExist())
                this.SpawnOne();
        }
    }

    bool CheckUsableExist()
    {
        for (int i = 0; i < m_objPool.Count; ++i)
        {
            if (m_objPool[i].activeSelf == false)
                return true;
        }

        ++m_iObjPoolCapacity;
        for (int i = 0; i < m_listSpawnObj.Count; ++i)
        {
            GameObject tempObj = Instantiate(m_listSpawnObj[i]);
            tempObj.SetActive(false);
            m_objPool.Add(tempObj);
            ++m_iCurInstCount;
        }

        return true;
    }
}
