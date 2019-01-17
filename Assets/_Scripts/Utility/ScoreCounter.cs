using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public UILabel m_label;
    public int m_iStartDigit;
    public int m_iTargetDigit;
    public float m_fDuration;

    public void Init(int iStart, int iTarget, float fDuration)
    {
        m_iStartDigit = iStart;
        m_iTargetDigit = iTarget;
        m_fDuration = fDuration;
    }

    void OnEnable ()
    {
        StartCoroutine("CoroutineCount");
	}

    public void UpdateCounter(int iStart, int iTarget, float fDuration)
    {
        m_iStartDigit = iStart;
        m_iTargetDigit = iTarget;
        m_fDuration = fDuration;

        StopCoroutine("CoroutineCount");
        StartCoroutine("CoroutineCount");
    }
	
	IEnumerator CoroutineCount()
    {
        float fElased = 0f;
        while(fElased < m_fDuration)
        {
            fElased += Time.deltaTime;

            m_label.text = ((int)(Mathf.Lerp(m_iStartDigit, m_iTargetDigit, fElased / m_fDuration))).ToString();

            yield return null;
        }

        m_label.text = MyUtility.CommaSeparateDigit(m_iTargetDigit).ToString();
    }
}
