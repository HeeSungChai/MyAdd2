using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectMgr : MonoBehaviour
{
    public UILabel m_labelReady;
    public UILabel m_labelLevel;
    public UILabel m_labelBestScore;
    //public UILabel m_labelBestScoreValue;
    public UILabel m_labelTotalScore;
    //public UILabel m_labelTotalScoreValue;
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
        m_labelReady.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_AREYOUREADY);
        m_labelLevel.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_LEVEL);
        m_labelBestScore.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_BESTSCORE);
        m_labelTotalScore.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_TOTALSCORE);
        m_labelGetItem.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_GET_ITEM_BTN);
        m_labelStart.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_START_BTN);
    }
}
