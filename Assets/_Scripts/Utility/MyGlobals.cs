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

public enum STAGE_NUM
{    
}

public class MyGlobals
{
    //private static StageMgr m_StageMgr;
    public static StageMgr StageMgr {get;set; }
    //public static BGMManager BGMMgr { get; set; }
    private static DigitSpawner m_DigitSpawner;
    public static DigitSpawner DigitSpawner { get; set; }

    public static INGAME_TYPE GameType { get; set; }
    public static STAGE_NUM StageNum { get; set; }
    public static bool EnterIngameFromOutgame { get; set; }
    public static bool EnteringIngame { get; set; }
    public static int MaxValue = 81;

    public static void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu");
        Release();
    }

    public static void Release()
    {
        Resources.UnloadUnusedAssets();
    }    
}
