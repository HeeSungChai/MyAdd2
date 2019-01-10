using System.Collections;
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
        m_iCurVolumnFx = Mathf.RoundToInt(PlayerPrefs.GetFloat("VolumnFx", 1f) * 10f);
        m_labelVolumnFxValue.text = m_iCurVolumnFx.ToString();
        m_iCurVolumnBGM = Mathf.RoundToInt(PlayerPrefs.GetFloat("VolumnBGM", 1f) * 10f);
        m_labelVolumnBGMValue.text = m_iCurVolumnBGM.ToString();
        //m_eCurLanguage = (eLANGUAGE)PlayerPrefs.GetInt("Language", 0);
        m_eCurLanguage = LanguageMgr.Instance.GetLanguage();
    }
	
	public void OnFxVolumnUp()
    {
        if (m_iCurVolumnFx >= 10)
            return;

        ++m_iCurVolumnFx;
        m_labelVolumnFxValue.text = m_iCurVolumnFx.ToString();
        MyGlobals.SoundMgr.OnFxVolumnUp();
	}

    public void OnFxVolumnDown()
    {
        if (m_iCurVolumnFx <= 0)
            return;

        --m_iCurVolumnFx;
        m_labelVolumnFxValue.text = m_iCurVolumnFx.ToString();
        MyGlobals.SoundMgr.OnFxVolumnDown();
    }

    public void OnBGMVolumnUp()
    {
        if (m_iCurVolumnBGM >= 10)
            return;

        ++m_iCurVolumnBGM;
        m_labelVolumnBGMValue.text = m_iCurVolumnBGM.ToString();
        MyGlobals.SoundMgr.OnBGMVolumnUp();
    }

    public void OnBGMVolumnDown()
    {
        if (m_iCurVolumnBGM <= 0)
            return;

        --m_iCurVolumnBGM;
        m_labelVolumnBGMValue.text = m_iCurVolumnBGM.ToString();
        MyGlobals.SoundMgr.OnBGMVolumnDown();
    }

    public void OnLanguageUp()
    {
        int iLanguage = (int)m_eCurLanguage;
        if (iLanguage >= 1)
            return;

        ++m_eCurLanguage;
        //m_labelLanguageValue.text = m_eCurLanguage.ToString();
        m_labelLanguageValue.text = LanguageMgr.Instance.GetLanguageData(
                                        eLANGUAGE_ID.OPTION_LANGUAGE_NAME,
                                        m_eCurLanguage);
    }

    public void OnLanguageDown()
    {
        int iLanguage = (int)m_eCurLanguage;
        if (iLanguage <= 0)
            return;

        --m_eCurLanguage;
        //m_labelLanguageValue.text = m_eCurLanguage.ToString();
        m_labelLanguageValue.text = LanguageMgr.Instance.GetLanguageData(
                                        eLANGUAGE_ID.OPTION_LANGUAGE_NAME,
                                        m_eCurLanguage);
    }

    public void OnApply()
    {
        MyGlobals.SoundMgr.OnSetVolumnFx(m_iCurVolumnFx);
        MyGlobals.SoundMgr.OnSetVolumnBGM(m_iCurVolumnBGM);
        LanguageMgr.Instance.SetLanguage(m_eCurLanguage);
        EventListener.Broadcast("OnLanguageChanged");
    }

    void OnLanguageChanged()
    {
        m_labelOption.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_SETTING);
        m_labelVolumnFxName.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_SOUND_EFFECT);
        m_labelVolumnBGMName.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_BGM);
        m_labelLanguageName.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_LANGUAGE);
        m_labelVolumnFxExplanation.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_SOUND_EFFECT_EXPLANATION);
        m_labelVolumnBGMExplanation.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_BGM_EXPLANATION);
        m_labelLanguageExplanation.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_LANGUAGE_EXPLANATION);
        m_labelApply.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.OPTION_APPLY_BTN);
    }
}
