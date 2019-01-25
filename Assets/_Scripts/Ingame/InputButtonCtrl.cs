using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtonCtrl : MonoBehaviour
{
    UIWidget[] arrWidget;
    int[] m_arrDepthOrigin;
    int m_iDepthSelected = 200;
    public UISprite m_spriteButton;
    public Color m_colorDisable = Color.gray;
    public TweenScale m_tweenScale;
    public BoxCollider m_collider;

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

    public void DisableButton()
    {
        m_spriteButton.color = m_colorDisable;
        m_collider.enabled = false;
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
