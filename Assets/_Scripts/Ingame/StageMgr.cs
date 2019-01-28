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
    [HideInInspector]
    public bool m_bIsTest = false;

    ////stage state
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
    [HideInInspector]
    public bool IsDeactiveSkill = false;

    [Header("Character Info")]
    public eCHARACTER m_eCharacter;
    public int m_iCharacterLv = 1;

    [Header("Super Skill")]
    public float m_fBonusPointForFullGauge = 800;

    [Header("Operator condition")]
    [Tooltip("Adventure일때만 적용, Infinite일때는 무조건 나누기까지 다 개방")]
    public eOPERATOR m_eMaxOperator;

    [Header("Result")]
    public GameObject m_objGameClear;
    public GameObject m_objGameOver;

    [Header("Infinite Mode")]
    public float m_fSpawnDelayMin = 0.8f;
    public float m_fSpawnDelayMax = 3f;
    public float RemainTime = 999f;
    [HideInInspector]
    public int ComboCount = -1;
    [HideInInspector]
    public int TotalComboCount;
    public UILabel m_labelRemainTime;

    private void Awake()
    {
        MyGlobals.StageMgr = this;

        if (MyGlobals.EnterIngameFromOutgame)
        {
            GameType = MyGlobals.GameType;
            if (IsAdventure())
            {
                if (MyGlobals.StageNum == 0)
                    MyGlobals.StageNum = 1;
                StageNum = MyGlobals.StageNum;
                m_labelRemainTime.gameObject.SetActive(false);
            }
            else
            {
                m_labelRemainTime.gameObject.SetActive(true);
            }
            m_eCharacter = PrefsMgr.Instance.GetChoosenCharacter();
            m_iCharacterLv = PrefsMgr.Instance.GetCharacterLevel(m_eCharacter);
            m_bIsTest = false;
            m_eMaxOperator = GetMaxOperator();
        }
        else
        {
            m_bIsTest = true;
            if(MyGlobals.EnterIngameFromTestMode)
                StageNum = MyGlobals.StageNum;
            else
                MyGlobals.StageNum = StageNum;

            if (IsAdventure())
                m_labelRemainTime.gameObject.SetActive(false);
            else
            {
                m_labelRemainTime.gameObject.SetActive(true);
                m_eMaxOperator = GetMaxOperator();
            }
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

    eOPERATOR GetMaxOperator()
    {
        if(IsAdventure())
        {
            if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.DIV))
                return eOPERATOR.DIVISION;
            else if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.MUL))
                return eOPERATOR.MULTIPLICATION;
            else if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.SUB))
                return eOPERATOR.SUBTRACTION;
            else
                return eOPERATOR.ADDITION;
        }
        else
        {
            return eOPERATOR.DIVISION;
        }
    }

    void Start ()
    {
        MyGlobals.EnteringIngame = false;
        RemainTime = 999;
        //if (GameType == INGAME_TYPE.ADVENTURE)
        //{
            StartCoroutine("CoroutineCheckPlayTime");
        //}
        //else
        //{
        //}
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
#endif
    }

    public bool IsAdventure()
    {
        if (GameType == INGAME_TYPE.ADVENTURE)
            return true;
        else
            return false;
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
            if (!IsAdventure())
            {
                RemainTime -= Time.deltaTime;
                m_labelRemainTime.text = Mathf.RoundToInt(RemainTime).ToString();

                if (RemainTime < 0)
                {
                    RemainTime = 0f;
                    EventListener.Broadcast("OnGameClear");
                }
            }

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
        if (m_bIsTest)
            MyGlobals.EnterIngameFromTestMode = true;
        ++MyGlobals.StageNum;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
        Resources.UnloadUnusedAssets();
    }
}
