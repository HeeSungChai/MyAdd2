using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectMgr : MonoBehaviour
{
    public UILabel m_labelReady;
    public UILabel m_labelLevel;
    public UILabel m_labelBestScore;
    public UILabel m_labelTotalScore;
    public UILabel m_labelGetItem;
    public UILabel m_labelStart;

    void Awake ()
    {
        EventListener.AddListener("OnLanguageChanged", this);
    }

    private void OnEnable()
    {
        OnLanguageChanged();
    }

    void OnLanguageChanged()
    {
        if(m_labelReady)
            m_labelReady.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_AREYOUREADY);
        if(m_labelLevel)
            m_labelLevel.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_LEVEL);
        if(m_labelBestScore)
            m_labelBestScore.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_BESTSCORE);
        if(m_labelTotalScore)
            m_labelTotalScore.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_TOTALSCORE);
        if(m_labelGetItem)
            m_labelGetItem.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_GET_ITEM_BTN);
        if(m_labelStart)
            m_labelStart.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_START_BTN);
    }
}
