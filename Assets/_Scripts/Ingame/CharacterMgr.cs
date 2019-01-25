using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCHARACTER_STATE
{
    IDLE,
    SKILL,
    TOUCH
}

public class CharacterMgr : MonoBehaviour
{
    public GameObject[] m_arrAdd;
    public AnimationClip m_ClipSkill_Add;
    public GameObject[] m_arrSub;
    public AnimationClip m_ClipSkill_Sub;
    public GameObject[] m_arrMul;
    public AnimationClip m_ClipSkill_Mul;
    public GameObject[] m_arrDiv;
    public AnimationClip m_ClipSkill_Div;
    public eCHARACTER CurCharacter;
    float m_fSkillDuration;

    private void Awake()
    {
        MyGlobals.CharacterMgr = this;
        EventListener.AddListener("OnActivateSuperSkill", this);
        //1차 발표버전에서는 캐릭터 애드/마이 중 하나 랜덤 등장
        //m_eCurCharacter = Random.Range(0, 2) == 0 ? eCHARACTER.ADD : eCHARACTER.SUB;
        //CurCharacter = (eCHARACTER)PrefsMgr.Instance.GetInt(PrefsMgr.strChoosenChar, (int)eCHARACTER.ADD);
        //if (MyGlobals.EnterIngameFromOutgame)
        //    CurCharacter = PrefsMgr.Instance.GetChoosenCharacter();
        //else
        //    CurCharacter = MyGlobals.StageMgr.m_eCharacter;
    }

    void Start()
    {
        MyUtility.ActivateAll(m_arrAdd, false);
        MyUtility.ActivateAll(m_arrSub, false);
        MyUtility.ActivateAll(m_arrMul, false);
        MyUtility.ActivateAll(m_arrDiv, false);

        if (MyGlobals.EnterIngameFromOutgame)
            CurCharacter = PrefsMgr.Instance.GetChoosenCharacter();
        else
            CurCharacter = MyGlobals.StageMgr.m_eCharacter;

        switch (CurCharacter)
        {
            case eCHARACTER.ADD:
                if (m_ClipSkill_Add)
                    m_fSkillDuration = m_ClipSkill_Add.length;
                m_arrAdd[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            case eCHARACTER.SUB:
                if (m_ClipSkill_Sub)
                    m_fSkillDuration = m_ClipSkill_Sub.length;
                m_arrSub[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            case eCHARACTER.MUL:
                if (m_ClipSkill_Mul)
                    m_fSkillDuration = m_ClipSkill_Mul.length;
                m_arrMul[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            case eCHARACTER.DIV:
                if (m_ClipSkill_Div)
                    m_fSkillDuration = m_ClipSkill_Div.length;
                m_arrDiv[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            default:
                m_fSkillDuration = 3f;
                break;
        }
    }

    void OnActivateSuperSkill()
    {
        ActivateSkillMotion(true);

        StopCoroutine("CoroutineBackToIdle");
        StartCoroutine("CoroutineBackToIdle");
    }

    IEnumerator CoroutineBackToIdle()
    {
        yield return new WaitForSeconds(m_fSkillDuration);

        ActivateSkillMotion(false);

        EventListener.Broadcast("OnDeactivateSuperSkill");
    }

    void ActivateSkillMotion(bool bActivate)
    {
        switch (CurCharacter)
        {
            case eCHARACTER.ADD:
                m_arrAdd[(int)eCHARACTER_STATE.IDLE].SetActive(!bActivate);
                m_arrAdd[(int)eCHARACTER_STATE.SKILL].SetActive(bActivate);
                break;
            case eCHARACTER.SUB:
                m_arrSub[(int)eCHARACTER_STATE.IDLE].SetActive(!bActivate);
                m_arrSub[(int)eCHARACTER_STATE.SKILL].SetActive(bActivate);
                break;
            case eCHARACTER.MUL:
                m_arrMul[(int)eCHARACTER_STATE.IDLE].SetActive(!bActivate);
                m_arrMul[(int)eCHARACTER_STATE.SKILL].SetActive(bActivate);
                break;
            case eCHARACTER.DIV:
                m_arrDiv[(int)eCHARACTER_STATE.IDLE].SetActive(!bActivate);
                m_arrDiv[(int)eCHARACTER_STATE.SKILL].SetActive(bActivate);
                break;
        }
    }
}
