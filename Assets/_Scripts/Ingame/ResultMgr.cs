using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMgr : MonoBehaviour
{
    public GameObject m_objGameClear;
    eTABLE_LIST m_eTableName = eTABLE_LIST.REWARD_TABLE;
    eTABLE_LIST m_eTitleTableName = eTABLE_LIST.TITLE_MARK;
    public GameObject m_objNewTitle;
    public GameObject m_objNewCharacter;

    [Header("Adventure")]
    public GameObject m_objGameClear_Adventure;
    public ScoreCounter m_scriptCounterScore;
    public ScoreCounter m_scriptCounterBonus;
    public ScoreCounter m_scriptCounterTotalScore;
    public GameObject m_objNewRecord_Adventure;
    int m_iUnlockID;

    [Header("Infinite")]
    public GameObject m_objGameClear_Infinite;
    public ScoreCounter m_scriptCounter_ScoreInfinite;
    public ScoreCounter m_scriptCounter_BonusInfinite;
    public ScoreCounter m_scriptCounter_ComboInfinite;
    public ScoreCounter m_scriptCounter_TotalScoreInfinite;
    public GameObject m_objNewRecord_Infinite;



    void OnEnable ()
    {
        SetScoreValues();

        StartCoroutine("CoroutineGameClearSequence");
    }

    void SetScoreValues()
    {
        if (MyGlobals.StageMgr.GameType == INGAME_TYPE.ADVENTURE)
        {
            m_scriptCounterScore.Init(0, MyGlobals.ScoreMgr.TotalBasicScore, 1.0f);
            m_scriptCounterBonus.Init(0, MyGlobals.ScoreMgr.TotalCharBonusScore, 1.0f);
            m_scriptCounterTotalScore.Init(0, MyGlobals.ScoreMgr.TotalScore, 1.0f);

            if (MyGlobals.ScoreMgr.TotalScore > PrefsMgr.Instance.GetStageScore(MyGlobals.StageNum))
            {
                m_objNewRecord_Adventure.SetActive(true);
                PrefsMgr.Instance.SetStageScore(MyGlobals.ScoreMgr.TotalScore);
            }
        }
        else
        {
            m_scriptCounter_ScoreInfinite.Init(0, MyGlobals.ScoreMgr.TotalBasicScore, 1.0f);
            m_scriptCounter_BonusInfinite.Init(0, MyGlobals.ScoreMgr.TotalCharBonusScore, 1.0f);
            m_scriptCounter_ComboInfinite.Init(0, MyGlobals.ScoreMgr.TotalComboBonusScore, 1.0f);
            m_scriptCounter_TotalScoreInfinite.Init(0, MyGlobals.ScoreMgr.TotalScore, 1.0f);

            if (MyGlobals.ScoreMgr.TotalScore > PrefsMgr.Instance.GetInfiniteModeBestScore())
            {
                m_objNewRecord_Infinite.SetActive(true);
                PrefsMgr.Instance.SetInfiniteModeBestScore(MyGlobals.ScoreMgr.TotalScore);
            }
        }
    }

    IEnumerator CoroutineGameClearSequence()
    {
        if (MyGlobals.StageMgr.IsAdventure())
        {
            yield return new WaitForSeconds(0.5f);

            m_objGameClear.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            //새로운 칭호 얻거나 새로 오픈된 캐릭터 연출 추가
            yield return StartCoroutine("CoroutineCheckNewTitleOrNewCharacter");

        }
        m_objGameClear.SetActive(false);

        if (MyGlobals.StageMgr.IsAdventure())
        {
            if (m_objGameClear_Adventure)
                m_objGameClear_Adventure.SetActive(true);
            if (m_objGameClear_Infinite)
                m_objGameClear_Infinite.SetActive(false);
        }
        else
        {
            if (m_objGameClear_Adventure)
                m_objGameClear_Adventure.SetActive(false);
            if (m_objGameClear_Infinite)
                m_objGameClear_Infinite.SetActive(true);
        }
    }

    IEnumerator CoroutineCheckNewTitleOrNewCharacter()
    {
        //새로운 칭호 획득 조건 체크
        if (CheckNewTitleCondition())
        {
            ShowTitleUpgrade();
            EventListener.AddListener("OnTouched", this);
            yield return StartCoroutine("CoroutineWaitUntilTouch");
            m_objNewTitle.SetActive(false);
        }

        //새로운 캐릭터 획득 조건 체크
        m_iUnlockID = (int)TableDB.Instance.GetData(m_eTableName, MyGlobals.StageMgr.StageNum, eKEY_TABLEDB.i_UNLOCK_ID);
        if (m_iUnlockID != -1)
        {
            yield return StartCoroutine("CoroutineUnlock");
            m_objNewCharacter.SetActive(false);
        }

        //yield return StartCoroutine("CoroutineWaitUntilTouch");
    }

    bool CheckNewTitleCondition()
    {
        eUSER_TITLE eCurTitle = PrefsMgr.Instance.GetTitle();
        //이미 수학의 신이면 리턴
        if (eCurTitle == eUSER_TITLE.GOD_OF_MATH)
            return false;

        int iConditionValue = (int)TableDB.Instance.GetData(m_eTitleTableName, (int)eCurTitle + 1, eKEY_TABLEDB.i_CONDITION);
        //박사면 무한모드에서 100콤보 달성했으면 진급처리
        if (eCurTitle == eUSER_TITLE.DOCTOR)
        {
            iConditionValue = (int)TableDB.Instance.GetData(m_eTitleTableName, (int)eCurTitle + 1, eKEY_TABLEDB.i_CONDITION);
            if (MyGlobals.StageMgr.IsAdventure())
                return false;
            else if (MyGlobals.StageMgr.MaxComboCount > iConditionValue)
                return true;
        }
        //박사 미만이면 그 다음 칭호의 condition에서 스테이지값 읽어와 현재 스테이지가 그 이상이면 새 칭호부여
        else if(eCurTitle < eUSER_TITLE.DOCTOR)
        {
            iConditionValue = (int)TableDB.Instance.GetData(m_eTitleTableName, (int)eCurTitle + 1, eKEY_TABLEDB.i_CONDITION);
            if (MyGlobals.StageNum >= iConditionValue)
                return true;
            else
                return false;
        }

        return false;
    }

    void ShowTitleUpgrade()
    {
        eUSER_TITLE eCurTitle = PrefsMgr.Instance.GetTitle();
        PrefsMgr.Instance.SetTitle((eUSER_TITLE)eCurTitle + 1);

        eKEY_TABLEDB eKeyName;
        if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.ENGLISH)
            eKeyName = eKEY_TABLEDB.s_TITLE_NAME_US;
        else
            eKeyName = eKEY_TABLEDB.s_TITLE_NAME_KR;

        string strResource = (string)TableDB.Instance.GetData(m_eTitleTableName, (int)eCurTitle, eKEY_TABLEDB.s_RESOURCE);
        string strName = (string)TableDB.Instance.GetData(m_eTitleTableName, (int)eCurTitle, eKeyName);
        string strExplanation = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.NEW_TITLE);

        m_objNewTitle.GetComponent<ShowUnlock>().init(strResource, strName, strExplanation);
        m_objNewTitle.SetActive(true);
    }

    //만약 보상목록에 있는 캐릭터가 아직 오픈되지 않았으면
    //오픈 연출 띄우고
    //해당 캐릭터 Open상태 true로
    IEnumerator CoroutineUnlock()
    {
        EventListener.AddListener("OnTouched", this);
        switch ((eCHARACTER)m_iUnlockID)
        {
            case eCHARACTER.SUB:
                if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.SUB) == false)
                {
                    ShowNewCharacterOpen(eCHARACTER.SUB);
                    PrefsMgr.Instance.SetCharacterOpen(eCHARACTER.SUB);
                    yield return StartCoroutine("CoroutineWaitUntilTouch");
                }
                break;
            case eCHARACTER.MUL:
                if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.MUL) == false)
                {
                    ShowNewCharacterOpen(eCHARACTER.MUL);
                    PrefsMgr.Instance.SetCharacterOpen(eCHARACTER.MUL);
                    yield return StartCoroutine("CoroutineWaitUntilTouch");
                }
                break;
            case eCHARACTER.DIV:
                if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.DIV) == false)
                {
                    ShowNewCharacterOpen(eCHARACTER.DIV);
                    PrefsMgr.Instance.SetCharacterOpen(eCHARACTER.DIV);
                    yield return StartCoroutine("CoroutineWaitUntilTouch");
                }
                break;
            case eCHARACTER.INFINITY_CHALLENGER:
                if (PrefsMgr.Instance.GetInfiniteModeOpen() == false)
                    PrefsMgr.Instance.SetInfiniteModeOpen();
                break;
            default:
                break;
        }
        yield return null;
    }

    void ShowNewCharacterOpen(eCHARACTER eChar)
    {
        eTABLE_LIST m_eCharInfoTable = eTABLE_LIST.CHAR_INFO;
        eTABLE_LIST eCharLevelTable;

        switch (eChar)
        {
            case eCHARACTER.SUB:
                eCharLevelTable = eTABLE_LIST.CHAR_LEVEL_SUB;
                break;
            case eCHARACTER.MUL:
                eCharLevelTable = eTABLE_LIST.CHAR_LEVEL_SUB;
                break;
            case eCHARACTER.DIV:
                eCharLevelTable = eTABLE_LIST.CHAR_LEVEL_SUB;
                break;
            default:
                eCharLevelTable = eTABLE_LIST.CHAR_LEVEL_SUB;
                break;
        }

        eKEY_TABLEDB eKeySkill;
        if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.ENGLISH)
            eKeySkill = eKEY_TABLEDB.s_SKILL_US;
        else
            eKeySkill = eKEY_TABLEDB.s_SKILL_KR;

        string strSkill = string.Format( (string)TableDB.Instance.GetData(m_eCharInfoTable,
                                                                (int)eChar, eKeySkill),
                                        TableDB.Instance.GetData(eCharLevelTable,
                                                                1, eKEY_TABLEDB.i_SKILL_VALUE));
        string strExplanation = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.NEW_CHARACTER);

        m_objNewCharacter.GetComponent<ShowUnlockCharacter>().init(eChar, strSkill, strExplanation);
        m_objNewCharacter.SetActive(true);
    }

    bool bTouced;
    void OnTouched()
    {
        bTouced = true;
    }

    IEnumerator CoroutineWaitUntilTouch()
    {
        yield return new WaitForSeconds(0.5f);

        bTouced = false;

        while(true)
        {
            if(bTouced)
                yield break;

            yield return null;
        }
    }

    private void OnDestroy()
    {
        EventListener.RemoveListener(this);
        StopAllCoroutines();
    }
}
