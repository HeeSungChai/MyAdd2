using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserState : MonoBehaviour
{
    public int m_iCoinAmount;
    public eGRADE m_eTitle;
    public eCHARACTER m_eCurCharacter;
    public int m_iLvAdd;
    public int m_iLvSub;
    public int m_iLvMul;
    public int m_iLvDiv;
    public int MaxSelectableLv;
    public int MaxStageLv;
    //int m_iSelectedStageLv;

    private void Awake()
    {
        MyGlobals.UserState = this;
    }

    void EnterAdventure()
    {
        MyGlobals.GameType = INGAME_TYPE.ADVENTURE;
        MyGlobals.StageNum = 1;

        MyGlobals.EnterIngameFromOutgame = true;
        MyGlobals.EnteringIngame = true;

        PlayerPrefs.SetInt("Chosen_Character", (int)m_eCurCharacter);
        //switch(m_eCurCharacter)
        //{
        //    case eCHARACTER.ADD:
                PlayerPrefs.SetInt("Chosen_CharacterLV", m_iLvAdd);
        //}
    }

    public int GetCurSkillLv()
    {
        int CurLevel;
        switch(m_eCurCharacter)
        {
            case eCHARACTER.ADD:
                CurLevel = m_iLvAdd;
                break;
            case eCHARACTER.SUB:
                CurLevel = m_iLvSub;
                break;
            case eCHARACTER.MUL:
                CurLevel = m_iLvMul;
                break;
            case eCHARACTER.DIV:
                CurLevel = m_iLvDiv;
                break;
            default:
                CurLevel = 1;
                break;
        }

        return CurLevel;
    }
}
