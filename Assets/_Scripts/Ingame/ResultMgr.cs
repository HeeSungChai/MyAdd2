using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMgr : MonoBehaviour
{
    public GameObject m_objGameClear;
    [Header("Adventure")]
    public GameObject m_objGameClear_Adventure;
    public ScoreCounter m_scriptCounterScore;
    public ScoreCounter m_scriptCounterBonus;
    public ScoreCounter m_scriptCounterTotalScore;

    [Header("Infinite")]
    public GameObject m_objGameClear_Infinite;
    public ScoreCounter m_scriptCounter_ScoreInfinite;
    public ScoreCounter m_scriptCounter_BonusInfinite;
    public ScoreCounter m_scriptCounter_ComboInfinite;
    public ScoreCounter m_scriptCounter_TotalScoreInfinite;

    void OnEnable ()
    {
        SetScoreValues();

        StartCoroutine("CoroutineGameClearSeauence");
    }

    void SetScoreValues()
    {
        if (MyGlobals.StageMgr.GameType == INGAME_TYPE.ADVENTURE)
        {
            m_scriptCounterScore.Init(0, MyGlobals.ScoreMgr.TotalBasicScore, 1.0f);
            m_scriptCounterBonus.Init(0, MyGlobals.ScoreMgr.TotalCharBonusScore, 1.0f);
            m_scriptCounterTotalScore.Init(0, MyGlobals.ScoreMgr.TotalScore, 1.0f);

            PrefsMgr.Instance.SetInt(PrefsMgr.strStageScore + MyGlobals.StageMgr.StageNum, MyGlobals.ScoreMgr.TotalScore);
        }
        else
        {
            m_scriptCounter_ScoreInfinite.Init(0, MyGlobals.ScoreMgr.TotalBasicScore, 1.0f);
            m_scriptCounter_BonusInfinite.Init(0, MyGlobals.ScoreMgr.TotalCharBonusScore, 1.0f);
            m_scriptCounter_ComboInfinite.Init(0, MyGlobals.ScoreMgr.TotalComboBonusScore, 1.0f);
            m_scriptCounter_TotalScoreInfinite.Init(0, MyGlobals.ScoreMgr.TotalScore, 1.0f);
        }
    }

    IEnumerator CoroutineGameClearSeauence()
    {
        yield return new WaitForSeconds(0.5f);

        m_objGameClear.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        //일거리. 새로운 칭호 얻거나 새로 오픈된 캐릭터 연출 추가

        m_objGameClear.SetActive(false);

        if (MyGlobals.StageMgr.GameType == INGAME_TYPE.ADVENTURE)
        {
            if (m_objGameClear_Adventure)
                m_objGameClear_Adventure.SetActive(true);
        }
        else
        {
            if (m_objGameClear_Infinite)
                m_objGameClear_Infinite.SetActive(true);
        }
    }
}
