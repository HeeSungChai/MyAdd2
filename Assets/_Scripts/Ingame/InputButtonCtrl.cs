using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtonCtrl : MonoBehaviour
{
    UIWidget[] arrWidget;
    int[] m_arrDepthOrigin;
    int m_iDepthSelected = 200;
    TweenScale m_tweenScale;

    private void Awake()
    {
        EventListener.AddListener("OnLowestChanged", this);
        EventListener.AddListener("OnDeselectAll", this);
        arrWidget = GetComponentsInChildren<UIWidget>();

        m_arrDepthOrigin = new int[arrWidget.Length];
        for(int i = 0; i < arrWidget.Length; ++i)
        {
            m_arrDepthOrigin[i] = arrWidget[i].depth;
        }

        m_tweenScale = GetComponent<TweenScale>();
    }

    public void Select ()
    {
        for (int i = 0; i < arrWidget.Length; ++i)
        {
            arrWidget[i].depth += m_iDepthSelected;
        }
        m_tweenScale.enabled = true;
        m_tweenScale.PlayForward();
        //m_tweenScale.ResetToBeginning();
    }

    public void Deselect ()
    {
        for (int i = 0; i < arrWidget.Length; ++i)
        {
            arrWidget[i].depth = m_arrDepthOrigin[i];
        }
        m_tweenScale.enabled = true;
        m_tweenScale.PlayReverse();
        //m_tweenScale.ResetToBeginning();
    }

    void OnDeselectAll()
    {
        Deselect();
    }

    void OnLowestChanged()
    {
        Deselect();
    }
}
