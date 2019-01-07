using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCHAR_ANIMSTATE
{
    APPEAR,
    IDLE,
    START
}

public class CharAnimCtrl : MonoBehaviour
{
    public Animator m_anim;
    eCHAR_ANIMSTATE m_eCharState;

    void Start ()
    {
        if (m_anim == null)
            m_anim = GetComponentInChildren<Animator>();

        EventListener.AddListener("OnCharState_IDLE", this);
        EventListener.AddListener("OnCharState_START", this);
    }
	
    void OnCharState_IDLE()
    {
        m_anim.SetBool("APPEAR", false);
        m_anim.SetBool("IDLE", true);
        m_anim.SetBool("START", false);
    }

    void OnCharState_START()
    {
        m_anim.SetBool("APPEAR", false);
        m_anim.SetBool("IDLE", false);
        m_anim.SetBool("START", true);
    }
}
