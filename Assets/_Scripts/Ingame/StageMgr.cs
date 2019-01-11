using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;

public enum STAGE_STATE
{
    READY = 0,
    PLAYING,
    EVENT,
    GAMEOVER,
    GAMECLEAR,
}

public enum eINPUT_TYPE
{
    FROM_ONE_TO_NINE,
    FROM_ONE_TO_EIGHTYONE,
}

public partial class StageMgr : MonoBehaviour
{
    private bool m_bIsTest = false;

    ////stage state
    [SerializeField]
    private STAGE_STATE m_eStageState;
    public STAGE_STATE StageState
    {
        get { return m_eStageState; }
        set { m_eStageState = value; }
    }

    [SerializeField]
    private INGAME_TYPE m_eGameType;
    public INGAME_TYPE GameType
    {
        get { return m_eGameType; }
        set { m_eGameType = value; }
    }

    [SerializeField]
    private int m_iStageNum;
    public int StageNum
    {
        get { return m_iStageNum; }
        set { m_iStageNum = value; }
    }

    public eINPUT_TYPE m_eInputType;

    public float PlayTime { get; set; }
    private bool m_bPauseDrop = false;
    public bool IsPauseDrop { get { return m_bPauseDrop; } set { m_bPauseDrop = value; } }

    [Header("Character Info")]
    public eCHARACTER m_eCharacter;
    public int m_iCharacterLv = 1;

    int TotalScore { get; set; }
    int EachCorrectScore { get; set; }
    int TotalBasicScore { get; set; }
    int EachCharBonusScore { get; set; }
    int TotalCharBonusScore { get; set; }
    int EachComboBonusScore { get; set; }
    int TotalComboBonusScore { get; set; }

    private void Awake()
    {
        MyGlobals.StageMgr = this;

        if (MyGlobals.EnterIngameFromOutgame)
        {
            GameType = MyGlobals.GameType;
            if (GameType == INGAME_TYPE.ADVENTURE)
            {
                StageNum = MyGlobals.StageNum;
            }
        }
        else
        {
            m_bIsTest = true;
        }

        EventListener.AddListener("OnCountdownDone", this);
        EventListener.AddListener("OnDialogueEnd", this);
        EventListener.AddListener("OnDivergeSelected", this);
        EventListener.AddListener("OnGameClear", this);
        EventListener.AddListener("OnGameOver", this);
        EventListener.AddListener("OnTimePaused", this);
        EventListener.AddListener("OnCorrectAnswer", this);

#if UNITY_ANDROID || UNITY_IPHONE
        //Application.targetFrameRate = 60;
#endif
    }

    void Start ()
    {
        MyGlobals.EnteringIngame = false;

        if (GameType == INGAME_TYPE.ADVENTURE)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.Append("CoroutineStageEvent_");
            //sb.Append(MyUtility.ConvertToString(StageNum));
            //StartCoroutine(sb.ToString());
            StartCoroutine("CoroutineCheckPlayTime");
            SetScoreValue();
        }
        else
        {
        }
    }

    void SetScoreValue()
    {
        EachCorrectScore = 100;
        if (MyGlobals.EnterIngameFromOutgame)
        {
            m_eCharacter = eCHARACTER.ADD;
            m_iCharacterLv = PlayerPrefs.GetInt("Chosen_CharacterLV");
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

    public void OnGameClear()
    {
        StageState = STAGE_STATE.GAMECLEAR;
        IsPauseDrop = true;
    }

    public void OnGameOver(bool bContinuePossible = true)
    {
        StageState = STAGE_STATE.GAMEOVER;
        IsPauseDrop = true;

        if(bContinuePossible)
            Invoke("TimeScaleZero", 3.0f);
    }

    void OnCountdownDone()
    {
        if (GameType == INGAME_TYPE.ADVENTURE)
        {
            StageState = STAGE_STATE.PLAYING;
            IsPauseDrop = false;
            //MyGlobals.BGMMgr.ReplayBGM();
        }
        else
        {
            StageState = STAGE_STATE.PLAYING;
            IsPauseDrop = false;
            //IngameUICtrl.instance.SetLevelInfo(CurLevel);
        }
    }

    public void ContinueGame()
    {
        //MyGlobals.BGMMgr.StopBGM();

        //게임오버 팝업 삭제
        //IngameUICtrl.instance.ActivateUI(eINGAME_UI.GAME_OVER, false);

        StageState = STAGE_STATE.READY;

        EventListener.Broadcast("OnRestart");
        EventListener.Broadcast("OnStartCountDown");

        TimeScale(1.0f);
    }

    void TimeScale(float fScale)
    {
        Time.timeScale = fScale;
    }

    IEnumerator CoroutineCheckGameStart()
    {
        float fTime = 0.0f;

        while (StageState < STAGE_STATE.PLAYING)
        {
            fTime += Time.deltaTime;

            if (fTime > 2.0f)
            {
                PlayTime = 0.0f;
                StageState = STAGE_STATE.PLAYING;
                break;
            }

            yield return null;
        }

        StageState = STAGE_STATE.PLAYING;

        EventListener.Broadcast("OnGameStart");
    }

    IEnumerator CoroutineCheckPlayTime()
    {
        yield return StartCoroutine("CoroutineCheckGameStart");

        while (StageState < STAGE_STATE.GAMECLEAR)
        {
            if (IsPauseDrop)
            {
                yield return null;
                continue;
            }

            PlayTime += Time.deltaTime;

            yield return null;
        }
    }

    void OnTimePaused()
    {
        StopCoroutine("CoroutineTimePause");
        StartCoroutine("CoroutineTimePause");
    }

    IEnumerator CoroutineTimePause()
    {
        IsPauseDrop = true;

        yield return new WaitForSeconds(3.0f);

        IsPauseDrop = false;
    }

    void OnCorrectAnswer()
    {
        //기본점수
        TotalBasicScore += EachCorrectScore;

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

        //콤보 점수

        //총 획득 점수
        TotalScore = TotalBasicScore + TotalCharBonusScore + TotalComboBonusScore;

        MyGlobals.ScoreMgr.UpdateScore(TotalScore);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
        Resources.UnloadUnusedAssets();
    }
}
