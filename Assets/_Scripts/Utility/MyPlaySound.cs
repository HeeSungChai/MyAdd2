using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlaySound : MonoBehaviour {

    public eSOUND_FX m_eSoundFx;
	
	public void PlaySound()
    {
        MyGlobals.SoundMgr.OnPlayFx(m_eSoundFx);
    }
}
