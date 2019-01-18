using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCHARACTER_STATE
{
    IDLE,
    SKILL,
    TOUCH
}

public class CharacterCtrl : MonoBehaviour
{
    public GameObject[] m_arrAdd;
    public AnimationClip m_ClipSkill_Add;
    public float m_fSkillDuration_Add;
    public GameObject[] m_arrSub;
    public AnimationClip m_ClipSkill_Sub;
    public float m_fSkillDuration_Sub;
    public GameObject[] m_arrMul;
    public AnimationClip m_ClipSkill_Mul;
    public float m_fSkillDuration_Mul;
    public GameObject[] m_arrDiv;
    public AnimationClip m_ClipSkill_Div;
    public float m_fSkillDuration_Div;
    public eCHARACTER m_eCurCharacter;
    float m_fSkillDuration;

    private void Awake()
    {
        EventListener.AddListener("OnActivateSuperSkill", this);
        //1차 발표버전에서는 캐릭터 애드/마이 중 하나 랜덤 등장
        m_eCurCharacter = Random.Range(0, 2) == 0 ? eCHARACTER.ADD : eCHARACTER.SUB;
    }

    void Start ()
    {
        //m_eCurCharacter = (eCHARACTER)PrefsMgr.Instance.GetInt(PrefsMgr.strChoosenChar, (int)eCHARACTER.ADD);

        for (int i = 0; i < m_arrAdd.Length; ++i)
        {
            m_arrAdd[i].SetActive(false);
        }

        for (int i = 0; i < m_arrSub.Length; ++i)
        {
            m_arrSub[i].SetActive(false);
        }

        for (int i = 0; i < m_arrMul.Length; ++i)
        {
            m_arrMul[i].SetActive(false);
        }

        for (int i = 0; i < m_arrDiv.Length; ++i)
        {
            m_arrDiv[i].SetActive(false);
        }

        switch (m_eCurCharacter)
        {
            case eCHARACTER.ADD:
                //m_fSkillDuration = m_fSkillDuration_Add;
                if(m_ClipSkill_Add)
                    m_fSkillDuration = m_ClipSkill_Add.length;
                m_arrAdd[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            case eCHARACTER.SUB:
                //m_fSkillDuration = m_fSkillDuration_Sub;
                if (m_ClipSkill_Sub)
                    m_fSkillDuration = m_ClipSkill_Sub.length;
                m_arrSub[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            case eCHARACTER.MUL:
                //m_fSkillDuration = m_fSkillDuration_Mul;
                if (m_ClipSkill_Mul)
                    m_fSkillDuration = m_ClipSkill_Mul.length;
                m_arrMul[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            case eCHARACTER.DIV:
                //m_fSkillDuration = m_fSkillDuration_Div;
                if (m_ClipSkill_Div)
                    m_fSkillDuration = m_ClipSkill_Div.length;
                m_arrDiv[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                break;
            default:
                m_fSkillDuration = m_fSkillDuration_Add;
                break;
        }
    }
	
	void OnActivateSuperSkill()
    {
		switch (m_eCurCharacter)
        {
            case eCHARACTER.ADD:
                m_arrAdd[(int)eCHARACTER_STATE.IDLE].SetActive(false);
                m_arrAdd[(int)eCHARACTER_STATE.SKILL].SetActive(true);
                break;
            case eCHARACTER.SUB:
                m_arrSub[(int)eCHARACTER_STATE.IDLE].SetActive(false);
                m_arrSub[(int)eCHARACTER_STATE.SKILL].SetActive(true);
                break;
            case eCHARACTER.MUL:
                m_arrMul[(int)eCHARACTER_STATE.IDLE].SetActive(false);
                m_arrMul[(int)eCHARACTER_STATE.SKILL].SetActive(true);
                break;
            case eCHARACTER.DIV:
                m_arrDiv[(int)eCHARACTER_STATE.IDLE].SetActive(false);
                m_arrDiv[(int)eCHARACTER_STATE.SKILL].SetActive(true);
                break;
        }

        StopCoroutine("CoroutineBackToIdle");
        StartCoroutine("CoroutineBackToIdle");
    }

    IEnumerator CoroutineBackToIdle()
    {
        yield return new WaitForSeconds(m_fSkillDuration);

        switch (m_eCurCharacter)
        {
            case eCHARACTER.ADD:
                m_arrAdd[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                m_arrAdd[(int)eCHARACTER_STATE.SKILL].SetActive(false);
                break;
            case eCHARACTER.SUB:
                m_arrSub[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                m_arrSub[(int)eCHARACTER_STATE.SKILL].SetActive(false);
                break;
            case eCHARACTER.MUL:
                m_arrMul[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                m_arrMul[(int)eCHARACTER_STATE.SKILL].SetActive(false);
                break;
            case eCHARACTER.DIV:
                m_arrDiv[(int)eCHARACTER_STATE.IDLE].SetActive(true);
                m_arrDiv[(int)eCHARACTER_STATE.SKILL].SetActive(false);
                break;
        }

        EventListener.Broadcast("OnDeactivateSuperSkill");
    }
}
