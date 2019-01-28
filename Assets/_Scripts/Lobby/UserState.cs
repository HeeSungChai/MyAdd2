using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserState : MonoBehaviour
{
    public bool m_IsTestMode = true;
    public bool m_ResetAllData = false;
    public int m_iCoinAmount;
    public eUSER_TITLE m_eTitle;
    eCHARACTER m_eCurCharacter;
    //public eCHARACTER m_eCharacterOpenUntil;
    public int m_iLvAdd;
    public int m_iLvSub;
    public int m_iLvMul;
    public int m_iLvDiv;
    [SerializeField]
    private int m_iMaxSelectableLv;
    public int MaxSelectableLv
    {
        get { return PrefsMgr.Instance.GetMaxSelectableLv(); }
        set { PrefsMgr.Instance.SetMaxSelectableLv(value); }
    }
    public int MaxStageLv;
    //int m_iSelectedStageLv;

    private void Awake()
    {
        MyGlobals.UserState = this;

        if (m_ResetAllData)
            PrefsMgr.Instance.InitializeAllState();

        if (m_IsTestMode)
        {
            PrefsMgr.Instance.SetCharacterLevel(eCHARACTER.ADD, m_iLvAdd);
            PrefsMgr.Instance.SetCharacterLevel(eCHARACTER.SUB, m_iLvSub);
            PrefsMgr.Instance.SetCharacterLevel(eCHARACTER.MUL, m_iLvMul);
            PrefsMgr.Instance.SetCharacterLevel(eCHARACTER.DIV, m_iLvDiv);

            PrefsMgr.Instance.SetMaxSelectableLv(m_iMaxSelectableLv);

            PrefsMgr.Instance.SetCoinAmount(m_iCoinAmount);
            PrefsMgr.Instance.SetTitle(m_eTitle);
        }
        else
        {
            m_iCoinAmount = PrefsMgr.Instance.GetCoinAmount();
            m_eTitle = PrefsMgr.Instance.GetTitle();
        }
    }

    public void EnterAdventure()
    {
        //if (MyGlobals.EnteringIngame)
        //    return;

        //MyGlobals.GameType = INGAME_TYPE.ADVENTURE;

        //MyGlobals.EnterIngameFromOutgame = true;
        //MyGlobals.EnteringIngame = true;

        //EventListener.Broadcast("OnEnterIngame");
    }

    public int GetCurSkillLv()
    {
        m_eCurCharacter = PrefsMgr.Instance.GetChoosenCharacter();
        
        return PrefsMgr.Instance.GetCharacterLevel(m_eCurCharacter);
    }
}
