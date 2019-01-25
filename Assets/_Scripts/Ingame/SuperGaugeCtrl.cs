using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGaugeCtrl : MonoBehaviour
{
    public UISprite m_sprGauge;
    public UISprite m_sprBtn;
    public UISprite m_sprIcon;
    public Color m_colorDiable;
    public float m_fActivateTime;
    public float m_fCoolTime;
    bool m_bIsCoolTime;
    bool m_bIsDiable;
    float m_fFillAmountBtn;
    public TweenScale m_tweenScale;
    public float m_fFillAmountDefault = 0.1f;
    public float m_fFillAmountGreat;
    public float m_fFillAmountCool;
    public float m_fFillAmountNice;
    public float m_fTimeToFill;
    public Animation m_AnimGaugeGain;
    public float m_fBonusPointForFullGauge = 800f;
    //public float m_fEachCharacterBonusScore;

    void Start ()
    {
        EventListener.AddListener("OnCorrectAnswer", this);
        EventListener.AddListener("OnDisableSkill", this);
        EventListener.AddListener("OnBonusAchieved", this);
        m_sprGauge.fillAmount = m_fFillAmountDefault;
        m_sprIcon.gameObject.SetActive(false);

        m_fActivateTime = (float)TableDB.Instance.GetData(eTABLE_LIST.ITEM_ID,
                                        405, eKEY_TABLEDB.f_ACTIVATE_DURARION);
        m_fCoolTime = (float)TableDB.Instance.GetData(eTABLE_LIST.ITEM_ID,
                                405, eKEY_TABLEDB.f_COOLDOWN_DURATION);

        m_fBonusPointForFullGauge = MyGlobals.StageMgr.m_fBonusPointForFullGauge;
    }
	
    public void OnActivateSuperSkill()
    {
        if (MyGlobals.StageMgr.IsDeactiveSkill)
            return;

        if (m_bIsCoolTime || m_bIsDiable)
            return;

        m_sprIcon.gameObject.SetActive(false);
        EventListener.Broadcast("OnActivateSuperSkill");

        m_fFillAmountTarget = m_fFillAmountDefault;
        StopCoroutine("CoroutineFillGauge");
        StartCoroutine("CoroutineFillGauge");

        StopCoroutine("CoroutineCoolTime");
        StartCoroutine("CoroutineCoolTime");

        MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.SUPER_POWER_ACTIVATE);
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

    //정답 판정에 따라 게이지 채우는 버전
    //void OnCorrectAnswer(bool bByItem)
    //{
    //    if (bByItem)
    //        return;

    //    if (m_sprGauge.fillAmount >= 1f)
    //        return;

    //    if (MyGlobals.DigitSpawner.m_eEvaluation == eEVALUATION.GREAT)
    //        m_fFillAmountTarget += m_fFillAmountGreat;
    //    else if (MyGlobals.DigitSpawner.m_eEvaluation == eEVALUATION.COOL)
    //        m_fFillAmountTarget += m_fFillAmountCool;
    //    else
    //        m_fFillAmountTarget += m_fFillAmountNice;

    //    StopCoroutine("CoroutineFillGauge");
    //    StartCoroutine("CoroutineFillGauge");
    //}

    void OnBonusAchieved()//float fBonusPoint)//보너스 점수 획득시 게이지 채우는 버전
    {
        //if (bByItem)
        //    return;

        if (m_sprGauge.fillAmount >= 1f)
            return;

        m_fFillAmountTarget += (float)MyGlobals.ScoreMgr.EachCharBonusScore / m_fBonusPointForFullGauge;

        StopCoroutine("CoroutineFillGauge");
        StartCoroutine("CoroutineFillGauge");
    }

    float m_fFillAmountTarget;
    IEnumerator CoroutineFillGauge()
    {
        float fElased = 0.0f;
        float fFillAmountOrigin = m_sprGauge.fillAmount;
        while(fElased < m_fTimeToFill)
        {
            fElased += Time.deltaTime;

            m_sprGauge.fillAmount = Mathf.Lerp(fFillAmountOrigin, m_fFillAmountTarget, fElased / m_fTimeToFill);

            yield return null;
        }
        m_sprGauge.fillAmount = m_fFillAmountTarget;
        if (m_sprGauge.fillAmount >= 1.0)
        {
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.SUPER_POWER_MAX);
            m_sprIcon.gameObject.SetActive(true);
        }
    }

    IEnumerator CoroutinePlayGaugeEffect()  ////show the booster gain effect(휘리릭)
    {
        float AAnimLength = m_AnimGaugeGain.clip.length;

        m_AnimGaugeGain.gameObject.SetActive(true);

        m_AnimGaugeGain.Rewind();
        m_AnimGaugeGain.Play();

        yield return new WaitForSeconds(AAnimLength);

        m_AnimGaugeGain.Stop();
        m_AnimGaugeGain.gameObject.SetActive(false);
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
            if (MyGlobals.StageMgr.IsPauseDrop)
            {
                yield return null;
                continue;
            }

            fElased += Time.deltaTime;
            m_fFillAmountBtn = fElased / m_fCoolTime;

            if (m_sprBtn)
                m_sprBtn.fillAmount = m_fFillAmountBtn;
            if (m_sprIcon)
                m_sprIcon.fillAmount = m_fFillAmountBtn;

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

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
