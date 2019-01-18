using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTextCtrl : MonoBehaviour
{
    public GameObject m_objStageText;

	void Start ()
    {
        EventListener.AddListener("OnTargetChanged", this);
    }

    void OnTargetChanged()
    {
        m_objStageText.SetActive(false);
    }
}
