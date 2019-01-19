using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtonCtrl : MonoBehaviour
{
    UIWidget[] arrWidget;
    int[] m_arrDepthOrigin;
    int m_iDepthSelected = 200;
    public TweenScale m_tweenScale;

    private void Awake()
    {
        EventListener.AddListener("OnTargetChanged", this);
        EventListener.AddListener("OnDeselectAll", this);
        arrWidget = GetComponentsInChildren<UIWidget>();

        m_arrDepthOrigin = new int[arrWidget.Length];
        for (int i = 0; i < arrWidget.Length; ++i)
        {
            m_arrDepthOrigin[i] = arrWidget[i].depth;
        }

        if (m_tweenScale == null)
            m_tweenScale = GetComponent<TweenScale>();
    }

    public void Select()
    {
        for (int i = 0; i < arrWidget.Length; ++i)
        {
            arrWidget[i].depth += m_iDepthSelected;
        }
        m_tweenScale.PlayForward();
    }

    public void Deselect()
    {
        for (int i = 0; i < arrWidget.Length; ++i)
        {
            arrWidget[i].depth = m_arrDepthOrigin[i];
        }
        m_tweenScale.PlayReverse();
    }

    void OnDeselectAll()
    {
        Deselect();
    }

    void OnTargetChanged()
    {
        Deselect();
    }

    private void OnDestroy()
    {
        EventListener.RemoveListener(this);
    }
}
