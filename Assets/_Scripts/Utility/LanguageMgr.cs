using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eLANGUAGE
{
    KOREAN,
    ENGLISH
}

public class LanguageMgr
{
    public eTABLE_LIST m_LanguageTable = eTABLE_LIST.LANGUAGE;
    public eKEY_TABLEDB LanguageKey { get; set; }

    #region Singleton Pattern Implementation
    private static LanguageMgr instance;

    public static LanguageMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new LanguageMgr();
            return instance;
        }
    }
    #endregion

    public eLANGUAGE GetLanguage()
    {
        return (eLANGUAGE)PrefsMgr.Instance.GetInt(PrefsMgr.strLanguage, 0);
    }

    public void SetLanguage(eLANGUAGE eLanguage)
    {
        PrefsMgr.Instance.SetInt(PrefsMgr.strLanguage, (int)eLanguage);
        if (eLanguage == eLANGUAGE.KOREAN)
            LanguageKey = eKEY_TABLEDB.s_LANGUAGE_KOR;
        else
            LanguageKey = eKEY_TABLEDB.s_LANGUAGE_ENG;
    }

    public string GetLanguageData(eLANGUAGE_DATA eLangID)
    {
        if ((eLANGUAGE)PrefsMgr.Instance.GetInt(PrefsMgr.strLanguage, 0) == eLANGUAGE.KOREAN)
            LanguageKey = eKEY_TABLEDB.s_LANGUAGE_KOR;
        else
            LanguageKey = eKEY_TABLEDB.s_LANGUAGE_ENG;

        return (string)TableDB.Instance.GetData(m_LanguageTable,
                                                    (int)eLangID, LanguageKey);
    }

    public string GetLanguageData(eLANGUAGE_DATA eLangID, eLANGUAGE eLang)
    {
        if (eLang == eLANGUAGE.KOREAN)
            LanguageKey = eKEY_TABLEDB.s_LANGUAGE_KOR;
        else
            LanguageKey = eKEY_TABLEDB.s_LANGUAGE_ENG;

        return (string)TableDB.Instance.GetData(m_LanguageTable,
                                                    (int)eLangID, LanguageKey);
    }
}
