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
    public bool IsDeactiveSkill = false;

    [Header("Character Info")]
    public eCHARACTER m_eCharacter;
    public int m_iCharacterLv = 1;

    [Header("Result")]
    public GameObject m_objGameClear;
    public GameObject m_objGameOver;

    private void Awake()
    {
        MyGlobals.StageMgr = this;

        if (MyGlobals.EnterIngameFromOutgame)
        {
            GameType = MyGlobals.GameType;
            if (GameType == INGAME_TYPE.ADVENTURE)
            {
                if (MyGlobals.StageNum == 0)
                    MyGlobals.StageNum = 1;
                StageNum = MyGlobals.StageNum;
            }
        }
        else
        {
            m_bIsTest = true;
            MyGlobals.StageNum = StageNum;
        }

        EventListener.AddListener("OnCountdownDone", this);
        EventListener.AddListener("OnDialogueEnd", this);
        EventListener.AddListener("OnDivergeSelected", this);
        EventListener.AddListener("OnGameClear", this);
        EventListener.AddListener("OnGameOver", this);
        EventListener.AddListener("OnTimePaused", this);
        EventListener.AddListener("OnCorrectAnswer", this);
        EventListener.AddListener("OnActivateSuperSkill", this);
        EventListener.AddListener("OnDeactivateSuperSkill", this);

#if UNITY_ANDROID || UNITY_IPHONE
        //Application.targetFrameRate = 60;
#endif
    }

    void Start ()
    {
        MyGlobals.EnteringIngame = false;

        if (GameType == INGAME_TYPE.ADVENTURE)
        {
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
        m_objGameClear.SetActive(true);
    }

    public void OnGameOver(bool bContinuePossible = true)
    {
        StageState = STAGE_STATE.GAMEOVER;
        IsPauseDrop = true;

        m_objGameOver.SetActive(true);
    }

    void OnCountdownDone()
    {
        if (GameType == INGAME_TYPE.ADVENTURE)
        {
            StageState = STAGE_STATE.PLAYING;
            IsPauseDrop = false;
        }
        else
        {
            StageState = STAGE_STATE.PLAYING;
            IsPauseDrop = false;
        }
    }

    public void ContinueGame()
    {
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

    bool bClockItemUsing = false;
    IEnumerator CoroutineTimePause()
    {
        IsPauseDrop = true;
        float fDuration = (float)TableDB.Instance.GetData(eTABLE_LIST.ITEM_ID,
                                        (int)eITEM_ID.CLOCK, eKEY_TABLEDB.f_ACTIVATE_DURARION);

        bClockItemUsing = true;
        yield return new WaitForSeconds(fDuration);
        bClockItemUsing = false;

        if (bSuperSkillActivating == false)
            IsPauseDrop = false;
    }

    bool bSuperSkillActivating = false;
    public void OnActivateSuperSkill()
    {
        bSuperSkillActivating = true;
        IsPauseDrop = true;
    }

    public void OnDeactivateSuperSkill()
    {
        bSuperSkillActivating = false;
        if(bClockItemUsing == false)
            IsPauseDrop = false;
    }

    public void OnGoToNextStage()
    {
        ++MyGlobals.StageNum;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
        Resources.UnloadUnusedAssets();
    }
}
