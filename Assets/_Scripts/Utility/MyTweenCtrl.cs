using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTweenCtrl : MonoBehaviour
{
    public TweenTransform m_tweenTransform;
    public TweenPosition m_tweenPositon;
    public TweenRotation m_tweenRotation;
    public TweenScale m_tweenScale;
    public TweenColor m_tweenColor;

    virtual public void OnEnable()
    {
        if (m_tweenTransform)
        {
            m_tweenTransform.ResetToBeginning();
            m_tweenTransform.PlayForward();
        }

        if (m_tweenPositon)
        {
            m_tweenPositon.ResetToBeginning();
            m_tweenPositon.PlayForward();
        }

        if (m_tweenRotation)
        {
            m_tweenRotation.ResetToBeginning();
            m_tweenRotation.PlayForward();
        }

        if (m_tweenScale)
        {
            m_tweenScale.ResetToBeginning();
            m_tweenScale.PlayForward();
        }

        if(m_tweenColor)
        {
            m_tweenColor.ResetToBeginning();
            m_tweenColor.PlayForward();
        }
    }
}
