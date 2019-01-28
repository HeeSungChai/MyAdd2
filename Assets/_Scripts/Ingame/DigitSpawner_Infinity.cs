using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class DigitSpawner
{
    eTABLE_LIST m_eTableStageLevelNext;
    public float m_fSpawnDelay = 3.0f;
    int m_iTotalCountOfCurTable;
    int m_iTotalCountOfNextTable;
    bool bIsFirstSpawn = true;
    int m_iLineID_Infinite = 1;

    IEnumerator CoroutineSpawn_Infinite()
    {
        float fElased = 0f;
        while (MyGlobals.StageMgr.StageState < STAGE_STATE.GAMECLEAR)
        {
            if(MyGlobals.StageMgr.IsPauseDrop)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            fElased += Time.deltaTime;

            //간혹 타깃이 지정되지 않는 경우에 대한 예외처리
            if (DigitsCount > 0)
                IfNotLockOnFindTargetAgain();

            if (fElased > m_fSpawnDelay)
            {
                fElased = 0f;
                if (m_iLineID <= m_iTotalCountOfCurTable)
                {
                    SpawnOne_Infinite();

                    if (bIsFirstSpawn)
                    {
                        m_labelNext.gameObject.SetActive(true);
                        bIsFirstSpawn = false;
                        OnChangeTarget_Infinite();
                    }
                }
                else
                {
                    //next table이 cur table이 되고 lineID는 다시 1부터
                    GetStageLevelTable();
                    SpawnOne_Infinite();
                }
            }

            yield return new WaitForFixedUpdate();
        }

        //while (DigitsCount > 0)
        //{
        //    yield return null;
        //}

        yield return new WaitForSeconds(1.0f);

        if (MyGlobals.StageMgr.StageState != STAGE_STATE.GAMEOVER)
            EventListener.Broadcast("OnGameClear");
    }

    void OnChangeTarget_Infinite()
    {
        if (DigitsCount <= 0)
        {
            SpawnOne_Infinite();
        }

        FindTarget_Infinite();
        MyUtility.DebugLog("TargetID : " + TargetID);
    }

    //void IfNotLockOnFindTargetAgain()
    //{
    //    bool bLockedOnAlready = false;
    //    for (int i = 0; i < m_objPool.Count; ++i)
    //    {
    //        if (m_objPool[i].gameObject.activeSelf == false)
    //            continue;

    //        if (m_objPool[i].GetComponent<NumDropCtrl>().IsLockedOn())
    //            bLockedOnAlready = true;
    //    }

    //    if (bLockedOnAlready == false)
    //    {
    //        MyUtility.DebugLog("LockOnNeeded");
    //        FindTarget_Infinite();
    //    }
    //}

    void FindTarget_Infinite()
    {
        for (int i = 0; i < m_objPool.Count; ++i)
        {
            if (m_objPool[i].gameObject.activeSelf == false)
                continue;

            m_scriptCurDigit = m_objPool[i].GetComponent<NumDropCtrl>();
            if (TargetID == m_scriptCurDigit.LineID_Infinite)
            {
                m_scriptTarget = m_scriptCurDigit;
                m_scriptTarget.LockOn();
            }
        }

        if (m_scriptTarget)
            LowestDigit = m_scriptTarget.GetDigit();

        EventListener.Broadcast("OnTargetChanged");
    }

    void SpawnOne_Infinite()
    {
        int iObjIndex = Random.Range(0, m_objPool.Count);

        if (m_objPool[iObjIndex].activeSelf == false)
        {
            //라인 1~5 에 랜덤하게 소환.
            iIndexSpawnPos = Random.Range(0, 5);

            if (iNextNum == -1)
                iCurNum = GetNextNum();
            else
                iCurNum = iNextNum;

            //일거리. 인피닛모드 테이블에서 속도 읽어와 반영하기
            fDuration = ((float)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.f_SPEED_VALUE));
            m_objPool[iObjIndex].GetComponent<NumDropCtrl>().Init(m_arrSpawnPos[iIndexSpawnPos], iCurNum, fDuration, m_iLineID, m_iLineID_Infinite);
            m_objPool[iObjIndex].SetActive(true);
            ++m_iLineID_Infinite;
            ++m_iLineID;
            //m_fAppearTime = ((float)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.f_APPEARTIME));

            ++DigitsCount;

            if (m_iLineID <= m_iTotalCountOfCurTable)
            {
                iNextNum = GetNextNum();
            }
            else
            {
                GetStageLevelTable();
                iNextNum = GetNextNum();
            }

            SetNextDigit_Infinite();
            m_bSpawnedOneRecently = true;
        }
        else
        {
            if (CheckUsableExist())
                this.SpawnOne_Infinite();
        }
    }

    void SetNextDigit_Infinite()
    {
        m_labelNext.text = iNextNum.ToString();
    }
}
