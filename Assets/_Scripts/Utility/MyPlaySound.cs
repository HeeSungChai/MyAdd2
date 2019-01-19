using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlaySound : MonoBehaviour
{
    public bool m_bPlayFxOnAwakef = false;
    public eSOUND_FX m_eSoundFx;
    public bool m_bPlayBGM;
    public AudioSource m_audioSource;

    private void OnEnable()
    {
        if(m_bPlayFxOnAwakef)
        {
            PlaySound();
        }

        if (m_bPlayBGM)
        {
            if (m_audioSource == null)
                m_audioSource = GetComponentInChildren<AudioSource>();
            PlayBGM();
        }
    }

    public void PlaySound()
    {
        MyGlobals.SoundMgr.OnPlayFx(m_eSoundFx);
    }

    public void PlayBGM()
    {
        if (m_audioSource)
        {
            m_audioSource.volume = PrefsMgr.Instance.GetFloat(PrefsMgr.strVolumnBGM, 1f);
            m_audioSource.Play();
        }
    }
}
