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
    public bool m_bActivateByLanguage = false;
    public bool m_bApplyOnLanguageChange = false;
    public GameObject m_objENG;
    public GameObject m_objKR;

    virtual public void Start()
    {
        EventListener.AddListener("OnTouched", this);
        if(m_bApplyOnLanguageChange)
            EventListener.AddListener("OnLanguageChanged", this);
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

        Activate(false);

        yield return new WaitForSeconds(m_fDelay);

        if (m_obj)
            m_obj.SetActive(true);

        if (m_bActivateByLanguage)
        {
            if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.ENGLISH && m_objENG)
                m_objENG.SetActive(true);
            else if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.KOREAN && m_objKR)
                m_objKR.SetActive(true);
        }
    }

    virtual public IEnumerator CoroutineSkippableActivator()
    {
        Activate(false);

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

        if (m_obj)
            m_obj.SetActive(true);

        if (m_bActivateByLanguage)
        {
            if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.ENGLISH && m_objENG)
                m_objENG.SetActive(true);
            else if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.KOREAN && m_objKR)
                m_objKR.SetActive(true);
        }
    }

    void OnTouched()
    {
        m_bTouched = true;
    }

    virtual public IEnumerator CoroutineDelayedDeactivator()
    {
        yield return new WaitForSeconds(m_fDelay);

        Activate(false);
    }

    virtual public void OnDeactivate()
    {
        Activate(false);
    }

    void Activate(bool bActivate)
    {
        if (m_obj)
            m_obj.SetActive(bActivate);

        if (m_bActivateByLanguage)
        {
            if (m_objENG)
                m_objENG.SetActive(bActivate);
            if (m_objKR)
                m_objKR.SetActive(bActivate);
        }
    }

    void OnLanguageChanged()
    {
        if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.ENGLISH && m_objENG)
        {
            m_objENG.SetActive(true);
            m_objKR.SetActive(false);
        }
        else if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.KOREAN && m_objKR)
        {
            m_objENG.SetActive(false);
            m_objKR.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
