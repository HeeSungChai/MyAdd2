using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStageNum : MonoBehaviour
{
    public UILabel m_label;

	void Start ()
    {
        m_label.text = MyGlobals.StageMgr.StageNum.ToString();
    }
}
