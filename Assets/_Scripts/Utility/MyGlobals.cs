using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum INGAME_TYPE
{
    ADVENTURE = 0,
    INFINITE,
    TEST
}

//public enum eLANGUAGE
//{
//    KOREAN,
//    ENGLISH
//}
//public enum STAGE_NUM
//{    
//}

public class MyGlobals
{
    //private static StageMgr m_StageMgr;
    public static StageMgr StageMgr {get;set; }
    //public static BGMManager BGMMgr { get; set; }
    public static DigitSpawner DigitSpawner { get; set; }
    public static HpBarMgr HpBarMgr { get; set; }
    public static InputCtrl InputCtrl { get; set; }
    public static ScoreMgr ScoreMgr { get; set; }
    public static UserState UserState { get; set; }
    public static SoundMgr SoundMgr { get; set; }
    public static CharacterMgr CharacterMgr { get; set; }
    //public static LanguageMgr LanguageMgr { get; set; }

    //public static eLANGUAGE Language { get; set; }
    //public static eKEY_TABLEDB LanguageKey { get; set; }
    public static INGAME_TYPE GameType { get; set; }
    public static int StageNum { get; set; }
    public static int MaxSelectableLv { get; set; }
    public static int MaxStage = 50;
    public static bool EnterIngameFromOutgame { get; set; }
    public static bool EnteringIngame { get; set; }
    public static bool EnterIngameFromTestMode { get; set; }
    public static int MaxValue = 81;
    public static int MaxInputValue = 9;

    public static void GoBackToLobby()
    {
        SceneManager.LoadScene("Lobby");
        Release();
    }

    public static void Release()
    {
        Resources.UnloadUnusedAssets();
    }    
}
