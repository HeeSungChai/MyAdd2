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
    int m_iSelectedStageLv;
    int m_iMaxSelectableLv;

    private void Awake()
    {
        m_fRotDegreeTarget = 0f;
        m_iIndexFocusedLabel = 0;
        m_iTotalLabelCount = m_arrLabelStageNum.Length;
        m_iMaxSelectableLv = m_scriptUserState.m_iMaxSelectableLv;
    }

    void OnEnable ()
    {
    }

    public void OnPressed_HigherLevel()
    {
        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();
        
        m_fRotDegreeTarget += 30f;
        ++m_iIndexFocusedLabel;
        if (m_iIndexFocusedLabel >= m_iTotalLabelCount)
            m_iIndexFocusedLabel = 0;

        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOn();

        StopCoroutine("CoroutineRotateRoulette");
        StartCoroutine("CoroutineRotateRoulette");
    }

    public void OnPressed_LowerLevel()
    {
        m_arrLabelStageNum[m_iIndexFocusedLabel].FocusOff();

        m_fRotDegreeTarget -= 30f;
        --m_iIndexFocusedLabel;
        if (m_iIndexFocusedLabel < 0)
            m_iIndexFocusedLabel = m_iTotalLabelCount - 1;

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

        Quaternion.Lerp(qStart, qTarget, fElased / m_fEachRotDuration);

        while (fElased < m_fEachRotDuration)
        {
            fElased += Time.deltaTime;

            m_transRoulette.rotation = Quaternion.Lerp(qStart, qTarget, fElased / m_fEachRotDuration);

            yield return null;
        }
    }
}
