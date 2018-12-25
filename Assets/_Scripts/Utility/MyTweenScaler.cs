using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTweenScaler : MonoBehaviour
{
    public TweenScale m_tweenScale;

    private void Awake()
    {
        if (m_tweenScale == null)
            m_tweenScale = GetComponentInChildren<TweenScale>();
    }

    void OnEnable()
    {
        if (m_tweenScale)
        {
            m_tweenScale.ResetToBeginning();
            m_tweenScale.enabled = true;
        }
    }

    void OnDisable()
    {
        if (m_tweenScale)
            m_tweenScale.enabled = false;
    }
}
