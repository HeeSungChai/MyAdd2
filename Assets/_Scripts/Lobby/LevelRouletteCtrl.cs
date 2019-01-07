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
    float m_fRotDegreeTarget;
    //int m_iSelectedStageLv;
    //int m_iMaxSelectableLv;

    private void Awake()
    {
        m_fRotDegreeTarget = 0f;
        m_iIndexFocusedLabel = 0;
        //m_iSelectedStageLv = 1;
        MyGlobals.StageNum = 1;
        m_iTotalLabelCount = m_arrLabelStageNum.Length;
        //m_iMaxSelectableLv = MyGlobals.UserState.MaxSelectableLv;
    }

    void OnEnable()
    {
        //SetStageNumber();
    }

    private void Start()
    {
        SetStageNumber();
        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();
    }

    void SetStageNumber()
    {
        int iIndexPrev = GetPrevIndex(m_iIndexFocusedLabel);
        int iIndexPrevPrev = GetPrevIndex(iIndexPrev);
        int iIndexNext = GetNextIndex(m_iIndexFocusedLabel);
        int iIndexNextNext = GetNextIndex(iIndexNext);

        m_arrLabelStageNum[iIndexPrevPrev].SetStageNum(MyGlobals.StageNum - 2);
        m_arrLabelStageNum[iIndexPrev].SetStageNum(MyGlobals.StageNum - 1);
        m_arrLabelStageNum[m_iIndexFocusedLabel].SetStageNum(MyGlobals.StageNum);
        m_arrLabelStageNum[iIndexNext].SetStageNum(MyGlobals.StageNum + 1);
        m_arrLabelStageNum[iIndexNextNext].SetStageNum(MyGlobals.StageNum + 2);
    }

    public void OnPressed_HigherLevel()
    {
        if (MyGlobals.StageNum >= MyGlobals.UserState.MaxSelectableLv)
            return;

        ++MyGlobals.StageNum;

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();
        
        m_fRotDegreeTarget += 30f;
        ++m_iIndexFocusedLabel;
        if (m_iIndexFocusedLabel >= m_iTotalLabelCount)
            m_iIndexFocusedLabel = 0;

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();

        SetStageNumber();

        StopCoroutine("CoroutineRotateRoulette");
        StartCoroutine("CoroutineRotateRoulette");
    }

    public void OnPressed_LowerLevel()
    {
        if (MyGlobals.StageNum <= 1)
            return;

        --MyGlobals.StageNum;

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();

        m_fRotDegreeTarget -= 30f;
        --m_iIndexFocusedLabel;
        if (m_iIndexFocusedLabel < 0)
            m_iIndexFocusedLabel = m_iTotalLabelCount - 1;

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();

        SetStageNumber();

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

        Quaternion.Lerp(qStart, qTarget, fElased / m_fEachRotDuration);

        while (fElased < m_fEachRotDuration)
        {
            fElased += Time.deltaTime;

            m_transRoulette.rotation = Quaternion.Lerp(qStart, qTarget, fElased / m_fEachRotDuration);

            yield return null;
        }
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
}
