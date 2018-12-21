﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum eSPAWN_POS
//{
//    LEFT_FAR,
//    LEFT,
//    MIDDLE,
//    RIGHT,
//    RIGHT_FAR
//}

public class DigitSpawner : MonoBehaviour
{
    Transform m_transform;
    public List<GameObject> m_listOriginPref;
    public int m_iObjPoolCapacity = 1;
    private List<GameObject> m_objPool;
    private List<GameObject> m_listSpawnedObj;
    //public int m_iMaxSpawnCount = 20;
    eTABLE_LIST m_eTableStageLevel;
    int m_iLineID = 1;
    public Vector3[] m_arrSpawnPos;
    float m_fAppearTime;
    bool m_bSpawnedAll;
    NumDropCtrl m_scriptLowest;
    private int m_iLowestDigit;
    public int LowestDigit {
        get
        {
            //if (m_scriptLowest == null)
            //    return 0;
            //else
            //    return m_scriptLowest.GetDigit();
            return m_iLowestDigit;
        }
        set
        {
            if (value < 0)
                m_iLowestDigit = 0;
            else if (value > 81)
                m_iLowestDigit = 81;
            else
                m_iLowestDigit = value;
        }
    }

    private void Awake()
    {
        MyGlobals.DigitSpawner = this;
    }

    void Start()
    {
        m_bSpawnedAll = false;
        m_transform = this.transform;
        m_objPool = new List<GameObject>();
        SetToObjPool();

        //일거리. 현재 레벨 얻어와 반영하기
        m_eTableStageLevel = eTABLE_LIST.STAGE_LEVEL_1;
        m_fAppearTime = ((float)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.f_APPEARTIME));
        StartCoroutine("CoroutineSpawn");
        StartCoroutine("CoroutineCheckLowestDigit");
    }

    public void SetObjPoolCapacity(int iCapacity)
    {
        m_iObjPoolCapacity = iCapacity;
    }

    int m_iCurInstCount = 0;
    void SetToObjPool()
    {
        for (int i = 0; i < m_listOriginPref.Count; ++i)
        {
            GameObject tempObj = Instantiate(m_listOriginPref[i]);
            //tempObj.transform.parent = m_transform;
            tempObj.SetActive(false);
            m_objPool.Add(tempObj);
            ++m_iCurInstCount;
        }

        if (m_iCurInstCount < m_iObjPoolCapacity)
            SetToObjPool();
    }

    public IEnumerator CoroutineSpawn()
    {        
        while (MyGlobals.StageMgr.StageState < STAGE_STATE.GAMECLEAR && m_bSpawnedAll == false)
        {
            if(MyGlobals.StageMgr.PlayTime < m_fAppearTime)
            {
                yield return null;
                continue;
            }

            //Appeartime 이 되면 다음 숫자 소환
            SpawnOne();

            yield return null;
        }
    }

    NumDropCtrl m_scriptLowestPrev;
    NumDropCtrl m_scriptCurDigit;
    public IEnumerator CoroutineCheckLowestDigit()
    {
        float fHeightOfLowestObj = 10000f;
        float fHeightOfThisObj;
        //bool bLowestChanged = false;
        while (MyGlobals.StageMgr.StageState < STAGE_STATE.GAMECLEAR)
        {
            //소환된 숫자 없으면 리턴
            if (m_objPool.Count < 1)
            {
                yield return null;
                continue;
            }

            //lowest였던 애가 바닥에 닿으면 다른애가 lowest가 될 수 있도록
            if (fHeightOfLowestObj <= 0f)
                fHeightOfLowestObj = 10000f;

            //for (int i = 0; i < m_objPool.Count; ++i)
            //{
            //    if (m_objPool[i].gameObject.activeSelf == false)
            //        continue;

            //    //최초로 소환된 숫자는 무조건 lowest
            //    if (m_scriptLowestPrev == null)
            //    {
            //        m_scriptLowest = m_objPool[i].GetComponent<NumDropCtrl>();
            //        LowestChanged();
            //        yield return null;
            //        continue;
            //    }

            //    m_scriptCurDigit = m_objPool[i].GetComponent<NumDropCtrl>();

            //    if (m_scriptLowest == m_scriptCurDigit)
            //        continue;

            //    fHeightOfThisObj = m_scriptCurDigit.GetHeight();
            //    if (fHeightOfThisObj <= 0f)
            //        continue;

            //    fHeightOfLowestObj = m_scriptLowestPrev.GetHeight();
            //    if (fHeightOfLowestObj <= 0f)
            //        fHeightOfLowestObj = 10000f;
            //    if (fHeightOfLowestObj > fHeightOfThisObj)
            //    {
            //        m_scriptLowest = m_scriptCurDigit;
            //        bLowestChanged = true;
            //    }
            //}

            //활성화된 모든 숫자를 루프 돌면서 가장 높이가 낮은놈을 찾음
            for (int i = 0; i < m_objPool.Count; ++i)
            {
                if (m_objPool[i].gameObject.activeSelf == false)
                    continue;

                m_scriptCurDigit = m_objPool[i].GetComponent<NumDropCtrl>();
                if (m_scriptCurDigit.IsReachToBottom())
                    continue;

                fHeightOfThisObj = m_scriptCurDigit.GetHeight();
                if (fHeightOfLowestObj > fHeightOfThisObj)
                {
                    fHeightOfLowestObj = fHeightOfThisObj;
                    m_scriptLowest = m_scriptCurDigit;
                }
            }

            //그놈이 이전프레임이 가장 낮았던 놈이 아니면 숫자 입력부 갱신
            if(m_scriptLowestPrev == null || m_scriptLowest != m_scriptLowestPrev)
            {
                LowestChanged();
            }


            //if (bLowestChanged)
            //{
            //    bLowestChanged = false;
            //    LowestChanged();
            //}

            yield return null;
        }
    }

    void LowestChanged()
    {
        if (m_scriptLowest == null)
            return;
        //m_scriptLowest = m_scriptNewLowest;
        LowestDigit = m_scriptLowest.GetDigit();
        m_scriptLowestPrev = m_scriptLowest;
        EventListener.Broadcast("OnLowestChanged");
    }

    int iIndexSpawnPos;
    int iNextNum;
    float fDuration;
    void SpawnOne()
    {
        int iObjIndex = Random.Range(0, m_objPool.Count);

        if (m_objPool[iObjIndex].activeSelf == false)
        {
            //라인 1~5 에 랜덤하게 소환.
            iIndexSpawnPos = Random.Range(0, 5);

            iNextNum = GetNextNum();
            if (iNextNum == -1)
                return;

            fDuration = ((float)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.f_SPEED_VALUE));
            m_objPool[iObjIndex].GetComponent<NumDropCtrl>().Init(m_arrSpawnPos[iIndexSpawnPos], iNextNum, fDuration);
            m_objPool[iObjIndex].SetActive(true);
            ++m_iLineID;
            m_fAppearTime = ((float)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.f_APPEARTIME));
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
        for (int i = 0; i < m_listOriginPref.Count; ++i)
        {
            GameObject tempObj = Instantiate(m_listOriginPref[i]);
            //tempObj.transform.parent = m_transform;
            tempObj.SetActive(false);
            m_objPool.Add(tempObj);
            ++m_iCurInstCount;
        }

        return true;
    }

    string strExamManual;
    int[] m_arrExamManual = new int[5];
    int iMinValue;
    int iMaxValue;
    int GetNextNum()
    {
        //지정된 숫자를 출력하는 경우
        int iExamNum = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_NUM));

        if (iExamNum > 0)
            return iExamNum;

        bool bIsAllZero = false;
        strExamManual = ((string)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.s_EXAM_MANUAL));
        if (strExamManual.Equals("0/0/0/0/0"))
            bIsAllZero = true;

        //다섯개의 숫자중 하나를 고르는 경우
        if (bIsAllZero == false)
        {
            string[] keys = strExamManual.Split('/');

            //for (int i = 0; i < keys.Length; ++i)
            //{
            //    m_arrExamManual[i] = int.Parse(keys[i]);
            //}

            iExamNum = 0;
            while (iExamNum == 0)
            {
                iExamNum = int.Parse(keys[Random.Range(0, keys.Length)]);
            }
            return iExamNum;
        }

        //지정된 범위 내에서 랜덤하게 뽑는 경우
        iMinValue = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_FTRANDOM_MIN));
        iMaxValue = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_FTRANDOM_MAX));

        return Random.Range(iMinValue, iMaxValue + 1);
    }

    //int PickOne()
    //{
    //    return m_arrExamManual[Random.Range(0, m_arrExamManual.Length)];
    //}
}
