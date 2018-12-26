using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGaugeCtrl : MonoBehaviour
{
    public UISprite m_sprGauge;
    public UISprite m_sprButton;
    public float m_fFillAmountGreat;
    public float m_fFillAmountCool;
    public float m_fFillAmountNice;
    public float m_fTimeToFill;
    public Animation m_AnimGaugeGain;

    void Start ()
    {
        EventListener.AddListener("OnCorrectAnswer", this);
        m_sprGauge.fillAmount = 0.1f;
        m_sprButton.gameObject.SetActive(false);
    }
	
    public void OnActivateSuperSkill()
    {
        m_sprButton.gameObject.SetActive(false);
        EventListener.Broadcast("OnActivateSuperSkill");

        m_fFillAmountTarget = 0.1f;
        StopCoroutine("CoroutineFillGauge");
        StartCoroutine("CoroutineFillGauge");
    }

    void OnCorrectAnswer()
    {
        if (MyGlobals.DigitSpawner.m_eEvaluation == eEVALUATION.GREAT)
            m_fFillAmountTarget += m_fFillAmountGreat;
        else if (MyGlobals.DigitSpawner.m_eEvaluation == eEVALUATION.COOL)
            m_fFillAmountTarget += m_fFillAmountCool;
        else
            m_fFillAmountTarget += m_fFillAmountNice;

        StopCoroutine("CoroutineFillGauge");
        StartCoroutine("CoroutineFillGauge");

        //StopCoroutine("CoroutinePlayGaugeEffect");
        //StartCoroutine("CoroutinePlayGaugeEffect");
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
            m_sprButton.gameObject.SetActive(true);
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

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
