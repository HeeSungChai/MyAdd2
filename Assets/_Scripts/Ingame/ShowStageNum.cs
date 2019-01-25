using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStageNum : MonoBehaviour
{
    public UILabel m_label;

	void Start ()
    {
        if(MyGlobals.StageMgr.IsAdventure())
            m_label.text = MyGlobals.StageMgr.StageNum.ToString();
        else
            m_label.text = "";
    }
}
