using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectMgr : MonoBehaviour
{
    public UILabel m_labelReady;
    public UILabel m_labelLevel;
    public UILabel m_labelBestScore;
    public UILabel m_labelBestScoreValue;
    public UILabel m_labelTotalScore;
    public UILabel m_labelTotalScoreValue;
    public UILabel m_labelGetItem;
    public UILabel m_labelStart;

    void Awake ()
    {
        EventListener.AddListener("OnLanguageChanged", this);
        EventListener.AddListener("OnStageChanged", this);
    }

    private void OnEnable()
    {
        OnLanguageChanged();

        int iTotalPoint = 0;
        for (int i = 1; i <= 50; ++i)
        {
            iTotalPoint += PrefsMgr.Instance.GetTotalPoint();
        }
        m_labelTotalScoreValue.text = MyUtility.CommaSeparateDigit(iTotalPoint).ToString();

        OnStageChanged();
    }

    void OnLanguageChanged()
    {
        //if(m_labelReady)
        //    m_labelReady.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_AREYOUREADY);
        //if(m_labelLevel)
        //    m_labelLevel.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_LEVEL);
        //if(m_labelBestScore)
        //    m_labelBestScore.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_BESTSCORE);
        //if(m_labelTotalScore)
        //    m_labelTotalScore.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_TOTALSCORE);
        //if(m_labelGetItem)
        //    m_labelGetItem.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.STAGE_SELECT_POPUP_GET_ITEM_BTN);
        if (m_labelStart)
            m_labelStart.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.LETSGO);
    }

    void OnStageChanged()
    {
        m_labelBestScoreValue.text = MyUtility.CommaSeparateDigit(PrefsMgr.Instance.GetStageScore(MyGlobals.StageNum)).ToString();
    }
}
