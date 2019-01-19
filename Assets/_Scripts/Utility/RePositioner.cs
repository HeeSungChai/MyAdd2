using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePositioner : MonoBehaviour {

    public Transform m_transform;
    UILabel m_label;
    int m_iValue;
    public bool m_bUseReposition;
    public Vector3 m_vPosStandby;
    public Vector3 m_vPosTarget;
    public bool m_bUseRescale;
    public TweenScale m_tweenScale;
    public float m_fDelay;
    public float m_fDuration;

    private void Start()
    {
        if(m_transform == null)
            m_transform = transform;
        if(m_label == null)
            m_label = GetComponentInChildren<UILabel>();
    }

    public void ResetDigit(int iNewDigit)
    {
        m_iValue = iNewDigit;
        StopCoroutine("CoroutineRePosition");
        StartCoroutine("CoroutineRePosition");
    }

    IEnumerator CoroutineRePosition()
    {
        m_label.text = m_iValue.ToString();

        if (m_bUseReposition)
            m_transform.localPosition = m_vPosStandby;

        if (m_bUseRescale && m_tweenScale)
        {
            m_tweenScale.ResetToBeginning();
            m_tweenScale.PlayForward();
        }

        yield return new WaitForSeconds(m_fDelay);
        
        if (m_bUseReposition)
        {
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

    private void OnDestroy()
    {
        EventListener.RemoveListener(this);
    }
}
