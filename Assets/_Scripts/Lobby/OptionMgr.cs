﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMgr : MonoBehaviour
{
    public UILabel m_labelOption;
    public UILabel m_labelVolumnFxName;
    public UILabel m_labelVolumnBGMName;
    public UILabel m_labelLanguageName;
    public UILabel m_labelVolumnFxValue;
    public UILabel m_labelVolumnBGMValue;
    public UILabel m_labelLanguageValue;
    public UILabel m_labelVolumnFxExplanation;
    public UILabel m_labelVolumnBGMExplanation;
    public UILabel m_labelLanguageExplanation;
    public UILabel m_labelApply;

    int m_iCurVolumnFx;
    int m_iCurVolumnBGM;
    eLANGUAGE m_eCurLanguage;
    eKEY_TABLEDB m_eKeyLanguage;

    private void Awake()
    {
        EventListener.AddListener("OnLanguageChanged", this);
    }

    void OnEnable()
    {
        m_iCurVolumnFx = Mathf.RoundToInt(PrefsMgr.Instance.GetFloat(PrefsMgr.strVolumnFX, 1f) * 10f);
        m_labelVolumnFxValue.text = m_iCurVolumnFx.ToString();
        m_iCurVolumnBGM = Mathf.RoundToInt(PrefsMgr.Instance.GetFloat(PrefsMgr.strVolumnBGM, 1f) * 10f);
        m_labelVolumnBGMValue.text = m_iCurVolumnBGM.ToString();
        m_eCurLanguage = LanguageMgr.Instance.GetLanguage();
        OnLanguageChanged();
        SetLanguageLabel();
    }
	
	public void OnFxVolumnUp()
    {
        if (m_iCurVolumnFx >= 10)
            return;

        ++m_iCurVolumnFx;
        m_labelVolumnFxValue.text = m_iCurVolumnFx.ToString();
	}

    public void OnFxVolumnDown()
    {
        if (m_iCurVolumnFx <= 0)
            return;

        --m_iCurVolumnFx;
        m_labelVolumnFxValue.text = m_iCurVolumnFx.ToString();
    }

    public void OnBGMVolumnUp()
    {
        if (m_iCurVolumnBGM >= 10)
            return;

        ++m_iCurVolumnBGM;
        m_labelVolumnBGMValue.text = m_iCurVolumnBGM.ToString();
    }

    public void OnBGMVolumnDown()
    {
        if (m_iCurVolumnBGM <= 0)
            return;

        --m_iCurVolumnBGM;
        m_labelVolumnBGMValue.text = m_iCurVolumnBGM.ToString();
    }

    public void OnLanguageUp()
    {
        int iLanguage = (int)m_eCurLanguage;
        //if (iLanguage >= 1)
        //    return;
        //++m_eCurLanguage;

        ++iLanguage;
        iLanguage = iLanguage % 2;
        m_eCurLanguage = (eLANGUAGE)iLanguage;
        SetLanguageLabel();
    }

    public void OnLanguageDown()
    {
        int iLanguage = (int)m_eCurLanguage;
        //if (iLanguage <= 0)
        //    return;
        //--m_eCurLanguage;

        --iLanguage;
        iLanguage = Mathf.Abs(iLanguage % 2);
        m_eCurLanguage = (eLANGUAGE)iLanguage;
        SetLanguageLabel();
    }

    public void OnApply()
    {
        MyGlobals.SoundMgr.OnSetVolumnFx(m_iCurVolumnFx);
        MyGlobals.SoundMgr.OnSetVolumnBGM(m_iCurVolumnBGM);
        LanguageMgr.Instance.SetLanguage(m_eCurLanguage);
        EventListener.Broadcast("OnLanguageChanged");

        SetLanguageLabel();
    }

    void SetLanguageLabel()
    {
        eLANGUAGE_DATA eLangData;
        if (m_eCurLanguage == eLANGUAGE.ENGLISH)
            eLangData = eLANGUAGE_DATA.ENGLISH;
        else
            eLangData = eLANGUAGE_DATA.KOREAN;

        m_labelLanguageValue.text = LanguageMgr.Instance.GetLanguageData(
                                        eLangData,
                                        LanguageMgr.Instance.GetLanguage());
    }

    void OnLanguageChanged()
    {
        //if(m_labelOption)
        //    m_labelOption.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_SETTING);
        if (m_labelVolumnFxName)
            m_labelVolumnFxName.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.FX);
        if (m_labelVolumnBGMName)
            m_labelVolumnBGMName.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.BGM);
        if (m_labelLanguageName)
            m_labelLanguageName.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.LANGUAGE);
        //if (m_labelLanguageValue)
        //    m_labelLanguageValue.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_LANGUAGE_VALUE);
        if (m_labelVolumnFxExplanation)
            m_labelVolumnFxExplanation.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.FX_EXPLANATION);
        if (m_labelVolumnBGMExplanation)
            m_labelVolumnBGMExplanation.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.BGM_EXPLANATION);
        if (m_labelLanguageExplanation)
            m_labelLanguageExplanation.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.LANGUAGE_EXPLANATION);
        if (m_labelApply)
            m_labelApply.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.APPLY);
    }
}
