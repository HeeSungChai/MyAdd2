using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRouletteCtrl : MonoBehaviour
{
    public UserState m_scriptUserState;
    public Transform m_transRoulette;
    public LabelFocusCtrl[] m_arrLabelStageNum;
    //UILabel m_labelFocused;
    int m_iIndexFocusedLabel;
    int m_iTotalLabelCount;
    public float m_fEachRotDuration;
    float m_fRotDegreeCur;
    float m_fRotDegreeTarget;
    int m_iSelectedStageLv;
    int m_iMaxSelectableLv;
    //public UILabel m_labelBestScoreValue;
    //public UILabel m_labelTotalScoreValue;

    private void Awake()
    {
        m_fRotDegreeCur = 0f;
        m_fRotDegreeTarget = 0f;
        m_iIndexFocusedLabel = 0;
        MyGlobals.StageNum = 1;
        m_iTotalLabelCount = m_arrLabelStageNum.Length;
    }

    private void OnEnable()
    {
        //m_labelBestScoreValue.text = PrefsMgr.Instance.GetInt(PrefsMgr.strStageScore + MyGlobals.StageNum, 0).ToString();

        //int iTotalPoint = 0;
        //for (int i = 1; i <= 50; ++i)
        //{
        //    iTotalPoint += PrefsMgr.Instance.GetInt(PrefsMgr.strStageScore + i.ToString(), 0);
        //}
        //m_labelTotalScoreValue.text = iTotalPoint.ToString();
    }

    private void Start()
    {
        SetStageNumber(1);
        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();
    }

    void SetStageNumber(int iStandardLevel)
    {
        int iIndexPrev = GetPrevIndex(m_iIndexFocusedLabel);
        int iIndexPrevPrev = GetPrevIndex(iIndexPrev);
        int iIndexPrevPrevPrev = GetPrevIndex(iIndexPrevPrev);
        int iIndexNext = GetNextIndex(m_iIndexFocusedLabel);
        int iIndexNextNext = GetNextIndex(iIndexNext);
        int iIndexNextNextNext = GetNextIndex(iIndexNextNext);

        m_arrLabelStageNum[iIndexPrevPrevPrev].SetStageNum(iStandardLevel - 3);
        m_arrLabelStageNum[iIndexPrevPrev].SetStageNum(iStandardLevel - 2);
        m_arrLabelStageNum[iIndexPrev].SetStageNum(iStandardLevel - 1);
        m_arrLabelStageNum[m_iIndexFocusedLabel].SetStageNum(iStandardLevel);
        m_arrLabelStageNum[iIndexNext].SetStageNum(iStandardLevel + 1);
        m_arrLabelStageNum[iIndexNextNext].SetStageNum(iStandardLevel + 2);
        m_arrLabelStageNum[iIndexNextNextNext].SetStageNum(iStandardLevel + 3);
    }

    public void OnPressed_HigherLevel()
    {
        if (MyGlobals.StageNum >= MyGlobals.UserState.MaxSelectableLv)
            return;

        ++MyGlobals.StageNum;

        //m_labelBestScoreValue.text = PrefsMgr.Instance.GetInt(PrefsMgr.strStageScore + MyGlobals.StageNum, 0).ToString();
        EventListener.Broadcast("OnStageChanged");

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();

        m_fRotDegreeTarget += 30f;
        ++m_iIndexFocusedLabel;
        if (m_iIndexFocusedLabel >= m_iTotalLabelCount)
            m_iIndexFocusedLabel = 0;

        SetStageNumber(MyGlobals.StageNum);

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();

        StopCoroutine("CoroutineRotateRoulette");
        StartCoroutine("CoroutineRotateRoulette");
    }

    public void OnPressed_LowerLevel()
    {
        if (MyGlobals.StageNum <= 1)
            return;

        --MyGlobals.StageNum;

        //m_labelBestScoreValue.text = PrefsMgr.Instance.GetInt(PrefsMgr.strStageScore + MyGlobals.StageNum, 0).ToString();
        EventListener.Broadcast("OnStageChanged");

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();

        m_fRotDegreeTarget -= 30f;
        --m_iIndexFocusedLabel;
        if (m_iIndexFocusedLabel < 0)
            m_iIndexFocusedLabel = m_iTotalLabelCount - 1;

        SetStageNumber(MyGlobals.StageNum);

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();

        StopCoroutine("CoroutineRotateRoulette");
        StartCoroutine("CoroutineRotateRoulette");
    }

    IEnumerator CoroutineRotateRoulette()
    {
        float fElased = 0.0f;
        Vector3 vCurRot = m_transRoulette.localRotation.eulerAngles;
        Vector3 vTargetRot = new Vector3(0f, 0f, m_fRotDegreeTarget);

        Quaternion qStart = Quaternion.Euler(vCurRot);
        Quaternion qTarget = Quaternion.Euler(vTargetRot);

        float fT;
        while (fElased < m_fEachRotDuration)
        {
            fElased += Time.deltaTime;

            fT = fElased / m_fEachRotDuration;
            m_transRoulette.rotation = Quaternion.Lerp(qStart, qTarget, fT);
            m_fRotDegreeCur = Mathf.Lerp(vCurRot.z, m_fRotDegreeTarget, fT);
            yield return null;
        }

        m_fRotDegreeCur = m_fRotDegreeTarget;
        m_transRoulette.rotation = qTarget;
    }

    int GetNextIndex(int iCurIndex)
    {
        if (iCurIndex < m_arrLabelStageNum.Length - 1)
            return ++iCurIndex;
        else
            return 0;
    }

    int GetPrevIndex(int iCurIndex)
    {
        if (iCurIndex > 0)
            return --iCurIndex;
        else
            return m_arrLabelStageNum.Length - 1;
    }

    //public void OnSwipe(int iDeltaLevel)
    //{
    //    if (MyGlobals.StageNum + iDeltaLevel > MyGlobals.UserState.MaxSelectableLv)
    //        iDeltaLevel = MyGlobals.UserState.MaxSelectableLv - MyGlobals.StageNum;

    //    MyGlobals.StageNum = Mathf.Clamp(MyGlobals.StageNum + iDeltaLevel, 1, MyGlobals.UserState.MaxSelectableLv);

    //    m_fRotDegreeTarget += (30f * iDeltaLevel);

    //    StopCoroutine("CoroutineSwipeRoulette");
    //    StartCoroutine("CoroutineSwipeRoulette", iDeltaLevel);
    //}

    //float m_fCurRot = 0.0f;
    //IEnumerator CoroutineSwipeRoulette(int iDeltaLevel)
    //{
    //    float fElased = 0.0f;
    //    Vector3 vCurRot = m_transRoulette.localRotation.eulerAngles;
    //    Vector3 vTargetRot = new Vector3(0f, 0f, m_fRotDegreeTarget);
    //    float fRotDelta = vTargetRot.z - m_fCurRot;
    //    float fTime = Mathf.Abs((float)iDeltaLevel / 5f);
    //    float fAngleEachFrame = fRotDelta / (fTime / Time.fixedDeltaTime);

    //    int iPrevArea = Mathf.RoundToInt(m_fCurRot / 30f);
    //    int iCurArea = iPrevArea;
    //    while (fElased < fTime)
    //    {
    //        fElased += Time.deltaTime;

    //        m_transRoulette.Rotate(Vector3.forward, fAngleEachFrame, Space.Self);
    //        m_fCurRot += fAngleEachFrame;

    //        iCurArea = Mathf.RoundToInt(m_fCurRot / 30f);
    //        if (iPrevArea != iCurArea)
    //        {
    //            MyUtility.DebugLog("RefocusArea : " + iCurArea);
    //            iPrevArea = iCurArea;
    //            ReFocus(fAngleEachFrame > 0, iCurArea);
    //        }

    //        yield return new WaitForFixedUpdate();
    //    }

    //    m_fCurRot = m_fRotDegreeTarget;
    //    m_transRoulette.localRotation = Quaternion.AngleAxis(m_fRotDegreeTarget, Vector3.forward);
    //}

    //void ReFocus(bool bHighWay, int iCurArea)
    //{
    //    m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();

    //    if (bHighWay)
    //        ++m_iIndexFocusedLabel;
    //    else
    //        --m_iIndexFocusedLabel;

    //    if (m_iIndexFocusedLabel >= m_iTotalLabelCount)
    //        m_iIndexFocusedLabel -= m_iTotalLabelCount;
    //    else if (m_iIndexFocusedLabel < 0)
    //        m_iIndexFocusedLabel = m_iTotalLabelCount + m_iIndexFocusedLabel;

    //    m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();

    //    SetStageNumber(iCurArea + 1);
    //}

    public void OnSwipe(int iDeltaLevel)
    {
        if (MyGlobals.StageNum + iDeltaLevel > MyGlobals.UserState.MaxSelectableLv)
            iDeltaLevel = MyGlobals.UserState.MaxSelectableLv - MyGlobals.StageNum;

        MyGlobals.StageNum = Mathf.Clamp(MyGlobals.StageNum + iDeltaLevel, 1, MyGlobals.UserState.MaxSelectableLv);
        //m_labelBestScoreValue.text = PrefsMgr.Instance.GetInt(PrefsMgr.strStageScore + MyGlobals.StageNum, 0).ToString();
        EventListener.Broadcast("OnStageChanged");

        m_fRotDegreeTarget = 30f * (MyGlobals.StageNum - 1);

        StopCoroutine("CoroutineSwipeRoulette");
        StartCoroutine("CoroutineSwipeRoulette", iDeltaLevel);
    }

    IEnumerator CoroutineSwipeRoulette(int iDeltaLevel)
    {
        float fElased = 0.0f;
        //m_fRotDegreeCur = m_transRoulette.localRotation.eulerAngles.z;
        float fRotDelta = m_fRotDegreeTarget - m_fRotDegreeCur;
        float fTime = Mathf.Abs(fRotDelta / 180f);
        float fRotAngleEachFrame = fRotDelta / (fTime / Time.fixedDeltaTime);

        int iPrevArea = Mathf.RoundToInt(m_fRotDegreeCur / 30f);
        int iCurArea = iPrevArea;
        while (fElased < fTime)
        {
            fElased += Time.deltaTime;

            m_transRoulette.Rotate(Vector3.forward, fRotAngleEachFrame, Space.Self);
            m_fRotDegreeCur += fRotAngleEachFrame;

            iCurArea = Mathf.RoundToInt(m_fRotDegreeCur / 30f);
            if (iPrevArea != iCurArea)
            {
                MyUtility.DebugLog("RefocusArea : " + iCurArea);
                iPrevArea = iCurArea;
                ReFocus(fRotAngleEachFrame > 0, iCurArea);
            }

            yield return new WaitForFixedUpdate();
        }

        m_fRotDegreeCur = m_fRotDegreeTarget;
        m_transRoulette.localRotation = Quaternion.AngleAxis(m_fRotDegreeTarget, Vector3.forward);
    }

    void ReFocus(bool bHighWay, int iCurArea)
    {
        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();

        MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.BUTTON);

        if (bHighWay)
            ++m_iIndexFocusedLabel;
        else
            --m_iIndexFocusedLabel;

        if (m_iIndexFocusedLabel >= m_iTotalLabelCount)
            m_iIndexFocusedLabel -= m_iTotalLabelCount;
        else if (m_iIndexFocusedLabel < 0)
            m_iIndexFocusedLabel = m_iTotalLabelCount + m_iIndexFocusedLabel;

        SetStageNumber(iCurArea + 1);

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();
    }
}
