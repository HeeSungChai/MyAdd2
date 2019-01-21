using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoolTimeCtrl : MonoBehaviour
{
    public UISprite m_sprBtn;
    public UISprite m_sprIcon;
    public Color m_colorDiable;
    public float m_fActivateTime;
    public float m_fCoolTime;
    float m_fFillAmount;
    bool m_bIsCoolTime;
    bool m_bIsDiable;
    public TweenScale m_tweenScale;
    public MyPlaySound m_scriptSound;
    public eITEM_TYPE m_eItemType;
    eTABLE_LIST m_eItemTable = eTABLE_LIST.ITEM_ID;

    private void Start()
    {
        if (m_tweenScale == null)
            m_tweenScale = GetComponentInChildren<TweenScale>();

        EventListener.AddListener("OnGameStart", this);
        EventListener.AddListener("OnDisableSkill", this);

        switch (m_eItemType)
        {
            case eITEM_TYPE.ERASER:
                m_fActivateTime = (float)TableDB.Instance.GetData(m_eItemTable,
                                        (int)eITEM_ID.ERASER, eKEY_TABLEDB.f_ACTIVATE_DURARION);
                m_fCoolTime = (float)TableDB.Instance.GetData(m_eItemTable,
                                        (int)eITEM_ID.ERASER, eKEY_TABLEDB.f_COOLDOWN_DURATION);
                break;
            case eITEM_TYPE.CLOCK:
                m_fActivateTime = (float)TableDB.Instance.GetData(m_eItemTable,
                                        (int)eITEM_ID.CLOCK, eKEY_TABLEDB.f_ACTIVATE_DURARION);
                m_fCoolTime = (float)TableDB.Instance.GetData(m_eItemTable,
                                        (int)eITEM_ID.CLOCK, eKEY_TABLEDB.f_COOLDOWN_DURATION);
                break;
            case eITEM_TYPE.RECOVERY:
                m_fActivateTime = (float)TableDB.Instance.GetData(m_eItemTable,
                                        (int)eITEM_ID.RECOVERY, eKEY_TABLEDB.f_ACTIVATE_DURARION);
                m_fCoolTime = (float)TableDB.Instance.GetData(m_eItemTable,
                                        (int)eITEM_ID.RECOVERY, eKEY_TABLEDB.f_COOLDOWN_DURATION);
                break;
            default:
                m_fCoolTime = 3f;
                break;
        }
    }

    void OnGameStart()
    {
        if (m_tweenScale)
        {
            m_tweenScale.ResetToBeginning();
            m_tweenScale.PlayForward();
        }
    }

    public bool IsCoolTime()
    {
        return m_bIsCoolTime;
    }

    public bool IsDisable()
    {
        return m_bIsDiable;
    }

    public void OnActivate()
    {
        if (m_bIsCoolTime)
            return;

        m_scriptSound.PlaySound();
        StopCoroutine("CoroutineCoolTime");
        StartCoroutine("CoroutineCoolTime");
    }

    void OnDisableSkill(bool bDisable)
    {
        m_bIsDiable = bDisable;

        if (bDisable)
        {
            m_sprBtn.color = m_colorDiable;
            m_sprIcon.color = m_colorDiable;
        }
        else
        {
            m_sprBtn.color = Color.white;
            m_sprIcon.color = Color.white;
        }
    }

    IEnumerator CoroutineCoolTime()
    {
        m_bIsCoolTime = true;

        MyGlobals.StageMgr.IsDeactiveSkill = true;
        EventListener.Broadcast("OnDisableSkill", true);
        yield return new WaitForSeconds(m_fActivateTime);
        MyGlobals.StageMgr.IsDeactiveSkill = false;
        EventListener.Broadcast("OnDisableSkill", false);

        float fElased = 0.0f;
        while (fElased < m_fCoolTime)
        {
            if(MyGlobals.StageMgr.IsPauseDrop)
            {
                yield return null;
                continue;
            }

            fElased += Time.deltaTime;
            m_fFillAmount = fElased / m_fCoolTime;

            if(m_sprBtn)
                m_sprBtn.fillAmount = m_fFillAmount;
            if(m_sprIcon)
                m_sprIcon.fillAmount = m_fFillAmount;

            yield return null;
        }

        if (m_sprBtn)
            m_sprBtn.fillAmount = 1.0f;
        if (m_sprIcon)
            m_sprIcon.fillAmount = 1.0f;
        m_bIsCoolTime = false;

        if (m_tweenScale)
        {
            m_tweenScale.ResetToBeginning();
            m_tweenScale.PlayForward();
        }
    }
}
