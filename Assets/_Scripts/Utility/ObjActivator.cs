using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjActivator : MonoBehaviour
{
    public GameObject m_obj;
    public float m_fDelay;
    public bool m_bAutoActivation;

    virtual public void OnEnable()
    {
        if(m_bAutoActivation)
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
        m_obj.SetActive(false);

        yield return new WaitForSeconds(m_fDelay);

        m_obj.SetActive(true);
    }

    virtual public void OnDeactivate()
    {
        m_obj.SetActive(false);
    }
}
