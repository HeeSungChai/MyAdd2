using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMgr : MonoBehaviour
{
    public UILabel m_labelScore;
    int m_iCurScore;
    public int m_iStartDigit;
    public int m_iTargetDigit;
    public float m_fDuration;

    public eCHARACTER m_eCharacter;
    public int m_iCharacterLv = 1;

    public int TotalScore { get; set; }
    public int EachGreatScore = 300;
    public int EachCoolScore = 120;
    public int EachNiceScore = 50;
    public int TotalBasicScore { get; set; }
    int EachCharBonusScore;
    public int TotalCharBonusScore { get; set; }
    int EachComboBonusScore;
    public int TotalComboBonusScore { get; set; }

    void Start ()
    {
        MyGlobals.ScoreMgr = this;
        m_iCurScore = 0;
        SetScoreValue();
    }

    void SetScoreValue()
    {
        if (MyGlobals.EnterIngameFromOutgame)
        {
            m_eCharacter = MyGlobals.CharacterMgr.CurCharacter;
            m_iCharacterLv = PrefsMgr.Instance.GetInt(PrefsMgr.strChoosenCharLv);
        }

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

    public void UpdateScore(eEVALUATION eEvaluation, bool bByItem)
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
                    {
                        if (m_eCharacter == eCHARACTER.ADD)
                            TotalCharBonusScore += EachCharBonusScore;
                    }
                    break;
                case eOPERATOR.SUBTRACTION:
                    {
                        if (m_eCharacter == eCHARACTER.SUB)
                            TotalCharBonusScore += EachCharBonusScore;
                    }
                    break;
                case eOPERATOR.MULTIPLICATION:
                    {
                        if (m_eCharacter == eCHARACTER.MUL)
                            TotalCharBonusScore += EachCharBonusScore;
                    }
                    break;
                case eOPERATOR.DIVISION:
                    {
                        if (m_eCharacter == eCHARACTER.DIV)
                            TotalCharBonusScore += EachCharBonusScore;
                    }
                    break;
                default:
                    break;
            }
        }

        //콤보 점수


        //총 획득 점수
        TotalScore = TotalBasicScore + TotalCharBonusScore + TotalComboBonusScore;

        UpdateScore(TotalScore);
    }

    public void UpdateScore (int iScore)
    {
        m_iStartDigit = m_iCurScore;
        m_iTargetDigit = iScore;
        m_fDuration = 0.5f;

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
}
