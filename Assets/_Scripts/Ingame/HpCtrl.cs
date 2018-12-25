using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpCtrl : MonoBehaviour
{
    public TweenScale m_tweenScale;

    private void Awake()
    {
        if (m_tweenScale == null)
            m_tweenScale = GetComponentInChildren<TweenScale>();
    }

    //void OnEnable()
    //{
    //    if (!m_tweenScale)
    //        return;

    //    m_tweenScale.ResetToBeginning();
    //    m_tweenScale.enabled = true;
    //}

    public void ActivateHP()
    {
        StopCoroutine("CoroutineDisableHP");

        if (!m_tweenScale)
            return;

        m_tweenScale.ResetToBeginning();
        m_tweenScale.enabled = true;
    }

    public void DisableHP()
    {
        if (!m_tweenScale)
            return;

        m_tweenScale.enabled = true;
        m_tweenScale.PlayReverse();

        StopCoroutine("CoroutineDisableHP");
        StartCoroutine("CoroutineDisableHP");
    }

    IEnumerator CoroutineDisableHP()
    {
        yield return new WaitForSeconds(0.5f);

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
