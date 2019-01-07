using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserState : MonoBehaviour
{
    public int m_iCoinAmount;
    public eGRADE m_eTitle;
    public eCHARACTER m_eCurCharacter;
    public int m_iLvAdd;
    public int m_iLvMi;
    public int m_iLvDoubleRobo;
    public int m_iLvDividivi;
    public int m_iMaxSelectableLv;
    int m_iSelectedStageLv;

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
}
