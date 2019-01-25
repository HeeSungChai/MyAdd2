using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTextCtrl : MonoBehaviour
{
    public GameObject m_objStageText;

	void Start ()
    {
        if (MyGlobals.StageMgr.IsAdventure())
            EventListener.AddListener("OnTargetChanged", this);
        else
            m_objStageText.SetActive(false);
    }

    void OnTargetChanged()
    {
        m_objStageText.SetActive(false);
    }
}
