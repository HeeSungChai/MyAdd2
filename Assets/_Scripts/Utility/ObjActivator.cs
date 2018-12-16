using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjActivator : MonoBehaviour
{
    public float m_fDelay;
    public GameObject m_obj;

    virtual public void Start()
    {
        //if (m_obj)
        //    m_obj.SetActive(false);
    }

    virtual public void OnActivate()
    {
        if (m_obj.activeSelf == true)
            return;

        m_obj.SetActive(false);

        StopCoroutine("CoroutineDelayedActivator");
        StartCoroutine("CoroutineDelayedActivator", m_fDelay);
    }

    public IEnumerator CoroutineDelayedActivator(float fDelay)
    {
        float fElased = 0.0f;

        while (fElased < fDelay)
        {
            fElased += Time.deltaTime;

            yield return null;
        }

        m_obj.SetActive(true);
    }
}
