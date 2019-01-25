using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMgr : MonoBehaviour
{
    public UILabel m_labelScore;
    int m_iCurScore;
    int m_iStartDigit;
    int m_iTargetDigit;
    public UILabel m_labelScore_Combo;
    int m_iCurScore_Combo;
    int m_iStartDigit_Combo;
    int m_iTargetDigit_Combo;
    public float m_fDuration = 0.5f;

    public eCHARACTER m_eCharacter;
    public int m_iCharacterLv = 1;

    public int TotalScore { get; set; }
    public int EachGreatScore = 300;
    public int EachCoolScore = 120;
    public int EachNiceScore = 50;
    public int TotalBasicScore;
    public int EachCharBonusScore;
    public int TotalCharBonusScore;
    public int EachComboBonusScore = 10;
    public int TotalComboBonusScore;
    public UILabel m_labelComboCount;

    void Start ()
    {
        MyGlobals.ScoreMgr = this;
        m_iCurScore = 0;
        SetScoreValue();

        if (MyGlobals.StageMgr.IsAdventure())
        {
            m_labelScore_Combo.parent.gameObject.SetActive(false);
            if (m_labelComboCount)
                m_labelComboCount.gameObject.SetActive(false);
        }
        else
        {
#if DEBUG
            if (m_labelComboCount)
            {
                m_labelComboCount.gameObject.SetActive(true);
                m_labelComboCount.text = "Combo : ";
            }
#endif
        }
    }

    void SetScoreValue()
    {
        m_eCharacter = MyGlobals.StageMgr.m_eCharacter;
        m_iCharacterLv = PrefsMgr.Instance.GetCharacterLevel(m_eCharacter);

        eTABLE_LIST eTable;
        switch (m_eCharacter)
        {
            case eCHARACTER.ADD:
                eTable = eTABLE_LIST.CHAR_LEVEL_ADD;
                break;
            case eCHARACTER.SUB:
                eTable = eTABLE_LIST.CHAR_LEVEL_SUB;
                break;
            case eCHARACTER.MUL:
                eTable = eTABLE_LIST.CHAR_LEVEL_MUL;
                break;
            case eCHARACTER.DIV:
                eTable = eTABLE_LIST.CHAR_LEVEL_DIV;
                break;
            default:
                eTable = eTABLE_LIST.CHAR_LEVEL_ADD;
                break;
        }
        EachCharBonusScore = ((int)TableDB.Instance.GetData(eTable, m_iCharacterLv, eKEY_TABLEDB.i_SKILL_VALUE));
    }

    public void UpdateScore(eEVALUATION eEvaluation, bool bByItem, bool bBySuperSkill = false)
    {
        //기본점수
        switch (eEvaluation)
        {
            case eEVALUATION.GREAT:
                TotalBasicScore += EachGreatScore;
                break;
            case eEVALUATION.COOL:
                TotalBasicScore += EachCoolScore;
                break;
            case eEVALUATION.NICE:
                TotalBasicScore += EachNiceScore;
                break;
            default:
                TotalBasicScore += EachNiceScore;
                break;
        }

        if (!bByItem)
        {
            //캐릭터 보너스 점수
            switch (MyGlobals.InputCtrl.m_eSelected_Operator)
            {
                case eOPERATOR.ADDITION:
                    if (m_eCharacter == eCHARACTER.ADD)
                    {
                        TotalCharBonusScore += EachCharBonusScore;
                        if(!bBySuperSkill)
                            EventListener.Broadcast("OnBonusAchieved");
                    }
                    break;
                case eOPERATOR.SUBTRACTION:
                    if (m_eCharacter == eCHARACTER.SUB)
                    {
                        TotalCharBonusScore += EachCharBonusScore;
                        if (!bBySuperSkill)
                            EventListener.Broadcast("OnBonusAchieved");
                    }
                    break;
                case eOPERATOR.MULTIPLICATION:
                    if (m_eCharacter == eCHARACTER.MUL)
                    {
                        TotalCharBonusScore += EachCharBonusScore;
                        if (!bBySuperSkill)
                            EventListener.Broadcast("OnBonusAchieved");
                    }
                    break;
                case eOPERATOR.DIVISION:
                    if (m_eCharacter == eCHARACTER.DIV)
                    {
                        TotalCharBonusScore += EachCharBonusScore;
                        if (!bBySuperSkill)
                            EventListener.Broadcast("OnBonusAchieved");
                    }
                    break;
                default:
                    break;
            }
        }

        //콤보 점수
        if (!MyGlobals.StageMgr.IsAdventure())
        {
            TotalComboBonusScore += MyGlobals.StageMgr.ComboCount * EachComboBonusScore;
            UpdateScore_Combo(TotalComboBonusScore);
        }

        //총 획득 점수
        TotalScore = TotalBasicScore + TotalCharBonusScore + TotalComboBonusScore;

        UpdateScore(TotalScore);


    }

    public void UpdateScore (int iScore)
    {
        m_iStartDigit = m_iCurScore;
        m_iTargetDigit = iScore;

        StopCoroutine("CoroutineCount");
        StartCoroutine("CoroutineCount");

        m_iCurScore = iScore;
        //m_labelScore.text = MyUtility.CommaSeparateDigit(iScore);
    }

    IEnumerator CoroutineCount()
    {
        float fElased = 0f;
        while (fElased < m_fDuration)
        {
            fElased += Time.deltaTime;

            m_labelScore.text = MyUtility.CommaSeparateDigit(((int)(Mathf.Lerp(m_iStartDigit, m_iTargetDigit, fElased / m_fDuration)))).ToString();

            yield return null;
        }

        m_labelScore.text = MyUtility.CommaSeparateDigit(m_iTargetDigit).ToString();
    }

    public void UpdateScore_Combo(int iScore)
    {
#if DEBUG
        if(m_labelComboCount)
            m_labelComboCount.text = "Combo : " + MyGlobals.StageMgr.ComboCount.ToString();
#endif

        m_iStartDigit_Combo = m_iCurScore_Combo;
        m_iTargetDigit_Combo = iScore;

        StopCoroutine("CoroutineCount_Combo");
        StartCoroutine("CoroutineCount_Combo");

        m_iCurScore_Combo = iScore;
    }

    IEnumerator CoroutineCount_Combo()
    {
        float fElased = 0f;
        while (fElased < m_fDuration)
        {
            fElased += Time.deltaTime;

            m_labelScore_Combo.text = MyUtility.CommaSeparateDigit(((int)(Mathf.Lerp(m_iStartDigit_Combo, m_iTargetDigit_Combo, fElased / m_fDuration)))).ToString();

            yield return null;
        }

        m_labelScore_Combo.text = MyUtility.CommaSeparateDigit(m_iTargetDigit_Combo).ToString();
    }
}
