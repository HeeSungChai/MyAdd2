using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimMgr : MonoBehaviour
{
    public float m_fTimeForIdle;

	void Start ()
    {
        StartCoroutine("CoroutineBroadCastIdle");
	}
	
	IEnumerator CoroutineBroadCastIdle()
    {
        yield return new WaitForSeconds(m_fTimeForIdle);

        EventListener.Broadcast("OnCharState_IDLE");
    }

    public void OnStart()
    {
        EventListener.Broadcast("OnCharState_START");
    }
}
