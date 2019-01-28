using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsMgr
{
    [Header("Character Info")]
    public static string strTitle = "UserTitle";
    public static string strChoosenChar = "Chosen_Character";
    //public static string strChoosenCharLv = "Chosen_CharacterLV";
    public static string strCharLv = "CharacterLV_";
    public static string strCharacterOpen = "CharacterOpen_";
    public static string strOperatorOpen = "OperatorOpen_";

    [Header("Stage")]
    public static string strMaxSelectableLv = "MaxSelectableLv";

    [Header("Score")]
    public static string strStageScore = "Score_Stage_";
    public static string strTotalScore = "TotalScore";
    public static string strBestComboScore = "BestComboScore";
    public static string strInfiniteModeBestScore = "InfiniteModeBestScore";

    [Header("Option")]
    public static string strVolumnFX = "VolumnFx";
    public static string strVolumnBGM = "VolumnBGM";
    public static string strLanguage = "Language";

    [Header("Inventory")]
    public static string strCoinCount = "CoinCount";
    public static string strItemEraserCount = "ItemEraserCount";
    public static string strItemClockCount = "ItemClockCount";
    public static string strItemRecoveryCount = "ItemRecoveryCount";


    #region Singleton Pattern Implementation
    private static PrefsMgr instance;

    public static PrefsMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new PrefsMgr();
            return instance;
        }
    }
    #endregion

    public void SetInt(string key, int iValue)
    {
        PlayerPrefs.SetInt(key, iValue);
    }

    public int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    public int GetInt(string key, int iDefault)
    {
        return PlayerPrefs.GetInt(key, iDefault);
    }

    public void SetFloat(string key, float fValue)
    {
        PlayerPrefs.SetFloat(key, fValue);
    }

    public float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, 0f);
    }

    public float GetFloat(string key, float fDefault)
    {
        return PlayerPrefs.GetFloat(key, fDefault);
    }

    public void InitializeAllState()
    {
        PlayerPrefs.DeleteAll();
    }

    public int GetTotalPoint()
    {
        int iTotalPoint = 0;
        for (int i = 1; i <= 50; ++i)
        {
            iTotalPoint += GetInt(strStageScore + i.ToString(), 0);
        }
        return iTotalPoint;
    }

    public void SetStageScore(int iScore)
    {
        SetInt(strStageScore + MyGlobals.StageMgr.StageNum, iScore);
    }

    public int GetStageScore(int iStageNum)
    {
        return GetInt(strStageScore + iStageNum, 0);
    }

    public void SetCharacterOpen(eCHARACTER eCharacter)
    {
        SetInt(strCharacterOpen + eCharacter.ToString(), 1);
    }

    public bool GetCharacterOpen(eCHARACTER eCharacter)
    {
        return GetInt(strCharacterOpen + eCharacter.ToString(), 0) == 1 ? true : false;
    }

    public bool GetOperatorOpen(eOPERATOR eOperator)
    {
        if (MyGlobals.EnterIngameFromOutgame)
            return MyGlobals.StageMgr.m_eMaxOperator >= eOperator ? true : false;
        else
        {
            if (MyGlobals.StageMgr.IsAdventure())
            {
                switch (eOperator)
                {
                    case eOPERATOR.ADDITION:
                        return true;
                    case eOPERATOR.SUBTRACTION:
                        return GetCharacterOpen(eCHARACTER.SUB);
                    case eOPERATOR.MULTIPLICATION:
                        return GetCharacterOpen(eCHARACTER.MUL);
                    case eOPERATOR.DIVISION:
                        return GetCharacterOpen(eCHARACTER.DIV);
                    default:
                        return false;
                }
            }
            else
                return true;
        }
    }

    public void SetChoosenCharacter(eCHARACTER eCharacter)
    {
        SetInt(strChoosenChar, (int)eCharacter);
    }

    public eCHARACTER GetChoosenCharacter()
    {
        return (eCHARACTER)GetInt(strChoosenChar, 200);
    }

    public void SetCharacterLevel(eCHARACTER eCharacter, int iLevel)
    {
        iLevel = Mathf.Clamp(iLevel, 1, 10);
        SetInt(strCharLv + eCharacter.ToString(), iLevel);
    }

    public int GetCharacterLevel(eCHARACTER eCharacter)
    {
        return GetInt(strCharLv + eCharacter.ToString(), 1);
    }

    public void SetMaxSelectableLv(int iNewStage)
    {
        iNewStage = Mathf.Clamp(iNewStage, 1, MyGlobals.MaxStage);
        SetInt(strMaxSelectableLv, iNewStage);
    }

    public int GetMaxSelectableLv()
    {
        return GetInt(strMaxSelectableLv, 1);
    }

    public void SetCoinAmount(int iAmount)
    {
        SetInt(strCoinCount, iAmount);
    }

    public int GetCoinAmount()
    {
        return GetInt(strCoinCount, 0);
    }

    public void ItemUsed(eITEM_TYPE eItem)
    {
        int iCurAmount;
        switch(eItem)
        {
            case eITEM_TYPE.ERASER:
                iCurAmount = GetInt(strItemEraserCount, 0);
                --iCurAmount;
                Mathf.Clamp(iCurAmount, 0, MyGlobals.ItemMaxAmount);
                SetInt(strItemEraserCount, iCurAmount);
                break;
            case eITEM_TYPE.CLOCK:
                iCurAmount = GetInt(strItemClockCount, 0);
                --iCurAmount;
                Mathf.Clamp(iCurAmount, 0, MyGlobals.ItemMaxAmount);
                SetInt(strItemClockCount, iCurAmount);
                break;
            case eITEM_TYPE.RECOVERY:
                iCurAmount = GetInt(strItemRecoveryCount, 0);
                --iCurAmount;
                Mathf.Clamp(iCurAmount, 0, MyGlobals.ItemMaxAmount);
                SetInt(strItemRecoveryCount, iCurAmount);
                break;
        }
    }

    public int GetItemAmount(eITEM_TYPE eItem)
    {
        int iCurAmount;
        switch (eItem)
        {
            case eITEM_TYPE.ERASER:
                iCurAmount = GetInt(strItemEraserCount, 0);
                break;
            case eITEM_TYPE.CLOCK:
                iCurAmount = GetInt(strItemClockCount, 0);
                break;
            case eITEM_TYPE.RECOVERY:
                iCurAmount = GetInt(strItemRecoveryCount, 0);
                break;
            default:
                iCurAmount = 0;
                break;
        }

        return iCurAmount;
    }

    public int SetTitle(eUSER_TITLE eTitle)
    {
        int iTitle = Mathf.Clamp((int)eTitle, (int)eUSER_TITLE.BEGINNER, (int)eUSER_TITLE.GOD_OF_MATH);
        return GetInt(strTitle, (int)iTitle);
    }

    public eUSER_TITLE GetTitle()
    {
        int iTitle = GetInt(strTitle, 100);
        return (eUSER_TITLE)iTitle;
    }
}
