using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMgr : MonoBehaviour
{
    public UILabel m_labelScore;

	void Start ()
    {
        MyGlobals.ScoreMgr = this;
	}
	
	public void UpdateScore (int iScore)
    {
        m_labelScore.text = iScore.ToString();
    }
}
