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
    private STAGE_NUM m_eStageNum;
    public STAGE_NUM StageNum
    {
        get { return m_eStageNum; }
        set { m_eStageNum = value; }
    }

    public eINPUT_TYPE m_eInputType;

    public float PlayTime { get; set; }
    private bool m_bPauseDrop = false;
    public bool IsPauseDrop { get { return m_bPauseDrop; } set { m_bPauseDrop = value; } }

    private void Awake()
    {
        MyGlobals.StageMgr = this;

        if (MyGlobals.EnterIngameFromOutgame == false)
        {
            m_bIsTest = true;
        }
        else
        {
            GameType = MyGlobals.GameType;
            if (GameType == INGAME_TYPE.ADVENTURE)
            {
                StageNum = MyGlobals.StageNum;
            }
        }

        EventListener.AddListener("OnCountdownDone", this);
        EventListener.AddListener("OnDialogueEnd", this);
        EventListener.AddListener("OnDivergeSelected", this);
        EventListener.AddListener("OnGameClear", this);
        EventListener.AddListener("OnGameOver", this);
        EventListener.AddListener("OnTimePaused", this);

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
        }
        else
        {
        }
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

            yield return new WaitForEndOfFrame();
        }

        StageState = STAGE_STATE.PLAYING;

        EventListener.Broadcast("OnGameStart");
    }

    IEnumerator CoroutineCheckPlayTime()
    {
        yield return StartCoroutine("CoroutineCheckGameStart");

        while (StageState < STAGE_STATE.GAMECLEAR)
        {
            PlayTime += Time.deltaTime;

            if (IsPauseDrop)
            {
                yield return null;
                continue;
            }

            yield return new WaitForEndOfFrame();
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

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
        Resources.UnloadUnusedAssets();
    }
}
