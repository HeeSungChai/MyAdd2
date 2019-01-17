using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsMgr{

    public static string strChoosenChar = "Chosen_Character";
    public static string strChoosenCharLv = "Chosen_CharacterLV";
    public static string strStageScore = "Score_Stage_";
    public static string strTotalScore = "TotalScore";
    public static string strVolumnFX = "VolumnFx";
    public static string strVolumnBGM = "VolumnBGM";
    public static string strLanguage = "Language";

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
}
