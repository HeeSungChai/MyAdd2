using System.Collections;
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

public partial class DigitSpawner : MonoBehaviour
{
    Transform m_transform;
    public List<GameObject> m_listOriginPref;
    public int m_iObjPoolCapacity = 1;
    private List<GameObject> m_objPool;
    //private List<GameObject> m_listSpawnedObj;
    //public int m_iMaxSpawnCount = 20;
    eTABLE_LIST m_eTableStageLevel;
    int m_iLineID;
    public Vector3[] m_arrSpawnPos;
    public float m_fHeightSpawn;
    public float m_fHeightGreat;
    public float m_fHeightCool;
    public float m_fHeightFail;
    float m_fAppearTime;
    bool m_bSpawnedAll;
    public Color m_colorLowest;
    public int TargetID { get; set; }
    public int TargetIDMax { get; set; }
    public NumDropCtrl m_scriptTarget;
    private int m_iLowestDigit;
    public int LowestDigit {
        get
        {
            //if (m_scriptTarget == null)
            //    return 0;
            //else
            //    return m_scriptTarget.GetDigit();
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
    public int DigitsCount { get; set; }

    public UILabel m_labelNext;

    private void Awake()
    {
        MyGlobals.DigitSpawner = this;

        EventListener.AddListener("OnCorrectAnswer", this);
        EventListener.AddListener("OnChangeTarget", this);
        //EventListener.AddListener("OnActivateSuperSkill", this);
    }

    void Start()
    {
        m_bSpawnedAll = false;
        m_transform = this.transform;
        m_objPool = new List<GameObject>();
        SetToObjPool();

        GetStageLevelTable();
        if(MyGlobals.StageMgr.GameType == INGAME_TYPE.ADVENTURE)
            StartCoroutine("CoroutineSpawn");
        else
            StartCoroutine("CoroutineSpawn_Infinite");
        //StartCoroutine("CoroutineCheckLowestDigit");
    }

    bool bIsFirstTableSetting = true;
    int m_iStageTableNumCur;
    int m_iStageTableNumNext;
    void GetStageLevelTable()
    {
        if (MyGlobals.StageMgr.IsAdventure())
        {
            m_eTableStageLevel = MyUtility.ParsingStringToEnumType<eTABLE_LIST>("STAGE_LEVEL_" + MyGlobals.StageMgr.StageNum);
            m_iLineID = 1;
            TargetID = 1;
            m_fAppearTime = GetNextDigitAppearTime();
        }
        else
        {
            if (bIsFirstTableSetting)
            {
                m_iStageTableNumCur = Random.Range(1, 51);
                m_iStageTableNumNext = Random.Range(1, 51);
                bIsFirstTableSetting = false;
                m_eTableStageLevel = MyUtility.ParsingStringToEnumType<eTABLE_LIST>("STAGE_LEVEL_" + m_iStageTableNumCur.ToString());
                m_eTableStageLevelNext = MyUtility.ParsingStringToEnumType<eTABLE_LIST>("STAGE_LEVEL_" + m_iStageTableNumNext.ToString());
                TargetID = 1;
            }
            else
            {
                m_eTableStageLevel = m_eTableStageLevelNext;
                m_iStageTableNumCur = m_iStageTableNumNext;
                m_iStageTableNumNext = Random.Range(1, 51);
                m_eTableStageLevelNext = MyUtility.ParsingStringToEnumType<eTABLE_LIST>("STAGE_LEVEL_" + m_iStageTableNumNext.ToString());
            }
            m_iTotalCountOfCurTable = (int)TableDB.Instance.GetRowCount(m_eTableStageLevel);
            m_iLineID = 1;
            MyUtility.DebugLog(string.Format("{0}, {1}", m_iStageTableNumCur, m_iTotalCountOfCurTable));
        }
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

    int m_iTotalCount;
    bool m_bSpawnedOneRecently;
    public IEnumerator CoroutineSpawn()
    {
        m_iTotalCount = (int)TableDB.Instance.GetRowCount(m_eTableStageLevel);
        //int iSpawedCount = 0;
        while (MyGlobals.StageMgr.StageState < STAGE_STATE.GAMECLEAR && m_bSpawnedAll == false)
        {
            //if (DigitsCount > 0)
            //    IfNotLockOnFindTargetAgain();

            if (MyGlobals.StageMgr.PlayTime < m_fAppearTime)
            {
                yield return null;
                continue;
            }

            //Appeartime 이 되면 다음 숫자 소환
            if (m_iLineID <= m_iTotalCount)
            {
                //++iSpawedCount;
                SpawnOne();

                if (m_iLineID == 2)
                    OnChangeTarget();
            }
            else
            {
                m_bSpawnedAll = true;
                MyUtility.DebugLog("SpawnedAll");
            }

            yield return null;
        }

        while(DigitsCount > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        if(MyGlobals.StageMgr.StageState != STAGE_STATE.GAMEOVER)
            EventListener.Broadcast("OnGameClear");
    }

    /// <summary>
    /// 가장 밑에 있는 숫자가 타깃이 되는 버전
    /// </summary>
    NumDropCtrl m_scriptTargetPrev;
    NumDropCtrl m_scriptCurDigit;
    public IEnumerator CoroutineCheckLowestDigit()
    {
        float fHeightOfLowestObj = 10000f;
        float fHeightOfThisObj;
        while (MyGlobals.StageMgr.StageState < STAGE_STATE.GAMECLEAR)
        {
            //소환된 숫자 없으면 리턴
            if (m_objPool.Count < 1)
            {
                yield return null;
                continue;
            }

            //lowest였던 애가 바닥에 닿으면 다른애가 lowest가 될 수 있도록
            //if (fHeightOfLowestObj <= 0f || (m_scriptTarget && m_scriptTarget.IsCorrect()))
            fHeightOfLowestObj = 10000f;

            //활성화된 모든 숫자를 루프 돌면서 가장 높이가 낮은놈을 찾음
            for (int i = 0; i < m_objPool.Count; ++i)
            {
                if (m_objPool[i].gameObject.activeSelf == false)
                    continue;

                m_scriptCurDigit = m_objPool[i].GetComponent<NumDropCtrl>();

                //if (m_scriptTarget && (m_scriptCurDigit == m_scriptTarget))
                //    continue;

                if (m_scriptCurDigit.IsReachToBottom() || m_scriptCurDigit.IsCorrect())
                    continue;

                fHeightOfThisObj = m_scriptCurDigit.GetHeight();
                if (fHeightOfLowestObj > fHeightOfThisObj)
                {
                    fHeightOfLowestObj = fHeightOfThisObj;
                    m_scriptTarget = m_scriptCurDigit;
                }
            }

            //그놈이 이전프레임이 가장 낮았던 놈이 아니면 숫자 입력부 갱신
            if (m_scriptTargetPrev == null ||
                m_scriptTarget != m_scriptTargetPrev ||
                m_scriptTarget.GetDigit() != LowestDigit)
            {
                LowestChanged();
            }

            yield return null;
        }
    }

    void LowestChanged()
    {
        if (m_scriptTarget == null)
            return;
        //m_scriptTarget = m_scriptNewLowest;
        LowestDigit = m_scriptTarget.GetDigit();

        if (m_scriptTargetPrev)
            m_scriptTargetPrev.SetTextColorDefault();

        m_scriptTarget.SetTextColor(m_colorLowest);
        m_scriptTargetPrev = m_scriptTarget;
        EventListener.Broadcast("OnTargetChanged");
    }

    /// <summary>
    /// 높이에 상관없이 LineID 순서로 타깃이 바뀌는 버전
    /// </summary>
    public void OnChangeTarget()
    {
        if (MyGlobals.StageMgr.GameType != INGAME_TYPE.ADVENTURE)
        {
            OnChangeTarget_Infinite();
            return;
        }

        if (TargetID > m_iTotalCount)
        {
            m_bSpawnedAll = true;
            return;
        }

        //소환된 숫자가 하나도 없으면 0.3초 후 하나 소환
        if (DigitsCount <= 0)
        {
            StopCoroutine("CoroutineSpawnOneAfterDelay");
            StartCoroutine("CoroutineSpawnOneAfterDelay");
            return;
        }

        FindTarget();
    }

    void IfNotLockOnFindTargetAgain()
    {
        bool bLockedOnAlready = false;
        for (int i = 0; i < m_objPool.Count; ++i)
        {
            if (m_objPool[i].gameObject.activeSelf == false)
                continue;

            if (m_objPool[i].GetComponent<NumDropCtrl>().IsLockedOn())
                bLockedOnAlready = true;
        }

        if (bLockedOnAlready == false)
        {
            MyUtility.DebugLog("LockOnNeeded");
            if(MyGlobals.StageMgr.IsAdventure())
                FindTarget();
            else
                FindTarget_Infinite();
        }
    }

    void FindTarget()
    {
        for (int i = 0; i < m_objPool.Count; ++i)
        {
            if (m_objPool[i].gameObject.activeSelf == false)
                continue;

            m_scriptCurDigit = m_objPool[i].GetComponent<NumDropCtrl>();
            if (TargetID == m_scriptCurDigit.LineID)
            {
                m_scriptTarget = m_scriptCurDigit;
                m_scriptTarget.LockOn();
            }
        }

        if (m_scriptTarget)
            LowestDigit = m_scriptTarget.GetDigit();

        EventListener.Broadcast("OnTargetChanged");
    }

    IEnumerator CoroutineSpawnOneAfterDelay()
    {
        m_bSpawnedOneRecently = false;
        float fElased = 0f;
        while(fElased < 0.3f)
        {
            if(MyGlobals.StageMgr.IsPauseDrop)
            {
                yield return null;
                continue;
            }

            if (m_bSpawnedOneRecently)
                yield break;

            fElased += Time.deltaTime;

            yield return null;
        }

        SpawnOne();
        FindTarget();
    }

    ////라인ID에 따라 타겟을 지정하는 버전
    //public IEnumerator CoroutineCheckLowestDigitByID()
    //{
    //    while (MyGlobals.StageMgr.StageState < STAGE_STATE.GAMECLEAR)
    //    {
    //        //해당 숫자를 맞추거나
    //        for (int i = 0; i < m_objPool.Count; ++i)
    //        {
    //            if (m_objPool[i].gameObject.activeSelf == false)
    //                continue;

    //            m_scriptCurDigit = m_objPool[i].GetComponent<NumDropCtrl>();
    //            m_scriptTarget = m_scriptCurDigit;
    //        }

    //        //그놈이 이전프레임이 가장 낮았던 놈이 아니면 숫자 입력부 갱신
    //        if (m_scriptTargetPrev == null ||
    //            m_scriptTarget != m_scriptTargetPrev ||
    //            m_scriptTarget.GetDigit() != LowestDigit)
    //        {
    //            LowestChanged();
    //        }

    //        yield return null;
    //    }
    //}

    int iIndexSpawnPos;
    int iCurNum;
    int iNextNum = -1;
    float fDuration;
    void SpawnOne()
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

            fDuration = ((float)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.f_SPEED_VALUE));
            m_objPool[iObjIndex].GetComponent<NumDropCtrl>().Init(m_arrSpawnPos[iIndexSpawnPos], iCurNum, fDuration, m_iLineID);
            m_objPool[iObjIndex].SetActive(true);
            ++m_iLineID;
            //m_fAppearTime = ((float)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.f_APPEARTIME));

            if(m_iLineID <= m_iTotalCount)
                m_fAppearTime = GetNextDigitAppearTime();
            ++DigitsCount;

            iNextNum = GetNextNum();

            SetNextDigit();
            m_bSpawnedOneRecently = true;
        }
        else
        {
            if (CheckUsableExist())
                this.SpawnOne();
        }
    }

    void SetNextDigit()
    {
        if (m_iLineID <= m_iTotalCount)
        {
            m_labelNext.gameObject.SetActive(true);
            m_labelNext.text = iNextNum.ToString();
        }
        else
        {
            m_labelNext.gameObject.SetActive(false);
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

    //string strExamManual;
    int[] m_arrExamManual = new int[5];
    int iMinValue;
    int iMaxValue;
    int GetNextNum()
    {
        //지정된 숫자를 출력하는 경우
        int iExamNum = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_NUM));

        if (iExamNum > -1)
            return iExamNum;

        ////다섯개의 숫자중 하나를 고르는 경우(하나의 column에서 /로 나누는 경우)
        //bool bIsAllZero = false;
        //strExamManual = ((string)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.s_EXAM_MANUAL));
        //if (strExamManual.Equals("0/0/0/0/0"))
        //    bIsAllZero = true;
        
        //다섯개의 숫자중 하나를 고르는 경우(각각 별도의 column을 사용하는 경우)
        bool bUseOneOfExamManual = false;
        m_arrExamManual[0] = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_MANUAL_1));
        m_arrExamManual[1] = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_MANUAL_2));
        m_arrExamManual[2] = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_MANUAL_3));
        m_arrExamManual[3] = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_MANUAL_4));
        m_arrExamManual[4] = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_MANUAL_5));

        for (int i = 0; i < m_arrExamManual.Length; ++i)
        {
            if (m_arrExamManual[i] != -1)
                bUseOneOfExamManual = true;
        }

        if (bUseOneOfExamManual)
        {
            iExamNum = -1;
            while (iExamNum == -1)
            {
                iExamNum = m_arrExamManual[Random.Range(0, m_arrExamManual.Length)];
            }

            return iExamNum;
        }

        //지정된 범위 내에서 랜덤하게 뽑는 경우
        iMinValue = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_FTRANDOM_MIN));
        iMaxValue = ((int)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.i_EXAM_FTRANDOM_MAX));
        iMinValue = Mathf.Clamp(iMinValue, 0, MyGlobals.MaxValue);
        iMaxValue = Mathf.Clamp(iMaxValue, 0, MyGlobals.MaxValue);

        return Random.Range(iMinValue, iMaxValue + 1);
    }

    string strAppearTime;
    string[] arrkeys;
    float fMinute;
    float fSecond;
    float fMilliSecond;
    float GetNextDigitAppearTime()
    {
        strAppearTime = ((string)TableDB.Instance.GetData(m_eTableStageLevel, m_iLineID, eKEY_TABLEDB.s_APPEARTIME));
        arrkeys = strAppearTime.Split('/');
        fMinute = float.Parse(arrkeys[0]);
        fSecond = float.Parse(arrkeys[1]);
        fMilliSecond = float.Parse(arrkeys[2]);
        strAppearTime = (fMinute * 60f + fSecond).ToString() + '.' + fMilliSecond.ToString();
        return float.Parse(strAppearTime);
    }

    public eEVALUATION m_eEvaluation;
    float fCurHeight;
    void OnCorrectAnswer(bool bByItem)
    {
        fCurHeight = m_scriptTarget.GetHeight();

        if (fCurHeight >= m_fHeightGreat)
            m_eEvaluation = eEVALUATION.GREAT;
        else if (fCurHeight >= m_fHeightCool)
            m_eEvaluation = eEVALUATION.COOL;
        else
            m_eEvaluation = eEVALUATION.NICE;

        m_scriptTarget.Correct(m_eEvaluation);

        if (!MyGlobals.StageMgr.IsAdventure())
        {
            m_fSpawnDelay -= 0.1f;
            m_fSpawnDelay = Mathf.Clamp(m_fSpawnDelay, MyGlobals.StageMgr.m_fSpawnDelayMin, MyGlobals.StageMgr.m_fSpawnDelayMax);
            //++MyGlobals.StageMgr.ComboCount;
            //if(MyGlobals.StageMgr.ComboCount > 0)//두번째 성공부터 콤보로 침
            //    ++MyGlobals.StageMgr.TotalComboCount;
        }

        MyGlobals.ScoreMgr.UpdateScore(m_eEvaluation, bByItem);
    }

    //void OnActivateSuperSkill()
    //{
    //    NumDropCtrl tempScript;
    //    for (int i = 0; i < m_objPool.Count; ++i)
    //    {
    //        if (m_objPool[i].gameObject.activeSelf == false)
    //            continue;

    //        tempScript = m_objPool[i].GetComponent<NumDropCtrl>();

    //        fCurHeight = tempScript.GetHeight();
    //        if (fCurHeight >= m_fHeightGreat)
    //            m_eEvaluation = eEVALUATION.GREAT;
    //        else if (fCurHeight >= m_fHeightCool)
    //            m_eEvaluation = eEVALUATION.COOL;
    //        else
    //            m_eEvaluation = eEVALUATION.NICE;

    //        //m_scriptTarget.Correct(m_eEvaluation);

    //        MyGlobals.ScoreMgr.UpdateScore(m_eEvaluation, false);
    //    }
    //}
    
    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
