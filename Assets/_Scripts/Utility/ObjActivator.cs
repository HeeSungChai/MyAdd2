using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjActivator : MonoBehaviour
{
    public GameObject m_obj;
    public bool m_bUseAsDeactivator = false;
    public float m_fDelay;
    public bool m_bAutoActivation;
    //public bool m_bAutoDeactivation;
    public bool m_bTimeSkipByTouch;
    public float m_fSkipTime = 1f;
    bool m_bTouched = false;

    virtual public void Start()
    {
        EventListener.AddListener("OnTouched", this);
    }

    virtual public void OnEnable()
    {
        if(m_bUseAsDeactivator)
        {
            StopCoroutine("CoroutineDelayedDeactivator");
            StartCoroutine("CoroutineDelayedDeactivator");
        }
        else if(m_bAutoActivation)
        {
            StopCoroutine("CoroutineDelayedActivator");
            StartCoroutine("CoroutineDelayedActivator");
        }
    }

    virtual public void OnActivate()
    {
        StopCoroutine("CoroutineDelayedActivator");
        StartCoroutine("CoroutineDelayedActivator");
    }

    virtual public IEnumerator CoroutineDelayedActivator()
    {
        if (m_bTimeSkipByTouch)
        {
            StartCoroutine("CoroutineSkippableActivator");
            yield break;
        }

        m_obj.SetActive(false);

        yield return new WaitForSeconds(m_fDelay);

        m_obj.SetActive(true);
    }

    virtual public IEnumerator CoroutineSkippableActivator()
    {
        m_obj.SetActive(false);

        float fElased = 0f;

        while(fElased < m_fDelay)
        {
            fElased += Time.deltaTime;
            if(m_bTouched)
            {
                fElased += m_fSkipTime;
                m_bTouched = false;
            }

            yield return null;
        }

        m_obj.SetActive(true);
    }

    void OnTouched()
    {
        m_bTouched = true;
    }

    virtual public IEnumerator CoroutineDelayedDeactivator()
    {
        yield return new WaitForSeconds(m_fDelay);

        m_obj.SetActive(false);
    }

    virtual public void OnDeactivate()
    {
        m_obj.SetActive(false);
    }

    //private void OnDisable()
    //{
    //    if (m_bAutoDeactivation)
    //        OnDeactivate();
    //}
}
