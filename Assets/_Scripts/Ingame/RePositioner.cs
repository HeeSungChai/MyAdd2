using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePositioner : MonoBehaviour {

    public Transform m_transform;
    UILabel m_label;
    int m_iValue;
    public Vector3 m_vPosStandby;
    public Vector3 m_vPosTarget;
    public float m_fDelay;
    public float m_fDuration;

    private void Awake()
    {
        if(m_transform == null)
            m_transform = transform;
        if(m_label == null)
            m_label = GetComponentInChildren<UILabel>();
    }

    public void ResetDigit(int iNewDigit)
    {
        StopCoroutine("CoroutineRePosition");
        StartCoroutine("CoroutineRePosition");
        m_iValue = iNewDigit;
    }

    IEnumerator CoroutineRePosition()
    {
        m_transform.localPosition = m_vPosStandby;

        yield return new WaitForSeconds(m_fDelay);

        m_label.text = m_iValue.ToString();

        float fElased = 0.0f;
        while (fElased < m_fDuration)
        {
            fElased += Time.deltaTime;
            m_transform.localPosition = Vector3.Lerp(m_vPosStandby, m_vPosTarget, fElased / m_fDuration);

            yield return null;
        }

        m_transform.localPosition = m_vPosTarget;
    }
}
