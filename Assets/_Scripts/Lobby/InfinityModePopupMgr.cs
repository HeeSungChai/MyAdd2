using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityModePopupMgr : MonoBehaviour
{
    public UILabel m_labelBestScore;
    public UILabel m_labelBestCombo;

    void Awake()
    {
        EventListener.AddListener("OnLanguageChanged", this);
    }

    void OnLanguageChanged()
    {

    }
}
