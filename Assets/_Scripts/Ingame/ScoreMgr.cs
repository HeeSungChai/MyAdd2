using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMgr : MonoBehaviour
{
    public UILabel m_labelScore;
    int m_iCurScore;
    public int m_iStartDigit;
    public int m_iTargetDigit;
    public float m_fDuration;

    void Start ()
    {
        MyGlobals.ScoreMgr = this;
        m_iCurScore = 0;
    }
	
	public void UpdateScore (int iScore)
    {
        m_iStartDigit = m_iCurScore;
        m_iTargetDigit = iScore;
        m_fDuration = 0.5f;

        StopCoroutine("CoroutineCount");
        StartCoroutine("CoroutineCount");

        m_iCurScore = iScore;
        //m_labelScore.text = MyUtility.CommaSeparateDigit(iScore);
    }

    IEnumerator CoroutineCount()
    {
        float fElased = 0f;
        while (fElased < m_fDuration)
        {
            fElased += Time.deltaTime;

            m_labelScore.text = MyUtility.CommaSeparateDigit(((int)(Mathf.Lerp(m_iStartDigit, m_iTargetDigit, fElased / m_fDuration)))).ToString();

            yield return null;
        }

        m_labelScore.text = MyUtility.CommaSeparateDigit(m_iTargetDigit).ToString();
    }
}
